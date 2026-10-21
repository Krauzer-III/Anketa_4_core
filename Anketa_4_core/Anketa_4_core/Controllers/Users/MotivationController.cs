using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace Anketa_4_core.Controllers.Users
{
    [Authorize]
    public class MotivationController : Controller
    {
        private readonly AnketaContext _context;
        private readonly UserManager<IdentityUser> _users;

        public MotivationController(AnketaContext context, UserManager<IdentityUser> users)
        {
            _context = context;
            _users = users;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return Challenge();

            if (await _users.IsInRoleAsync(user, "testable_user"))
            {
                var linked = await _context.TestableUsers.Include(x => x.testable)
                    .FirstOrDefaultAsync(x => x.UserName == user.UserName);
                if (linked?.testable == null)
                    return Error("Для учётной записи не найден тестируемый.");

                var (access, error) = await CheckAccess(linked.testable.Code, user.UserName!, true);
                if (access == null) return Error(error!);
                return View("MotivationTest", new MVC_MotivationModel { AccessTestID = access.ID });
            }

            return View(new MVC_MotivationLoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(MVC_MotivationLoginModel model)
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return Challenge();
            if (await _users.IsInRoleAsync(user, "testable_user"))
                return RedirectToAction(nameof(Index));
            if (!ModelState.IsValid) return View(model);

            var (access, error) = await CheckAccess(model.Code, user.UserName!, false);
            if (access == null)
            {
                ModelState.AddModelError("", error!);
                return View(model);
            }
            return View("MotivationTest", new MVC_MotivationModel { AccessTestID = access.ID });
        }

        private async Task<(AccessForTestable? access, string? error)> CheckAccess(
            string code, string userName, bool personal)
        {
            var testable = await _context.Testables.Include(x => x.filial)
                .FirstOrDefaultAsync(x => x.Code == code);
            if (testable == null) return (null, "Такого кода не существует.");

            if (!personal)
            {
                var filial = await _context.UserFilials.Include(x => x.Filial)
                    .FirstOrDefaultAsync(x => x.UserName == userName);
                if (filial?.Filial == null) return (null, "Учётной записи не назначен филиал.");
                if (filial.Filial.ID != testable.filial.ID)
                    return (null, "Код тестируемого не принадлежит вашему филиалу.");
            }

            var access = await _context.AccessForTestables.Include(x => x.Testable)
                .Where(x => x.Testable.ID == testable.ID)
                .OrderByDescending(x => x.ID).FirstOrDefaultAsync();
            if (access == null) return (null, "Период тестирования не назначен.");
            if (!access.isActive) return (null, "Последний период тестирования закрыт.");
            if (await _context.MotivationTests.AnyAsync(x => x.AccessTest.ID == access.ID))
                return (null, "Мотивационный тест за текущий период уже пройден.");
            return (access, null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MotivationTest(MVC_MotivationModel result)
        {
            var user = await _users.GetUserAsync(User);
            if (user == null) return Challenge();

            var personal = await _users.IsInRoleAsync(user, "testable_user");
            var submitted = await _context.AccessForTestables.Include(x => x.Testable)
                .FirstOrDefaultAsync(x => x.ID == result.AccessTestID);
            if (submitted == null) return Error("Период тестирования не найден.");

            if (personal)
            {
                var linked = await _context.TestableUsers.Include(x => x.testable)
                    .FirstOrDefaultAsync(x => x.UserName == user.UserName);
                if (linked?.testable?.ID != submitted.Testable.ID)
                    return Error("Доступ к указанному тестируемому запрещён.");
            }

            var (access, error) = await CheckAccess(submitted.Testable.Code, user.UserName!, personal);
            if (access == null || access.ID != submitted.ID)
                return Error(error ?? "Период тестирования изменился.");

            if (!result.IsAgree && string.IsNullOrWhiteSpace(result.Question_YES_NO))
            {
                ModelState.AddModelError(nameof(result.Question_YES_NO), "Укажите причину отказа.");
                return View(result);
            }

            var raw = new Motivation_Result
            {
                RecomendPosition = result.RecomendPosition,
                Recomendator = result.Recomendator,
                Question_01 = result.Question_01, Question_02 = result.Question_02,
                Question_03 = result.Question_03, Question_04 = result.Question_04,
                Question_05 = result.Question_05, Question_06 = result.Question_06,
                Question_07 = result.Question_07, Question_08 = result.Question_08,
                Question_09 = result.Question_09,
                Question_YES_NO = result.IsAgree ? "Да" : result.Question_YES_NO
            };

            await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT 1 FROM \"AccessForTestables\" WHERE \"ID\" = {access.ID} FOR UPDATE");

            if (await _context.MotivationTests.AnyAsync(x => x.AccessTest.ID == access.ID))
                return Error("Мотивационный тест за текущий период уже пройден.");

            var entity = new MotivationTest
            {
                AccessTest = access,
                DatetimeTest = DateTime.UtcNow,
                IsAgree = result.IsAgree,
                Results = JsonSerializer.Serialize(raw)
            };
            _context.MotivationTests.Add(entity);
            await _context.SaveChangesAsync();

            var stored = await _context.TestResults.Include(x => x.Access)
                .FirstOrDefaultAsync(x => x.Access.ID == access.ID);
            JSON_ResultModel aggregate;
            if (stored == null)
                aggregate = new JSON_ResultModel { AccessID = access.ID, TestableCode = access.Testable.Code };
            else
                aggregate = JsonSerializer.Deserialize<JSON_ResultModel>(stored.Results)
                    ?? throw new InvalidOperationException("Не удалось прочитать сохранённый результат.");

            aggregate.MotivationTest = new JSON_Motivation_Result(entity);
            if (stored == null)
                _context.TestResults.Add(new TestResult { Access = access, Results = aggregate.GetJSON() });
            else
                stored.Results = aggregate.GetJSON();

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return RedirectToAction(nameof(Finish), new { result = true });
        }

        [HttpGet]
        public IActionResult Finish(bool result) => View(result);

        private IActionResult Error(string message)
        {
            ViewBag.Error = message;
            return View("MotivationError");
        }
    }
}
