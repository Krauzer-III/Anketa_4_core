using Microsoft.AspNetCore.Mvc;

namespace Anketa_4_core.Controllers.Users
{
    public class PersonalCabinetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
