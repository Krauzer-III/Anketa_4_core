using Microsoft.AspNetCore.Mvc;

namespace Anketa_4_core.Models.AnketaModels.MVC_Models
{
    public class MVC_CompetentionModel_Question
    {
        public int Number { get; set; }
        public string Text { get; set; }
        [HiddenInput]
        public int AccesForTestable { get; set; }
        public MVC_CompetentionModel_Answer[] Answers { get; set; }
    }

    public class MVC_CompetentionModel_Answer
    {
        public int ID { get; set; }
        public string AnswerNumber { get; set; }
        public string AnswerText { get; set; }
    }

    public class MVC_Competention_AnswerTestable
    {
        public int AnswerID { get; set; }
        public int Ball { get; set; }
        [HiddenInput]
        public int AccesForTestable { get; set; }
    }
}
