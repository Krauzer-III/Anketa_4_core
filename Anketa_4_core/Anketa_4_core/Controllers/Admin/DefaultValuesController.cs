using Anketa_4_core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Anketa_4_core.Controllers.Admin
{
    public class DefaultValuesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddDefaultValues()
        {
            var val = new AnketaDefaultValues();
            val.AddFilials();
            val.AddReservLevels();
            val.AddRespondentRoles();
            val.AddCompetention_1year();
            val.AddCompetention_2year();
            val.AddKettelTest();
            return View();
        }
    }
}
