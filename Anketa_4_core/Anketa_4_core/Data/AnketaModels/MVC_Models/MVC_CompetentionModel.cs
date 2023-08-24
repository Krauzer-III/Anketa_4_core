namespace Anketa_4_core.Data.AnketaModels.MVC_Models
{
    public class MVC_CompetentionModel_Question
    {
        public int Number { get; set; }
        public string Text { get; set; }
        public MVC_KettelModel_Answer[] Answers { get; set; }
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
    }
}
