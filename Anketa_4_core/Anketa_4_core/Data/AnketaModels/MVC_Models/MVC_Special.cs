namespace Anketa_4_core.Data.AnketaModels.MVC_Models
{
    public class MVC_TestableAdd
    {
        public string Code { get; set; }
        public int Filial { get; set; }
        public int Year { get; set; }
        public int RezervLevel { get; set; }
    }

    public class MVC_TestableEdit
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int Filial { get; set; }
        public int Year { get; set; }
        public int RezervLevel { get; set; }
    }

    public class MVC_TestableDetails
    {
        public string Code { get; set; }
        public string Filial { get; set; }
        public int Year { get; set; }
        public string RezervLevel { get; set; }
        //TODO MVC представления тестов и периодов
    }
}
