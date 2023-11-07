using Anketa_4_core.Data;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Controllers.Users
{
    public class CompetentionController : Controller
    {
        public IActionResult Index()
        {
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
                        ID=e.ID,
                        AnswerNumber=e.Number,
                        AnswerText=e.Text
                    }).ToArray();
                //TODO проверка на роли и потом в зависимости от роли применить нужный метод по поиску доступа к тестам
                return View(model);
            }
        }
    }
}
