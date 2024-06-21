using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Anketa_4_core.Controllers.Moderator
{
    public class TestableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult TestablesPage(string code, int[] filialID, int[] reservID, int[] year)
        {
            var model = new AnketaContext().Testables
                .Include(t => t.reservLevel).Include(f => f.filial).ToList();
            if (code != "" && code != null)
                model = model.Where(t => t.Code.Contains(code)).ToList();
            if (filialID.Length > 0)
                model = model.Where(t => filialID.Contains(t.filial.ID)).ToList();
            if (reservID.Length > 0)
                model = model.Where(t => reservID.Contains(t.reservLevel.ID)).ToList();
            if (year.Length>0)
                model = model.Where(t => year.Contains(t.YearTraining)).ToList();
            return PartialView(model.Select(t => new MVC_TestableView
            {
                Id = t.ID,
                Code = t.Code,
                FilialName = t.filial.FilialName,
                ReservLevelName = t.reservLevel.ReservLevelName,
                YearTraining = t.YearTraining
            }).ToList());
        }

        public IActionResult AddTestable() => View();

        [HttpPost]
        public IActionResult AddTestable(MVC_TestableAdd model)
        {

            using (var context = new AnketaContext())
            {
                if (context.Testables.Any(t => t.Code == model.Code))
                {
                    ModelState.AddModelError("Code", "Код не должен повторяться");
                }
                if (ModelState.IsValid)
                {
                    var filial = context.Filials.First(f => f.ID == model.Filial);
                    var reservLevel = context.ReservLevels.First(f => f.ID == model.RezervLevel);
                    context.Testables.Add(new Testable
                    {
                        Code = model.Code,
                        filial = filial,
                        reservLevel = reservLevel,
                        isArchived = false,
                        YearTraining = model.Year
                    });
                    context.SaveChanges();
                }
                else return View();
            }
            return RedirectToAction("Finish");
        }

        [HttpPost]
        public ActionResult AddTestableMany(MVCTestableAddMany model)
        {
            using (var context = new AnketaContext())
            {
                List<MVC_TestableAdd> list = new List<MVC_TestableAdd>();
                List<string[]> lines = Regex.Split(model.data, "\r\n|\r|\n").Select(line => line.Split(model.separator)).ToList();
                foreach (var line in lines)
                {
                    MVC_TestableAdd item = new MVC_TestableAdd();
                    if (context.Testables.FirstOrDefault(t => t.Code == line[0]) != null)
                        ModelState.AddModelError("data", $"Код {line[0]} уже есть в базе");
                    else
                        item.Code = line[0];
                    var filial = context.Filials.FirstOrDefault(f => f.FilialName == line[1]);
                    if (filial == null)
                        ModelState.AddModelError("data", $"Филиал {line[1]} не распознан");
                    else
                        item.Filial = filial.ID;
                    var reserv = context.ReservLevels.FirstOrDefault(r => r.ReservLevelName == line[2]);
                    if (reserv == null)
                        ModelState.AddModelError("data", $"Уровень резерва {line[2]} не распознан");
                    else
                        item.RezervLevel = reserv.ID;
                    if (!"12345".Contains(line[3]))
                        ModelState.AddModelError("data", $"Год {line[3]} не является числом");
                    else
                        item.Year = Convert.ToInt32(line[3]);

                    list.Add(item);
                }
                if (ModelState.IsValid)
                {
                    foreach (var item in list)
                    {
                        var filial = context.Filials.First(f => f.ID == item.Filial);
                        var reserv = context.ReservLevels.First(f => f.ID == item.RezervLevel);
                        Testable t = new Testable
                        {
                            YearTraining = item.Year,
                            Code = item.Code,
                            filial = filial,
                            reservLevel = reserv
                        };
                        context.Testables.Add(t);
                    }
                    context.SaveChanges();
                    return RedirectToAction("Finish");
                }
                else
                    return View();
            }
        }

        public IActionResult EditTestable(int ID)
        {
            using (var context = new AnketaContext())
            {
                MVC_TestableEdit model = context.Testables.Include(t => t.filial).Include(t => t.reservLevel)
                    .Where(f => f.ID == ID).Select(t => new MVC_TestableEdit
                    {
                        oldCode = t.Code,
                        Filial = t.filial.ID,
                        ID = t.ID,
                        RezervLevel = t.reservLevel.ID,
                        Year = t.YearTraining
                    }).ElementAt(0);
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult EditTestable(MVC_TestableEdit model)
        {
            using (var context = new AnketaContext())
            {
                if (model.oldCode != model.Code)
                    if (context.Testables.Any(t => t.Code == model.Code && t.Code != model.oldCode))
                    {
                        ModelState.AddModelError("Code", "Код не должен повторяться");
                    }
                if (ModelState.IsValid)
                {
                    var filial = context.Filials.First(f => f.ID == model.Filial);
                    var reservLevel = context.ReservLevels.First(f => f.ID == model.RezervLevel);
                    var tes = context.Testables.Include(t => t.filial).Include(t => t.reservLevel)
                        .First(t => t.ID == model.ID);
                    tes.Code = model.Code;
                    tes.reservLevel = reservLevel;
                    tes.filial = filial;
                    tes.YearTraining = model.Year;
                    context.SaveChanges();
                }
                else return View();
            }
            return RedirectToAction("Finish");
        }

        public IActionResult DetailsTestable(int ID)
        {
            return View();
        }

        public IActionResult Finish() => RedirectToAction("Index");
    }
}
