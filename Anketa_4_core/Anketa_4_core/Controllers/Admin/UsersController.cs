using Anketa_4_core.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Controllers.Admin
{
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            ViewData["rolemanager"] = roleManager;
            return View(userManager);
        }

        [HttpGet]
        public IActionResult UserEditor(string UserID)
        {
            return View();
        }
    }
}
