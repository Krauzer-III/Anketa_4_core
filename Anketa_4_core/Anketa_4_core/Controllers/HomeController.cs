using Anketa_4_core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Anketa_4_core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Точка входа после авторизации. Индивидуальный тестируемый сразу
        /// попадает в личный кабинет; другие роли — в свои рабочие разделы.
        /// Для филиальной учётной записи показывается выбор тестирования.
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("testable_user"))
                    return RedirectToAction("Index", "PersonalCabinet");

                if (User.IsInRole("admin") || User.IsInRole("moderator"))
                    return RedirectToAction("Index", "Testable");

                if (User.IsInRole("verificator"))
                    return RedirectToAction("IndexModerator", "Result");
            }

            return View();
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
