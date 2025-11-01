using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Anketa_4_core.Models.AnketaModels.MVC_Models
{
    public class MVC_TestPeriodListItem
    {
        public int Id { get; set; }
        [Display(Name = "Название периода")]
        public string PeriodName { get; set; } = string.Empty;

        [Display(Name = "Кол-во привязок")]
        public int CountTestables { get; set; }
    }

    public class MVC_TestPeriodDetails
    {
        public int Id { get; set; }
        [Display(Name = "Название периода")]
        public string PeriodName { get; set; } = string.Empty;

        public List<MVC_TestPeriodBoundTestable> BoundTestables { get; set; } = new();
    }

    public class MVC_TestPeriodBoundTestable
    {
        public int TestableId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? Filial { get; set; }
        [Display(Name = "Активен")]
        public bool IsActive { get; set; }
    }

    public class MVC_TestPeriodCreate
    {
        [Required(ErrorMessage = "Название периода обязательно")]
        [Display(Name = "Название периода")]
        public string PeriodName { get; set; } = string.Empty;

        [Display(Name = "Тестируемые для привязки")]
        public List<int> SelectedTestableIds { get; set; } = new();

        // источники для выпадающих списков
        public List<(int id, string name, string? filial)> AllTestables { get; set; } = new();
    }

    public class MVC_TestPeriodEdit
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название периода обязательно")]
        [Display(Name = "Название периода")]
        public string PeriodName { get; set; } = string.Empty;

        // текущие привязки (с возможностью менять активность/удалять)
        public List<MVC_TestPeriodBoundTestable> BoundTestables { get; set; } = new();

        // список для добавления новых
        [Display(Name = "Добавить тестируемых")]
        public List<int> AddTestableIds { get; set; } = new();

        public List<(int id, string name, string? filial)> AllTestables { get; set; } = new();
    }
}
