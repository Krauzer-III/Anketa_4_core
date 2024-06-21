using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Controllers.Moderator
{
    public class PeriodController : Controller
    {
        public IActionResult Index()
        {
            using (AnketaContext context = new AnketaContext())
            {
                List<MVC_PeriodList> model = context.TestPeriods.Select(e => new MVC_PeriodList
                {
                    Id = e.ID,
                    PeriodName = e.GroupName
                }).ToList();
                for (int i = 0; i < model.Count; i++)
                {
                    model[i].CountTestables = context.AccessForTestables.Include(e => e.TestPeriod).Count(e => e.TestPeriod.ID == model[i].Id);
                }

                return View(model);
            }
        }

        public IActionResult AddPeriod()
        {
            using (AnketaContext context = new AnketaContext())
            {
                ViewData["testables"] = context.Testables.Select(e => e.Code).ToArray();
                return View();
            }
        }

        [HttpPost]
        public IActionResult AddPeriod(MVC_PeriodAdd model)
        {
            using (AnketaContext context = new AnketaContext())
            {

                if (context.TestPeriods.FirstOrDefault(e => e.GroupName == model.PeriodName) == null)
                {
                    TestPeriod period = new TestPeriod
                    {
                        GroupName = model.PeriodName
                    };
                    context.TestPeriods.Add(period);
                    context.SaveChanges();
                    if (model.TestableNames.Length > 0)
                    {
                        foreach (string cod in model.TestableNames)
                        {
                            var testable = context.Testables.FirstOrDefault(e => e.Code == cod);
                            if (testable != null)
                            {
                                context.AccessForTestables.Add(new AccessForTestable
                                {
                                    isActive = false,
                                    Testable = testable,
                                    TestPeriod = period
                                });
                            }
                            else
                            {
                                ModelState.AddModelError("TestableNames", "Код " +cod + " не существует");
                            }
                        }
                        context.SaveChanges();
                    }
                }
                else
                {
                    ModelState.AddModelError("PeriodName", "Такой период уже существует");
                }

                return View();
            }
        }


        public IActionResult EditPeriod(int ID)
        {
            using (AnketaContext context = new AnketaContext())
            {
                ViewData["testables"] = context.Testables.Select(e => e.Code).ToArray();
                var period = context.TestPeriods.FirstOrDefault(e => e.ID == ID);
                if (period != null)
                {
                    var data = context.AccessForTestables
                        .Include(e => e.TestPeriod)
                        .Include(e => e.Testable)
                        .Where(e => e.TestPeriod.ID == ID).
                        ToList();
                    MVC_PeriodEdit model; 
                    if (data.Count > 0)
                    {
                        model = new MVC_PeriodEdit
                        {
                            Id = period.ID,
                            oldPeriodName = period.GroupName,
                            PeriodName = period.GroupName,
                            oldTestableNames = data.Select(e => e.Testable.Code).ToArray(),
                            TestableNames = data.Select(e => e.Testable.Code).ToArray()
                        };
                    }
                    else
                    {
                        model = new MVC_PeriodEdit
                        {
                            Id = period.ID,
                            oldPeriodName = period.GroupName,
                            PeriodName = period.GroupName,
                            oldTestableNames = new string[0],
                            TestableNames = new string[0]
                        };
                    }    
                    return View(model);
                }
                throw new Exception("Такого периода не существует");
                
            }
        }


        [HttpPost]
        public IActionResult EditPeriod(MVC_PeriodEdit model)
        {
            using (AnketaContext context = new AnketaContext())
            {
                var period = context.TestPeriods.FirstOrDefault(e => e.ID == model.Id);
                if(period != null)
                {
                    //удаляем всех неотмеченных
                    var forDelete = model.oldTestableNames.Except(model.TestableNames).ToList();
                    foreach(var name in forDelete)
                    {
                        var tmpTestable = context.AccessForTestables
                            .Include(e => e.TestPeriod)
                            .Include(e => e.Testable)
                            .FirstOrDefault(e => e.TestPeriod.ID == model.Id && e.Testable.Code == name);
                        if (tmpTestable != null)
                            context.AccessForTestables.Remove(tmpTestable);
                    }

                    //добавляем всех заново отмеченных
                    var forAdding = model.TestableNames.Except(model.oldTestableNames).ToList();
                    foreach (var name in forAdding)
                    {
                        var tmpTestable = context.AccessForTestables
                            .Include(e => e.TestPeriod)
                            .Include(e => e.Testable)
                            .FirstOrDefault(e => e.TestPeriod.ID == model.Id && e.Testable.Code == name);
                        if (tmpTestable != null)
                            context.AccessForTestables.Add(tmpTestable);
                    }

                    //переименовываем период, если был изменён
                    if(model.PeriodName!=model.oldPeriodName)
                    {
                        period.GroupName = model.PeriodName;
                    }
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }


    }
}
