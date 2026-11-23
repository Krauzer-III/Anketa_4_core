using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Services;
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
        public async Task<IActionResult> Create(MVC_TestPeriodCreate vm)
        {
            vm.PeriodName = vm.PeriodName?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(vm.PeriodName))
                ModelState.AddModelError(nameof(vm.PeriodName), "Укажите название периода.");
            if (await _db.TestPeriods.AnyAsync(x => x.GroupName.ToLower() == vm.PeriodName.ToLower()))
                ModelState.AddModelError(nameof(vm.PeriodName), "Такое название периода уже существует.");

            var ids = (vm.SelectedTestableIds ?? new List<int>()).Distinct().OrderBy(x => x).ToArray();
            var found = await _db.Testables.CountAsync(x => ids.Contains(x.ID));
            if (ids.Any(x => x <= 0) || found != ids.Length)
                ModelState.AddModelError(nameof(vm.SelectedTestableIds), "Список содержит несуществующего тестируемого.");

            if (!ModelState.IsValid)
            {
                vm.AllTestables = await LoadTestablesAsync();
                return View(vm);
            }

            await using var tx = await _db.Database.BeginTransactionAsync();
            var period = new TestPeriod { GroupName = vm.PeriodName };
            _db.TestPeriods.Add(period);
            await _db.SaveChangesAsync();

            var assignments = new PeriodAssignmentService(_db);
            foreach (var id in ids)
                await assignments.AssignAsync(id, period.ID);

            await tx.CommitAsync();
            return RedirectToAction(nameof(Details), new { id = period.ID });
        }

        // ============== Edit ==============
        [HttpGet]
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
        public async Task<IActionResult> Edit(MVC_TestPeriodEdit vm, string? actionBtn)
        {
            vm.PeriodName = vm.PeriodName?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(vm.PeriodName))
                ModelState.AddModelError(nameof(vm.PeriodName), "Укажите название периода.");
            if (await _db.TestPeriods.AnyAsync(x => x.ID != vm.Id &&
                 x.GroupName.ToLower() == vm.PeriodName.ToLower()))
                ModelState.AddModelError(nameof(vm.PeriodName), "Такое название периода уже существует.");

            // Invalid/duplicate form data must never throw or partially save a rename.
            var bound = vm.BoundTestables ?? new List<MVC_TestPeriodBoundTestable>();
            if (bound.GroupBy(x => x.TestableId).Any(x => x.Count() > 1))
                ModelState.AddModelError(nameof(vm.BoundTestables), "Тестируемый повторяется в списке.");

            var addIds = (vm.AddTestableIds ?? new List<int>()).Distinct().OrderBy(x => x).ToArray();
            if (addIds.Any(x => x <= 0) ||
                await _db.Testables.CountAsync(x => addIds.Contains(x.ID)) != addIds.Length)
                ModelState.AddModelError(nameof(vm.AddTestableIds), "Список содержит несуществующего тестируемого.");

            var period = await _db.TestPeriods.FirstOrDefaultAsync(x => x.ID == vm.Id);
            if (period == null) return NotFound();

            var existing = await _db.AccessForTestables
                .Include(x => x.Testable)
                .Where(x => x.TestPeriod.ID == vm.Id)
                .ToListAsync();
            var existingIds = existing.Select(x => x.Testable.ID).ToHashSet();
            if (bound.Any(x => !existingIds.Contains(x.TestableId)))
                ModelState.AddModelError(nameof(vm.BoundTestables), "Недопустимый тестируемый в списке привязок.");

            if (!ModelState.IsValid)
            {
                vm.AllTestables = await LoadTestablesAsync(existingIds);
                return View(vm);
            }

            await using var tx = await _db.Database.BeginTransactionAsync();
            period.GroupName = vm.PeriodName;
            var activity = bound.ToDictionary(x => x.TestableId, x => x.IsActive);
            foreach (var link in existing)
                if (activity.TryGetValue(link.Testable.ID, out var active))
                    link.isActive = active;

            // A selection from another period is a MOVE, not another link.
            var assignments = new PeriodAssignmentService(_db);
            foreach (var id in addIds)
                await assignments.AssignAsync(id, period.ID);

            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }

        private async Task<List<(int id, string name, string? filial)>> LoadTestablesAsync(
            IReadOnlyCollection<int>? excludedIds = null)
        {
            var ids = excludedIds?.ToArray() ?? Array.Empty<int>();
            return await _db.Testables.AsNoTracking()
                .Where(x => !ids.Contains(x.ID))
                .OrderBy(x => x.Code)
                .Select(x => new ValueTuple<int, string, string?>(
                    x.ID, x.Code, x.filial != null ? x.filial.FilialName : null))
                .ToListAsync();
        }


    }
}
