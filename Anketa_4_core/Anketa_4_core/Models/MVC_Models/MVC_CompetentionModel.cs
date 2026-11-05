using System.ComponentModel.DataAnnotations;

namespace Anketa_4_core.Models.AnketaModels.MVC_Models
{
    public class MVC_CompetentionLoginModel
    {
        [Required(ErrorMessage = "Введите код тестируемого.")]
        [Display(Name = "Код тестируемого")]
        public string Code { get; set; } = "";
        [Range(1, 2)]
        public int Year { get; set; } = 1;
    }

    public class MVC_CompetentionModel_Question
    {
        public int AccessID { get; set; }
        public int Year { get; set; }
        public int QuestionID { get; set; }
        public int Number { get; set; }
        public int Position { get; set; }
        public int TotalQuestions { get; set; }
        public string Text { get; set; } = "";
        public MVC_CompetentionModel_Answer[] Answers { get; set; } =
            Array.Empty<MVC_CompetentionModel_Answer>();
    }

    public class MVC_CompetentionModel_Answer
    {
        public int ID { get; set; }
        public string AnswerNumber { get; set; } = "";
        public string AnswerText { get; set; } = "";
        public int? Priority { get; set; }
    }

    public class MVC_CompetentionSubmitModel
    {
        public int AccessID { get; set; }

        [Range(1, 2)]
        public int Year { get; set; }
        public int QuestionID { get; set; }
        public MVC_CompetentionModel_Answer[] Answers { get; set; } =
            Array.Empty<MVC_CompetentionModel_Answer>();
    }
}
