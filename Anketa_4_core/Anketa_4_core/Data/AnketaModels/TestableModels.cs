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
        public DateTime DateCreate { get; set; }
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

    public class Group
    {
        [Key]   
        public int ID { get; set; }
        [Required]
        public string GroupName { get; set;}
    }

    public class AccessForTestables
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public Group Group { get; set; }
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
}
