using Anketa_4_core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Anketa_4_core.Controllers
{
    public class HomeController : Controller
    {
        // The personal account always starts in its own cabinet, regardless of
        // whether the user opens the site root or navigates to Home/Index.
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true && User.IsInRole("testable_user"))
                return RedirectToAction("Index", "PersonalCabinet");

            return View();
        }

        [HttpGet]
        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
    }
}
