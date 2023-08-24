namespace Anketa_4_core.Data.AnketaModels.MVC_Models
{
    public class MVC_NewsAdd
    {
        public string Header { get; set; }
        public string Body { get; set; }
    }

    public class MVC_NewsEdit
    {
        public int ID { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
    }
}
