// =============================================
// File: Controllers/KettelController.cs
// =============================================
using System.Linq;
using System.Threading.Tasks;
using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Controllers
{
    public class KettelController : Controller
    {
        private readonly AnketaContext _db;

        public KettelController(AnketaContext db)
        {
            _db = db;
        }

        // Стартовый экран: возраст, пол, AccessID
        [HttpGet]
        public IActionResult Start(int accessId)
        {
            var vm = new MVC_KettelModel_Main
            {
                AccessID = accessId
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Start(MVC_KettelModel_Main model)
        {
            if (model.Years <= 0)
            {
                ModelState.AddModelError(nameof(model.Years), "Укажите возраст");
            }
            if (string.IsNullOrWhiteSpace(model.Gender))
            {
                ModelState.AddModelError(nameof(model.Gender), "Укажите пол");
            }

            if (!ModelState.IsValid)
                return View(model);

            // начинаем с первого вопроса
            return RedirectToAction(nameof(Question), new
            {
                index = 1,
                accessId = model.AccessID,
                years = model.Years,
                gender = model.Gender
            });
        }

        // Один вопрос
        [HttpGet]
        public async Task<IActionResult> Question(
            int index,
            int accessId,
            int years,
            string gender)
        {
            if (index < 1) index = 1;

            var total = await _db.KT_Questions.CountAsync();
            if (total == 0)
                return View("Empty"); // если вопросов нет – отдельная страница, сделаешь при необходимости

            if (index > total)
                return RedirectToAction(nameof(Finish), new { accessId, years, gender });

            var questionEntity = await _db.KT_Questions
                .OrderBy(q => q.ID)            // TODO: при необходимости поменять на Number
                .Skip(index - 1)
                .FirstOrDefaultAsync();

            if (questionEntity == null)
                return RedirectToAction(nameof(Finish), new { accessId, years, gender });

            // ответы для этого вопроса
            var answers = await _db.KT_Answers
                .Where(a => a.Question.ID == questionEntity.ID) 
                .OrderBy(a => a.AnswerNumber)                   
                .Select(a => new MVC_KettelModel_Answer
                {
                    ID = a.ID,
                    AnswerNumber = a.AnswerNumber,
                    AnswerText = a.AnswerText
                })
                .ToArrayAsync();

            var vmQuestion = new MVC_KettelModel_Question
            {
                Number = index,
                Text = questionEntity.Text, 
                Answers = answers
            };

            ViewBag.TotalQuestions = total;
            ViewBag.AccessID = accessId;
            ViewBag.Years = years;
            ViewBag.Gender = gender;

            return View(vmQuestion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Question(
            int number,           
            int selectedAnswerId, 
            int accessId,
            int years,
            string gender)
        {
            // TODO: сохранить ответ в KT_TestableAnswers/ещё куда нужно
            // пример:
            // var answer = new KT_TestableAnswers { ... };
            // _db.KT_TestableAnswers.Add(answer);
            // await _db.SaveChangesAsync();

            var total = await _db.KT_Questions.CountAsync();
            var nextIndex = number + 1;

            if (nextIndex > total)
            {
                return RedirectToAction(nameof(Finish), new { accessId, years, gender });
            }

            return RedirectToAction(nameof(Question), new
            {
                index = nextIndex,
                accessId,
                years,
                gender
            });
        }

        // Конец теста – пока просто заглушка
        [HttpGet]
        public IActionResult Finish(int accessId, int years, string gender)
        {
            // TODO: анализ результатов по AccessID / сохранённым ответам
            ViewBag.AccessID = accessId;
            ViewBag.Years = years;
            ViewBag.Gender = gender;
            return View();
        }
    }
}
