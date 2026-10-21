using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace Anketa_4_core.Controllers
{
    [Authorize]
    public class Test360Controller : Controller
    {
        private readonly AnketaContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public Test360Controller(AnketaContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> StartTest()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            if (await _userManager.IsInRoleAsync(user, "testable_user"))
            {
                var linked = await _context.TestableUsers.Include(x => x.testable)
                    .FirstOrDefaultAsync(x => x.UserName == user.UserName);
                if (linked?.testable == null)
                    return TestError("Не удалось найти тестируемого, привязанного к учётной записи.");

                var selfRole = await _context.RespondentRoles.FirstOrDefaultAsync(x => x.isSelfMark);
                if (selfRole == null) return TestError("Роль самооценки не найдена.");

                var (access, error) = await CheckAccess(linked.testable.Code, selfRole, user.UserName!, true);
                if (access == null) return TestError(error!);

                return View("TestForm", new MVC_Test360Model
                {
                    AccesTestID = access.ID,
                    RespondentRoleID = selfRole.ID,
                    isSelf = true
                });
            }

            return View(new MVC_Test360_StartTest_Model());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartTest(MVC_Test360_StartTest_Model model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            // Личный кабинет никогда не принимает произвольный код.
            if (await _userManager.IsInRoleAsync(user, "testable_user"))
                return RedirectToAction(nameof(StartTest));

            if (!ModelState.IsValid) return View(model);

            var role = await _context.RespondentRoles.FirstOrDefaultAsync(x => x.ID == model.RoleID);
            if (role == null)
            {
                ModelState.AddModelError("", "Выберите роль респондента.");
                return View(model);
            }

            var (access, error) = await CheckAccess(model.Code, role, user.UserName!, false);
            if (access == null)
            {
                ModelState.AddModelError("", error!);
                return View(model);
            }

            return View("TestForm", new MVC_Test360Model
            {
                AccesTestID = access.ID,
                RespondentRoleID = role.ID,
                isSelf = role.isSelfMark
            });
        }

        private async Task<(AccessForTestable? access, string? error)> CheckAccess(
            string code, RespondentRole role, string userName, bool personalAccount)
        {
            var testable = await _context.Testables.Include(x => x.filial)
                .FirstOrDefaultAsync(x => x.Code == code);
            if (testable == null) return (null, "Такого кода не существует.");

            if (!personalAccount)
            {
                var filial = await _context.UserFilials.Include(x => x.Filial)
                    .FirstOrDefaultAsync(x => x.UserName == userName);
                if (filial?.Filial == null)
                    return (null, "Учётной записи не назначен филиал.");
                if (filial.Filial.ID != testable.filial.ID)
                    return (null, "Код тестируемого не принадлежит вашему филиалу.");
            }

            // Проверяем именно последний назначенный период, а не последний активный.
            var access = await _context.AccessForTestables.Include(x => x.Testable)
                .Where(x => x.Testable.ID == testable.ID)
                .OrderByDescending(x => x.ID).FirstOrDefaultAsync();
            if (access == null) return (null, "Тестируемому не назначен период оценки.");
            if (!access.isActive) return (null, "Последний период тестирования закрыт.");

            if (role.isSelfMark && await _context.Test360
                .AnyAsync(x => x.AccessTest.ID == access.ID && x.IsSelfMark))
                return (null, "Самооценка за текущий период уже пройдена.");

            return (access, null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitTest(MVC_Test360Model model)
        {
            if (!ModelState.IsValid) return View("TestForm", model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var personal = await _userManager.IsInRoleAsync(user, "testable_user");
            var role = await _context.RespondentRoles.FirstOrDefaultAsync(x => x.ID == model.RespondentRoleID);
            if (role == null) return TestError("Роль респондента не найдена.");
            if (personal && !role.isSelfMark)
                return TestError("Личная учётная запись может проходить только самооценку.");

            var submittedAccess = await _context.AccessForTestables.Include(x => x.Testable)
                .FirstOrDefaultAsync(x => x.ID == model.AccesTestID);
            if (submittedAccess == null) return TestError("Период оценки не найден.");

            if (personal)
            {
                var linked = await _context.TestableUsers.Include(x => x.testable)
                    .FirstOrDefaultAsync(x => x.UserName == user.UserName);
                if (linked?.testable?.ID != submittedAccess.Testable.ID)
                    return TestError("Доступ к указанному тестируемому запрещён.");
            }

            var (access, error) = await CheckAccess(
                submittedAccess.Testable.Code, role, user.UserName!, personal);
            if (access == null || access.ID != submittedAccess.ID)
                return TestError(error ?? "Период оценки изменился. Начните тест заново.");

            var answer = new Test360_Result
            {
                q01 = model.q01, q02 = model.q02, q03 = model.q03,
                q04 = model.q04, q05 = model.q05, q06 = model.q06,
                q07 = model.q07, q08 = model.q08, q09 = model.q09,
                q10 = model.q10, q11 = model.q11, q12 = model.q12,
                q13 = model.q13, q14 = model.q14, q15 = model.q15
            };

            // Блокировка строки доступа сериализует обновления общего JSON
            // для разных респондентов одного тестируемого.
            await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT 1 FROM \"AccessForTestables\" WHERE \"ID\" = {access.ID} FOR UPDATE");

            if (role.isSelfMark && await _context.Test360
                .AnyAsync(x => x.AccessTest.ID == access.ID && x.IsSelfMark))
                return TestError("Самооценка за текущий период уже пройдена.");

            var entity = new Test360
            {
                AccessTest = access,
                Role = role,
                IsSelfMark = role.isSelfMark,
                DatetimeTest = DateTime.UtcNow,
                results = JsonSerializer.Serialize(answer)
            };
            _context.Test360.Add(entity);
            await _context.SaveChangesAsync();

            var stored = await _context.TestResults.Include(x => x.Access)
                .FirstOrDefaultAsync(x => x.Access.ID == access.ID);
            JSON_ResultModel aggregate;
            if (stored == null)
            {
                aggregate = new JSON_ResultModel
                {
                    AccessID = access.ID,
                    TestableCode = access.Testable.Code
                };
            }
            else
            {
                try
                {
                    aggregate = JsonSerializer.Deserialize<JSON_ResultModel>(stored.Results)
                        ?? throw new JsonException("Пустой результат");
                }
                catch (JsonException)
                {
                    throw new InvalidOperationException(
                        "Не удалось прочитать сохранённый JSON_ResultModel. Обновление отменено.");
                }
            }

            var item = new JSON_Test360(entity, role);
            if (role.isSelfMark)
                aggregate.Add_JSON_Test360_ToReport_Testable(item);
            else
                aggregate.Add_JSON_Test360_ToReport_Respondents(item);

            if (stored == null)
                _context.TestResults.Add(new TestResult { Access = access, Results = aggregate.GetJSON() });
            else
                stored.Results = aggregate.GetJSON();

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            ViewBag.SuccessMessage = "Ваши ответы успешно сохранены!";
            return View("TestCompleted");
        }

        private IActionResult TestError(string message)
        {
            ViewBag.Error = message;
            return View("TestError");
        }
    }
}
