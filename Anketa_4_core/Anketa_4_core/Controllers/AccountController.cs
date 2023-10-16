using Microsoft.AspNetCore.Mvc;
using Anketa_4_core.Models;

namespace Anketa_4_core.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(MVC_LoginModel model)
        {
            return View();
        }
    }
}
