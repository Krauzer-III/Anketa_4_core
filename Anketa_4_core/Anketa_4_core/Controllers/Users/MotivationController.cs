using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Anketa_4_core.Controllers.Users
{
    public class MotivationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(MVC_MotivationLoginModel model)
        {
            return View();
        }

        public IActionResult MotivationTest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MotivationTest(MVC_MotivationModel result)
        {
            using (var context = new AnketaContext())
            {
                var aft_ID = context.AccessForTestables.FirstOrDefault(a => a.ID == result.AccessTestID);
                if (aft_ID != null)
                {
                    var mt = new MotivationTest
                    {
                        AccessTest = aft_ID,
                        DatetimeTest = DateTime.Now,
                        IsAgree = result.IsAgree,
                        Results = JsonSerializer.Serialize(new Motivation_Result
                        {
                            Question_01 = result.Question_01,
                            Question_02 = result.Question_02,
                            Question_03 = result.Question_03,
                            Question_04 = result.Question_04,
                            Question_05 = result.Question_05,
                            Question_06 = result.Question_06,
                            Question_07 = result.Question_07,
                            Question_08 = result.Question_08,
                            Question_09 = result.Question_09,
                            Question_YES_NO = result.Question_YES_NO,
                            Recomendator = result.Recomendator,
                            RecomendPosition = result.RecomendPosition
                        })
                    };
                }
                else RedirectToAction("Finish", false);
                return RedirectToAction("Finish", true);
            }
        }

        public IActionResult Finish(bool result) => View(result);
    }
}
