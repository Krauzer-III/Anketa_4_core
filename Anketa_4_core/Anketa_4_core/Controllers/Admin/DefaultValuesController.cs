using Anketa_4_core.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Anketa_4_core.Controllers.Admin
{
    [Authorize(Roles = "admin")]
    public class DefaultValuesController : Controller
    {
        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDefaultValues()
        {
            var val = new AnketaDefaultValues();
            val.AddFilials();
            val.AddReservLevels();
            val.AddRespondentRoles();
            val.AddCompetention_1year();
            val.AddCompetention_2year();
            val.AddKettelTest();
            return RedirectToAction(nameof(allAdded));
        }

        [HttpGet]
        public IActionResult allAdded() => View();
    }
}
