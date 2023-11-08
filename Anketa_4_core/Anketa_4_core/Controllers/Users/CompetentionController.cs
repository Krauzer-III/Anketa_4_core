using Anketa_4_core.Data;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Controllers.Users
{
    public class CompetentionController : Controller
    {
        public IActionResult Index()
        {

            //TODO проверка на роли и потом в зависимости от роли применить нужный метод по поиску доступа к тестам
            return View();
        }

        public IActionResult Question(int number)
        {
            using (var context = new AnketaContext())
            {
                var model = new MVC_CompetentionModel_Question();
                var q_model = context.Comp_Questions.First(e => e.QuestionNumber == number);
                model.Number = number;
                model.Text = q_model.QuestionText;
                model.Answers = context.Comp_Answers.Include(q => q.Question)
                    .Where(e => e.Question.ID == q_model.ID).Select(e => new MVC_CompetentionModel_Answer
                    {
                        ID = e.ID,
                        AnswerNumber = e.Number,
                        AnswerText = e.Text
                    }).ToArray();
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Question(MVC_Competention_AnswerTestable[] answers)
        {
            using (var context = new AnketaContext())
            {
                try
                {
                    var access = context.AccessForTestables.FirstOrDefault(e => e.ID == answers[0].AccesForTestable);
                    if (access != null)
                    {
                        foreach (var answer in answers)
                        {
                            var _answer = context.Comp_Answers.FirstOrDefault(a => a.ID == answer.AnswerID);
                            if (_answer != null)
                                context.Comp_TestableAnswers.Add(new Data.AnketaModels.Comp_TestableAnswer
                                {
                                    Access = access,
                                    Answer = _answer,
                                    Mark = answer.Ball,
                                    DateTimeAnswer = DateTime.Now
                                });
                        }
                    }

                    //TODO Сделать проверку на последний ворпос и перенаправить либо на страницу результатов 
                    return View();
                }
                catch (Exception ex)
                {
                    //TODO журналировать и перенаправить на страницу ошибки
                    return View();
                }
            }
        }
    }
}
