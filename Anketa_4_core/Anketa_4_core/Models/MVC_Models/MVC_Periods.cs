using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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

        // Привязка необязательна: период можно создать без тестируемых.
        [Display(Name = "Тестируемые для привязки (необязательно)")]
        public List<int>? SelectedTestableIds { get; set; }

        // Данные для представления, а не обязательная часть POST-запроса.
        [ValidateNever]
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

        // После создания периода добавление тестируемых остаётся необязательным.
        [Display(Name = "Добавить тестируемых (необязательно)")]
        public List<int>? AddTestableIds { get; set; }

        [ValidateNever]
        public List<(int id, string name, string? filial)> AllTestables { get; set; } = new();
    }
}
