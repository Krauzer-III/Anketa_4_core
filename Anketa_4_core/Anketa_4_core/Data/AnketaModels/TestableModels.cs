using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Anketa_4_core.Data.AnketaModels
{
    public class Testable
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public int YearTraining { get; set; }
        [Required]
        [DefaultValue(false)]
        public bool isArchived { get; set; }
        [Required]
        public Filial filial { get; set; }
        public ReservLevel reservLevel { get; set; }
    }

    public class Filial
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string FilialName { get; set; }
    }

    public class ReservLevel
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string ReservLevelName { get; set; }
    }

    public class TestPeriod
    {
        [Key]   
        public int ID { get; set; }
        [Required]
        public string GroupName { get; set;}
    }

    public class AccessForTestable
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public TestPeriod TestPeriod { get; set; }
        [Required]
        public Testable Testable { get; set; }
        [Required]
        [DefaultValue(true)]
        public bool isActive { get; set; }
    }

    public class News
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Header { get; set; }
        [Required]
        public string Body { get; set; }
        public DateTime DateNews { get; set; }
    }

    public class TestableUser {
        [Key]
        public int ID { get; set; }
        public string LK_Data { get; set; }
        public Testable testable { get; set; }
        public string UserName { get; set; }
    }

    public class  UserFilial
    {
        public string UserName { get; set; }
        public Filial Filial { get; set; }

    }



}
