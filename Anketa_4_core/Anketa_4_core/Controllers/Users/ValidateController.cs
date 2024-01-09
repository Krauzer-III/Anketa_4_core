using Anketa_4_core.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Controllers.Users
{
    public class ValidateController : Controller
    {

        public bool MotivationTestValidate(string code)
        {
            bool result = true;
            using (var context = new AnketaContext())
            {
                var IDslastperiod = context.AccessForTestables.Include(x => x.Testable).Where(x => x.Testable.Code == code).OrderByDescending(x => x.ID).ToArray();
                if (IDslastperiod.Length > 0)
                {
                    int IDlastperiod = IDslastperiod[0].ID;
                    if (context.MotivationTests.Include(x => x.AccessTest).Any(x => x.AccessTest.ID == IDlastperiod))
                    {
                        result = false;
                    }
                }
            }
            return result;
        }
    }
}
