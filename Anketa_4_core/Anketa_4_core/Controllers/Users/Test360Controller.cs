using Microsoft.AspNetCore.Mvc;

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
