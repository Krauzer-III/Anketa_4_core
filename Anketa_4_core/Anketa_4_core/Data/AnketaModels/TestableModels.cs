using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Anketa_4_core.Data.AnketaModels
{
    public class Testables
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
        public Filials filial { get; set; }
        public ReservLevels reservLevel { get; set; }
    }

    public class Filials
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string FilialName { get; set; }
    }

    public class ReservLevels
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string ReservLevelName { get; set; }
    }

    public class Groups
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
        public Groups Group { get; set; }
        [Required]
        public Testables Testable { get; set; }
        [Required]
        [DefaultValue(true)]
        public bool isActive { get; set; }
    }
}
