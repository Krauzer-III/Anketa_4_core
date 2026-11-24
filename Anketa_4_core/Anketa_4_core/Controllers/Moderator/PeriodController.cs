using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Controllers.Moderator
{
    [Authorize(Roles = "admin,moderator,verificator")]
    public class PeriodController : Controller
    {
        private readonly AnketaContext _db;
        public PeriodController(AnketaContext db) => _db = db;

        // ============== Index ==============
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var items = await _db.TestPeriods
                .AsNoTracking()
                .OrderBy(p => p.GroupName)
                .Select(p => new MVC_TestPeriodListItem
                {
                    Id = p.ID,
                    PeriodName = p.GroupName,
                    CountTestables = _db.AccessForTestables.Count(a => a.TestPeriod.ID == p.ID)
                })
                .ToListAsync();

            return View(items);
        }

        // ============== Details ==============
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var period = await _db.TestPeriods.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);
            if (period == null) return NotFound();

            var links = await _db.AccessForTestables
                .Where(a => a.TestPeriod.ID == id)
                .Include(a => a.Testable).ThenInclude(t => t.filial)
                .AsNoTracking()
                .ToListAsync();

            var vm = new MVC_TestPeriodDetails
            {
                Id = id,
                PeriodName = period.GroupName,
                BoundTestables = links.Select(l => new MVC_TestPeriodBoundTestable
                {
                    TestableId = l.Testable.ID,
                    Code = l.Testable.Code,
                    Filial = l.Testable.filial != null ? l.Testable.filial.FilialName : null,
                    IsActive = l.isActive
                }).OrderBy(x => x.Code).ToList()
            };

            return View(vm);
        }

        // ============== Create ==============
        [HttpGet]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Create()
        {
            var vm = new MVC_TestPeriodCreate
            {
                AllTestables = await _db.Testables
                    .Include(t => t.filial)
                    .AsNoTracking()
                    .OrderBy(t => t.Code)
                    .Select(t => new ValueTuple<int, string, string?>(t.ID, t.Code, t.filial != null ? t.filial.FilialName : null))
                    .ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Create(MVC_TestPeriodCreate vm)
        {
            vm.PeriodName = vm.PeriodName?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(vm.PeriodName))
                ModelState.AddModelError(nameof(vm.PeriodName), "Укажите название периода.");

            if (await _db.TestPeriods.AnyAsync(p => p.GroupName == vm.PeriodName))
                ModelState.AddModelError(nameof(vm.PeriodName), "Такое название периода уже существует.");

            // Отсутствие SelectedTestableIds — нормальная ситуация.
            // Не назначаем никого автоматически, даже если в базе уже есть тестируемые.
            var selectedIds = vm.SelectedTestableIds?.Distinct().ToArray()
                ?? Array.Empty<int>();
            var selectedTestables = new List<Testable>();

            if (selectedIds.Length > 0)
            {
                selectedTestables = await _db.Testables
                    .Where(t => selectedIds.Contains(t.ID))
                    .ToListAsync();

                if (selectedIds.Any(id => id <= 0) ||
                    selectedTestables.Count != selectedIds.Length)
                {
                    ModelState.AddModelError(nameof(vm.SelectedTestableIds),
                        "Один или несколько выбранных тестируемых не найдены.");
                }
            }

            if (!ModelState.IsValid)
            {
                vm.AllTestables = await _db.Testables
                    .AsNoTracking()
                    .OrderBy(t => t.Code)
                    .Select(t => new ValueTuple<int, string, string?>(
                        t.ID, t.Code, t.filial != null ? t.filial.FilialName : null))
                    .ToListAsync();
                return View(vm);
            }

            // Создание периода и необязательных привязок атомарно.
            await using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var period = new TestPeriod { GroupName = vm.PeriodName };
                _db.TestPeriods.Add(period);

                foreach (var testable in selectedTestables)
                {
                    _db.AccessForTestables.Add(new AccessForTestable
                    {
                        Testable = testable,
                        TestPeriod = period,
                        isActive = true
                    });
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return RedirectToAction(nameof(Details), new { id = period.ID });
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError(string.Empty,
                    "Не удалось создать период. Проверьте введённые данные.");
                vm.AllTestables = await _db.Testables
                    .AsNoTracking()
                    .OrderBy(t => t.Code)
                    .Select(t => new ValueTuple<int, string, string?>(
                        t.ID, t.Code, t.filial != null ? t.filial.FilialName : null))
                    .ToListAsync();
                return View(vm);
            }
        }

        // ============== Edit ==============
        [HttpGet]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Edit(int id)
        {
            var period = await _db.TestPeriods.FirstOrDefaultAsync(p => p.ID == id);
            if (period == null) return NotFound();

            var links = await _db.AccessForTestables
                .Where(a => a.TestPeriod.ID == id)
                .Include(a => a.Testable).ThenInclude(t => t.filial)
                .AsNoTracking()
                .ToListAsync();

            var linkedIds = links.Select(l => l.Testable.ID).ToHashSet();

            var vm = new MVC_TestPeriodEdit
            {
                Id = id,
                PeriodName = period.GroupName,
                BoundTestables = links.Select(l => new MVC_TestPeriodBoundTestable
                {
                    TestableId = l.Testable.ID,
                    Code = l.Testable.Code,
                    Filial = l.Testable.filial != null ? l.Testable.filial.FilialName : null,
                    IsActive = l.isActive
                }).OrderBy(x => x.Code).ToList(),
                AllTestables = await _db.Testables
                    .Include(t => t.filial)
                    .AsNoTracking()
                    .Where(t => !linkedIds.Contains(t.ID)) // 🚫 исключаем уже добавленных
                    .OrderBy(t => t.Code)
                    .Select(t => new ValueTuple<int, string, string?>(t.ID, t.Code, t.filial != null ? t.filial.FilialName : null))
                    .ToListAsync()
            };

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Edit(MVC_TestPeriodEdit vm, string? actionBtn)
        {
            if (!ModelState.IsValid)
            {
                vm.AllTestables = await _db.Testables
                    .Include(t => t.filial)
                    .AsNoTracking()
                    .OrderBy(t => t.Code)
                    .Select(t => new ValueTuple<int, string, string?>(t.ID, t.Code, t.filial != null ? t.filial.FilialName : null))
                    .ToListAsync();
                return View(vm);
            }

            var period = await _db.TestPeriods.FirstOrDefaultAsync(p => p.ID == vm.Id);
            if (period == null) return NotFound();

            // обновить название
            period.GroupName = vm.PeriodName;
            await _db.SaveChangesAsync();

            // текущие связи этого периода
            var existing = await _db.AccessForTestables
                .Where(a => a.TestPeriod.ID == vm.Id)
                .Include(a => a.Testable)
                .ToListAsync();

            // обновить активность по присланному списку BoundTestables
            if (vm.BoundTestables != null && vm.BoundTestables.Count > 0)
            {
                var mapIsActive = vm.BoundTestables.ToDictionary(x => x.TestableId, x => x.IsActive);
                foreach (var link in existing)
                {
                    if (mapIsActive.TryGetValue(link.Testable.ID, out var active))
                    {
                        link.isActive = active;
                    }
                }
            }

            // добавить новые привязки
            if (vm.AddTestableIds != null && vm.AddTestableIds.Count > 0)
            {
                var existingIds = existing.Select(e => e.Testable.ID).ToHashSet();
                var newIds = vm.AddTestableIds.Distinct().Where(id => !existingIds.Contains(id)).ToList();

                if (newIds.Count > 0)
                {
                    var testablesToAdd = await _db.Testables.Where(t => newIds.Contains(t.ID)).ToListAsync();
                    foreach (var t in testablesToAdd)
                    {
                        _db.AccessForTestables.Add(new AccessForTestable
                        {
                            Testable = t,
                            TestPeriod = period,
                            isActive = true
                        });
                    }
                }
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }


    }
}
