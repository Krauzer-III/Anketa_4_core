using Anketa_4_core.Data;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Anketa_4_core.Data.AnketaModels;
using System.ComponentModel.DataAnnotations;

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

            // 🔹 Проверяем, если это testable_user — пропускаем ввод кода
            if (await _userManager.IsInRoleAsync(user, "testable_user"))
            {
                var testableUser = _context.TestableUsers
                    .Include(t => t.testable)
                    .ThenInclude(f => f.filial)
                    .FirstOrDefault(t => t.UserName == user.UserName);

                if (testableUser == null || testableUser.testable == null)
                {
                    ViewBag.Error = "Не удалось найти данные для этого пользователя.";
                    return View("Error");
                }

                // Ищем роль "Самооценка"
                var selfRole = _context.RespondentRoles.FirstOrDefault(r => r.isSelfMark);
                if (selfRole == null)
                {
                    ViewBag.Error = "Роль самооценки не найдена. Обратитесь к администратору.";
                    return View("Error");
                }

                // Ищем доступ для тестируемого
                var access = _context.AccessForTestables
                    .Include(a => a.Testable)
                    .FirstOrDefault(a => a.Testable.Code == testableUser.testable.Code && a.isActive);

                if (access == null)
                {
                    ViewBag.Error = "Нет активного теста для данного пользователя.";
                    return View("Error");
                }

                // Создаём модель теста и сразу рендерим форму
                var testModel = new MVC_Test360Model
                {
                    AccesTestID = access.ID,
                    RespondentRoleID = selfRole.ID,
                    isSelf = true
                };

                return View("TestForm", testModel);
            }

            // 🔹 Иначе — обычный сценарий: ввод кода вручную
            return View(new MVC_Test360_StartTest_Model());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartTest(MVC_Test360_StartTest_Model model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var validationResults = model.Validate(new ValidationContext(model));
            foreach (var v in validationResults)
                ModelState.AddModelError("", v.ErrorMessage);

            if (!ModelState.IsValid)
                return View(model);

            var access = _context.AccessForTestables
                .Include(a => a.Testable)
                .FirstOrDefault(a => a.Testable.Code == model.Code && a.isActive);

            if (access == null)
            {
                ModelState.AddModelError("", "Нет активного теста для данного кода.");
                return View(model);
            }

            var testModel = new MVC_Test360Model
            {
                AccesTestID = access.ID,
                RespondentRoleID = model.RoleID,
                isSelf = false
            };

            return View("TestForm", testModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitTest(MVC_Test360Model model)
        {
            if (!ModelState.IsValid)
                return View("TestForm", model);

            var result = new Test360_Result
            {
                q01 = model.q01,
                q02 = model.q02,
                q03 = model.q03,
                q04 = model.q04,
                q05 = model.q05,
                q06 = model.q06,
                q07 = model.q07,
                q08 = model.q08,
                q09 = model.q09,
                q10 = model.q10,
                q11 = model.q11,
                q12 = model.q12,
                q13 = model.q13,
                q14 = model.q14,
                q15 = model.q15
            };

            string json = JsonConvert.SerializeObject(result);

            var access = _context.AccessForTestables.FirstOrDefault(a => a.ID == model.AccesTestID);
            if (access == null)
            {
                ModelState.AddModelError("", "Не удалось найти тест-доступ.");
                return View("TestForm", model);
            }

            var entity = new TestResult
            {
                Access = access,
                Results = json
            };

            _context.TestResults.Add(entity);
            _context.SaveChanges();

            ViewBag.SuccessMessage = "Ваши ответы успешно сохранены!";
            return View("TestCompleted");
        }
    }
}





/*using Microsoft.AspNetCore.Mvc;

namespace Anketa_4_core.Controllers.Users
{
    public class Test360Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TestPage(string testableCode, int RoleID)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Final()
        {
            return View();
        }
    }
}
                                      */

