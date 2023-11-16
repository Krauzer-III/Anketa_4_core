using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
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
                    if (answers.Count() != 4) throw new Exception("Прогрузились не все ответы при отправке на форму");
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
                    else throw new Exception("Такого ID нет в базе");
                    context.SaveChanges();

                    int year = context.Comp_Answers.Include(a => a.Question.Block).First(a => a.ID == answers[0].AnswerID).Question.Block.Year;
                    int questionCount = context.Comp_Questions.Include(b => b.Block).Count(q => q.Block.Year == year);
                    if (answers[0].QuestionNumber < questionCount) RedirectToAction("Question", answers[0].QuestionNumber + 1);
                    else
                    {
                        Task.Run(() =>
                        {
                            //сначала проверим есть ли результат в базе
                            var result = context.TestResults.Include(aft => aft.Access).FirstOrDefault(r => r.Access.ID == answers[0].AccesForTestable);
                            if (result != null)
                            {
                                var json = new JSON_ResultModel(result.Results);
                                json.Add_JSON_Competention(context.Comp_TestableAnswers.Include(aft => aft.Access).Where(a => a.Access.ID == answers[0].AccesForTestable).ToArray());
                                result.Results = json.GetJSON();
                                context.SaveChanges();
                            }
                            else
                            {
                                result = new TestResult
                                {
                                    Access=
                                };
                            }
                        });
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    //TODO журналировать и перенаправить на страницу ошибки
                    return View();
                }
            }
        }


        public IActionResult Final() => View();
    }
}
