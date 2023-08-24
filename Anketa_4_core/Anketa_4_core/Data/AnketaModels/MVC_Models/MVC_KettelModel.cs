namespace Anketa_4_core.Data.AnketaModels.MVC_Models
{
    public class MVC_KettelModel_Main
    {
        public int Years { get; set; }
        public string Gender { get; set; }
        public int AccessID { get; set; }
    }

    public class MVC_KettelModel_Question
    {
        public int Number { get; set; }
        public string Text { get; set; }
        public MVC_KettelModel_Answer[] Answers { get; set; }
    }

    public class MVC_KettelModel_Answer
    {
        public int ID { get; set; }
        public string AnswerNumber { get; set; }
        public string AnswerText { get; set; }
    }




}
