using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Anketa_4_core.Controllers.Admin
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["rolemanager"] = roleManager;
            return View(userManager);
        }

        // The editor does not have an implementation yet; no broken link is exposed.
        [HttpGet]
        public IActionResult UserEditor(string UserID) => RedirectToAction(nameof(Index));
    }
}
