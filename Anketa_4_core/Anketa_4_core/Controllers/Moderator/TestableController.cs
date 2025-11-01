using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Controllers
{
    [Authorize(Roles = "admin,moderator")]
    public class TestableController : Controller
    {
        private readonly AnketaContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _identityDb;


        public TestableController(
            AnketaContext db,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext identityDb)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _identityDb = identityDb;
        }


        // ================= Index (лента с подгрузкой) =================
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Частичная подгрузка
        [HttpGet]
        public async Task<IActionResult> List(int skip = 0, int take = 30, string? q = null)
        {
            var query = _db.Testables
                .Include(t => t.filial)
                .AsNoTracking()
                .OrderBy(t => t.ID)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var lower = q.ToLower();
                query = query.Where(t => t.Code.ToLower().Contains(lower));
            }

            var items = await query.Skip(skip).Take(take)
                .Select(t => new MVC_TestableListItem
                {
                    Id = t.ID,
                    Code = t.Code,
                    Filial = t.filial != null ? t.filial.FilialName : null,
                    YearTraining = t.YearTraining,
                    IsArchived = t.isArchived
                })
                .ToListAsync();

            return PartialView("_TestableRows", items);
        }

        // ================= Create =================

        [HttpGet]
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> Create()
        {
            var single = new MVC_TestableEditFull();
            await FillLookupsAsync(single);
            single.Code = await GetNextCodeAsync();


            var vm = new MVC_TestableAddPageVM
            {
                Single = single,
                Import = new MVC_TestableImportVM(),
                FreeUsers = await GetFreeTestableUsersAsync(),
                FilialNames = await _db.Filials.AsNoTracking().OrderBy(f => f.FilialName)
                     .Select(f => f.FilialName).ToListAsync(),
                ReservLevels = await _db.ReservLevels.AsNoTracking().OrderBy(r => r.ID)
                     .Select(r => new ValueTuple<int, string>(r.ID, r.ReservLevelName)).ToListAsync(),
                TestPeriods = await _db.TestPeriods.AsNoTracking().OrderBy(p => p.ID)
                     .Select(p => new ValueTuple<int, string>(p.ID, p.GroupName)).ToListAsync()
            };
            return View("Create", vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(Prefix = "Single")] MVC_TestableEditFull single)
        {
            // 1) Подготовка данных
            if (string.IsNullOrWhiteSpace(single.Code))
                single.Code = await GetNextCodeAsync();

            // 2) Уникальность кода
            if (await _db.Testables.AsNoTracking().AnyAsync(t => t.Code == single.Code))
                ModelState.AddModelError("Code", $"Код '{single.Code}' уже существует"); // ключ без "Single." т.к. Prefix="Single"

            if (!ModelState.IsValid)
            {
                // Собираем wrapper для возврата в ту же страницу с табами
                await FillLookupsAsync(single);
                var vm = new MVC_TestableAddPageVM
                {
                    Single = single,
                    FreeUsers = await GetFreeTestableUsersAsync(),
                    FilialNames = await _db.Filials.AsNoTracking().OrderBy(f => f.FilialName).Select(f => f.FilialName).ToListAsync(),
                    ReservLevels = await _db.ReservLevels.AsNoTracking().OrderBy(r => r.ID)
                                        .Select(r => new ValueTuple<int, string>(r.ID, r.ReservLevelName)).ToListAsync(),
                    TestPeriods = await _db.TestPeriods.AsNoTracking().OrderBy(p => p.ID)
                                        .Select(p => new ValueTuple<int, string>(p.ID, p.GroupName)).ToListAsync()
                };
                return View("Create", vm);
            }

            // 3) Создание сущности
            var entity = new Testable
            {
                Code = single.Code,
                YearTraining = single.Year > 0 ? single.Year : 1,
                isArchived = false,
                filial = single.Filial > 0 ? await _db.Filials.FindAsync(single.Filial) : null,
                reservLevel = single.RezervLevel > 0 ? await _db.ReservLevels.FindAsync(single.RezervLevel) : null
            };
            _db.Testables.Add(entity);
            await _db.SaveChangesAsync();

            // 4) Привязки
            await UpsertTestableUser(entity, single.UserName);

            if (single.SelectedTestPeriodId == null)
            {
                var lastPeriodId = await _db.TestPeriods.OrderByDescending(p => p.ID).Select(p => p.ID).FirstOrDefaultAsync();
                if (lastPeriodId > 0) single.SelectedTestPeriodId = lastPeriodId;
            }

            await UpsertAccessForTestable(entity.ID,  single.SelectedTestPeriodId!.Value );

            return RedirectToAction(nameof(Details), new { id = entity.ID });
        }




        [HttpGet]
        public async Task<string> NextCode()
        {
            return await GetNextCodeAsync();
        }

        private async Task<string> GetNextCodeAsync()
        {
            var codes = await _db.Testables.AsNoTracking().Select(t => t.Code).ToListAsync();
            int max = 0;
            foreach (var c in codes)
            {
                if (int.TryParse(c, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int n))
                    if (n > max) max = n;
            }
            return (max + 1).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }


        [HttpGet]
        public IActionResult Import() => View(new MVC_TestableImportVM());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(MVC_TestableImportVM vm)
        {
            var errors = new List<string>();
            var toCreate = new List<(Testable t, int periodId, string? userName)>();

            if (vm.File == null || vm.File.Length == 0)
            {
                errors.Add("Файл не выбран");
            }
            else
            {
                using var reader = new StreamReader(vm.File.OpenReadStream(), Encoding.UTF8);
                string? header = await reader.ReadLineAsync();
                if (header == null)
                {
                    errors.Add("Пустой файл");
                }
                else
                {
                    // Ожидаемые колонки: code, filial, rezerv_level, testperiod, username
                    int line = 1;
                    var allCodes = new HashSet<string>(await _db.Testables.AsNoTracking().Select(x => x.Code).ToListAsync());
                    var batchCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    var filialByName = await _db.Filials.AsNoTracking().ToDictionaryAsync(f => f.FilialName, f => f.ID);
                    var periodByName = await _db.TestPeriods.AsNoTracking().ToDictionaryAsync(p => p.GroupName, p => p.ID);
                    int lastPeriodId = await _db.TestPeriods.OrderByDescending(p => p.ID).Select(p => p.ID).FirstOrDefaultAsync();

                    var freeUsers = await GetFreeTestableUsersAsync();
                    var freeUserSet = new HashSet<string>(freeUsers, StringComparer.OrdinalIgnoreCase);

                    while (!reader.EndOfStream)
                    {
                        line++;
                        var raw = await reader.ReadLineAsync();
                        if (raw == null) break;
                        var parts = raw.Split(',');
                        if (parts.Length < 5)
                        {
                            errors.Add($"Строка {line}: ожидается 5 столбцов (code,filial,rezerv_level,testperiod,username)");
                            continue;
                        }
                        var code = parts[0].Trim();
                        var filialName = parts[1].Trim();
                        var rezervStr = parts[2].Trim();
                        var periodName = parts[3].Trim();
                        var username = parts[4].Trim();

                        if (string.IsNullOrWhiteSpace(code)) { errors.Add($"Строка {line}: пустой code"); continue; }
                        if (allCodes.Contains(code) || !batchCodes.Add(code)) { errors.Add($"Строка {line}: код '{code}' уже существует или повторяется в файле"); continue; }

                        int filialId = 0;
                        if (!string.IsNullOrEmpty(filialName))
                        {
                            if (!filialByName.TryGetValue(filialName, out filialId))
                            { errors.Add($"Строка {line}: филиал '{filialName}' не найден"); continue; }
                        }

                        if (!int.TryParse(rezervStr, out int rezervId))
                        { errors.Add($"Строка {line}: rezerv_level должен быть числом (ID)"); continue; }
                        if (rezervId > 0 && await _db.ReservLevels.FindAsync(rezervId) == null)
                        { errors.Add($"Строка {line}: уровень резерва ID={rezervId} не найден"); continue; }

                        int period = new();
                        if (string.IsNullOrEmpty(periodName))
                        {
                            if (lastPeriodId > 0) period = lastPeriodId;
                        }
                        else
                        {
                            if (!periodByName.TryGetValue(periodName, out int pid))
                            { errors.Add($"Строка {line}: период '{periodName}' не найден"); continue; }
                            period = pid;
                        }

                        string? userToBind = null;
                        if (!string.IsNullOrEmpty(username))
                        {
                            if (!freeUserSet.Contains(username))
                            { errors.Add($"Строка {line}: пользователь '{username}' недоступен (нет роли testable_user или уже привязан)"); continue; }
                            userToBind = username;
                        }

                        var t = new Testable
                        {
                            Code = code,
                            YearTraining = vm.DefaultYear > 0 ? vm.DefaultYear : DateTime.UtcNow.Year,
                            isArchived = false,
                            filial = filialId > 0 ? await _db.Filials.FindAsync(filialId) : null,
                            reservLevel = rezervId > 0 ? await _db.ReservLevels.FindAsync(rezervId) : null
                        };
                        toCreate.Add((t, period, userToBind));
                    }
                }
            }

            if (errors.Count > 0)
            {
                ModelState.AddModelError(string.Empty, string.Join("\n", errors));
                return View(vm); // НИЧЕГО не сохраняем, показываем все ошибки
            }

            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                foreach (var item in toCreate)
                {
                    _db.Testables.Add(item.t);
                    await _db.SaveChangesAsync();

                    if (!string.IsNullOrEmpty(item.userName))
                        await UpsertTestableUser(item.t, item.userName);

                    await UpsertAccessForTestable(item.t.ID, item.periodId);
                }
                await tx.CommitAsync();
                TempData["ImportOk"] = $"Добавлено записей: {toCreate.Count}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                ModelState.AddModelError(string.Empty, "Ошибка сохранения: " + ex.Message);
                return View(vm);
            }
        }


        private async Task<List<string>> GetFreeTestableUsersAsync()
        {
            // NB: нужен ApplicationDbContext в конструкторе и RoleManager/Identity
            var role = await _roleManager.FindByNameAsync("testable_user");
            if (role == null) return new List<string>();

            var roleUsers = await (from ur in _identityDb.UserRoles
                                   join u in _identityDb.Users on ur.UserId equals u.Id
                                   where ur.RoleId == role.Id
                                   select u.UserName!).ToListAsync();

            var busy = await _db.Set<TestableUser>().Select(tu => tu.UserName).ToListAsync();
            var busySet = new HashSet<string>(busy, StringComparer.OrdinalIgnoreCase);
            return roleUsers.Where(u => !busySet.Contains(u)).OrderBy(u => u).ToList();
        }




        // ================= Edit =================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var t = await _db.Testables
                .Include(x => x.filial)
                .Include(x => x.reservLevel)
                .FirstOrDefaultAsync(x => x.ID == id);

            if (t == null) return NotFound();

            var accessId = await _db.AccessForTestables
                .Where(a => a.Testable.ID == id)
                .Select(a => (int?)a.TestPeriod.ID)
                .FirstOrDefaultAsync() ?? 0;


            var userName = await _db.Set<TestableUser>()
                .Where(u => u.testable.ID == id)
                .Select(u => u.UserName)
                .FirstOrDefaultAsync();

            var vm = new MVC_TestableEditFull
            {
                ID = t.ID,
                oldCode = t.Code,
                Code = t.Code,
                Year = t.YearTraining,
                Filial = t.filial?.ID ?? 0,
                RezervLevel = t.reservLevel?.ID ?? 0,
                UserName = userName,
                SelectedTestPeriodId = accessId
            };

            await FillLookupsAsync(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MVC_TestableEditFull vm)
        {
            if (id != vm.ID) return BadRequest();
            if (!ModelState.IsValid)
            {
                await FillLookupsAsync(vm);
                return View(vm);
            }

            var t = await _db.Testables
                .Include(x => x.filial)
                .Include(x => x.reservLevel)
                .FirstOrDefaultAsync(x => x.ID == id);
            if (t == null) return NotFound();

            t.Code = vm.Code;
            t.YearTraining = vm.Year;
            t.filial = vm.Filial > 0 ? await _db.Filials.FindAsync(vm.Filial) : null;
            t.reservLevel = vm.RezervLevel > 0 ? await _db.ReservLevels.FindAsync(vm.RezervLevel) : null;

            await _db.SaveChangesAsync();

            // Обновить привязку аккаунта
            await UpsertTestableUser(t, vm.UserName);

            // Обновить периоды
            await UpsertAccessForTestable(t.ID, vm.SelectedTestPeriodId);

            return RedirectToAction(nameof(Details), new { id });
        }

        // ================= Details =================
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var t = await _db.Testables
                .Include(x => x.filial)
                .Include(x => x.reservLevel)
                .FirstOrDefaultAsync(x => x.ID == id);
            if (t == null) return NotFound();

            // Подгружаем периоды из БД и конвертируем в кортежи уже в памяти
            var periodRows = await _db.AccessForTestables
                .Where(a => a.Testable.ID == id)
                .Select(a => new { a.TestPeriod.ID, a.TestPeriod.GroupName })
                .ToListAsync();
            var boundPeriods = periodRows
                .Select(x => ((int)x.ID, (string)x.GroupName))
                .ToList();

            var vm = new MVC_TestableDetailsFull
            {
                ID = t.ID,
                Code = t.Code,
                Year = t.YearTraining,
                Filial = t.filial?.ID ?? 0,
                RezervLevel = t.reservLevel?.ID ?? 0,
                UserName = await _db.Set<TestableUser>().Where(u => u.testable.ID == id).Select(u => u.UserName).FirstOrDefaultAsync(),
                LK_Data = await _db.Set<TestableUser>().Where(u => u.testable.ID == id).Select(u => u.LK_Data).FirstOrDefaultAsync(),
                BoundTestPeriods = boundPeriods,
                Reports = new List<string> { "Индивидуальный отчёт (PDF)", "Сводный отчёт (XLSX)" }
            };
            return View(vm);
        }

        // === Заглушки действий по тестам/ЛК (заполните позже) ===
        [HttpPost]
        public IActionResult GenerateReportPdf(int id)
        {
            // TODO: сгенерировать и отдать PDF
            return NoContent();
        }

        [HttpPost]
        public IActionResult GenerateReportXlsx(int id)
        {
            // TODO: сгенерировать и отдать XLSX
            return NoContent();
        }

        [HttpPost]
        public IActionResult SaveLKData(int id, string lkData)
        {
            // TODO: сохранить данные личного кабинета
            return NoContent();
        }

        // ================= Delete =================
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var t = await _db.Testables
                .Include(x => x.filial)
                .Include(x => x.reservLevel)
                .FirstOrDefaultAsync(x => x.ID == id);
            if (t == null) return NotFound();

            // Подгружаем периоды из БД и конвертируем в кортежи уже в памяти
            var delPeriodRows = await _db.AccessForTestables
                .Where(a => a.Testable.ID == id)
                .Select(a => new { a.TestPeriod.ID, a.TestPeriod.GroupName })
                .ToListAsync();
            var delBoundPeriods = delPeriodRows
                .Select(x => ((int)x.ID, (string)x.GroupName))
                .ToList();

            var vm = new MVC_TestableDeletePreview
            {
                Id = t.ID,
                Code = t.Code,
                YearTraining = t.YearTraining,
                IsArchived = t.isArchived,
                Filial = t.filial?.FilialName,
                ReservLevel = t.reservLevel?.ReservLevelName,
                UserName = await _db.Set<TestableUser>().Where(u => u.testable.ID == id).Select(u => u.UserName).FirstOrDefaultAsync(),
                LK_Data = await _db.Set<TestableUser>().Where(u => u.testable.ID == id).Select(u => u.LK_Data).FirstOrDefaultAsync(),
                BoundTestPeriods = delBoundPeriods
            };
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var t = await _db.Testables.FirstOrDefaultAsync(x => x.ID == id);
            if (t == null) return NotFound();

            // Сбор данных для архива
            // TODO: Сохранить все связанные данные в архив (внешнее хранилище/таблица) перед удалением

            // Удалить связи AccessForTestable
            var access = await _db.AccessForTestables.Where(a => a.Testable.ID == id).ToListAsync();
            _db.AccessForTestables.RemoveRange(access);

            // Удалить TestableUser
            var tu = await _db.Set<TestableUser>().Where(u => u.testable.ID == id).ToListAsync();
            _db.RemoveRange(tu);

            // Удалить UserFilial по UserName из TestableUser
            var tuUserNames = tu.Select(x => x.UserName).ToList();
            if (tuUserNames.Count > 0)
            {
                var uf = await _db.UserFilials.Where(u => u.UserName != null && tuUserNames.Contains(u.UserName)).ToListAsync();
                _db.UserFilials.RemoveRange(uf);
            }

            _db.Testables.Remove(t);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ================= Helpers =================
        private async Task FillLookupsAsync(MVC_TestableEditFull vm)
        {
            vm.Filials = await _db.Filials.AsNoTracking().OrderBy(x => x.FilialName).Select(x => new ValueTuple<int, string>(x.ID, x.FilialName)).ToListAsync();
            vm.ReservLevels = await _db.ReservLevels.AsNoTracking().OrderBy(x => x.ReservLevelName).Select(x => new ValueTuple<int, string>(x.ID, x.ReservLevelName)).ToListAsync();
            vm.TestPeriods = await _db.TestPeriods.AsNoTracking().OrderBy(x => x.GroupName).Select(x => new ValueTuple<int, string>(x.ID, x.GroupName)).ToListAsync();
        }

        private async Task UpsertTestableUser(Testable testable, string? userName)
        {
            var existing = await _db.Set<TestableUser>().FirstOrDefaultAsync(u => u.testable.ID == testable.ID);
            if (string.IsNullOrWhiteSpace(userName))
            {
                if (existing != null)
                {
                    _db.Remove(existing);
                    await _db.SaveChangesAsync();
                }
                return;
            }

            // Проверим, что такой пользователь есть в Identity (не обязательно, но полезно)
            var identityUser = await _userManager.FindByNameAsync(userName);
            if (identityUser == null)
            {
                // Можно добавить ModelState ошибку — по требованиям оставим без ошибки
            }

            if (existing == null)
            {
                existing = new TestableUser { testable = testable, UserName = userName, LK_Data = null };
                _db.Add(existing);
            }
            else
            {
                existing.UserName = userName;
            }
            await _db.SaveChangesAsync();
        }

        private async Task UpsertAccessForTestable(int testableId, int? selectedPeriodId)
        {
            if (selectedPeriodId == null)
                return;
            var existing = await _db.AccessForTestables.Where(a => a.Testable.ID == testableId).ToListAsync();
            var existingIds = existing.Select(e => e.TestPeriod.ID).ToHashSet();
            var targetId = selectedPeriodId;

            if (!existingIds.Any(e => e == targetId))
            {
                var period = await _db.TestPeriods.FindAsync(targetId);
                if (period != null)
                {
                    var testable = await _db.Testables.FindAsync(testableId);
                    if (testable != null)
                    {
                        _db.AccessForTestables.Add(new AccessForTestable
                        {
                            Testable = testable,
                            TestPeriod = period,
                            isActive = true
                        });
                    }
                }
            }


            await _db.SaveChangesAsync();
        }
    }
}

