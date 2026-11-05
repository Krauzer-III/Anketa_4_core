using System.ComponentModel.DataAnnotations;

namespace Anketa_4_core.Models.AnketaModels.MVC_Models
{
    public class MVC_KettelModel_Login
    {
        [Required(ErrorMessage = "Введите код тестируемого.")]
        [Display(Name = "Код тестируемого")]
        public string Code { get; set; } = "";
    }

    public class MVC_KettelModel_Main
    {
        [Range(1, 120, ErrorMessage = "Укажите возраст от 1 до 120 лет.")]
        [Display(Name = "Возраст")]
        public int Years { get; set; }

        [Required(ErrorMessage = "Укажите пол.")]
        [Display(Name = "Пол")]
        public string Gender { get; set; } = "";

        public int AccessID { get; set; }
    }

    public class MVC_KettelModel_Question
    {
        public int AccessID { get; set; }
        public int Number { get; set; }
        public int Position { get; set; }
        public int TotalQuestions { get; set; }
        public string Text { get; set; } = "";
        public MVC_KettelModel_Answer[] Answers { get; set; } = Array.Empty<MVC_KettelModel_Answer>();
    }

    public class MVC_KettelModel_Answer
    {
        public int ID { get; set; }
        public string AnswerNumber { get; set; } = "";
        public string AnswerText { get; set; } = "";
    }

    public class MVC_KettelModel_Submit
    {
        public int AccessID { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Выберите ответ.")]
        public int SelectedAnswerID { get; set; }
    }
}
