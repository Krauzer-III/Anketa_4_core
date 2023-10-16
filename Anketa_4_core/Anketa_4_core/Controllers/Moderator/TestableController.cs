using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Mvc;

namespace Anketa_4_core.Controllers.Moderator
{
    public class TestableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult TestablesPage()
        {
            var model = new MVC_TestableView[]
                {
                    new MVC_TestableView
                    {
                        Id=1,
                        Code="123",
                        FilialName="fil_1",
                        ReservLevelName="res_1",
                        YearTraining=1,
                        isArchived=false
                    }  ,
                    new MVC_TestableView
                    {
                        Id=1,
                        Code="12345",
                        FilialName="fil_11",
                        ReservLevelName="res_11",
                        YearTraining=2,
                        isArchived=false
                    }        ,
                    new MVC_TestableView
                    {
                        Id=1,
                        Code="1234567",
                        FilialName="fil_12",
                        ReservLevelName="res_12",
                        YearTraining=3,
                        isArchived=false
                    }
                };
            return PartialView(model);
        }
    }
}
