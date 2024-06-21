using Anketa_4_core.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Anketa_4_core.Helpers;
using static Anketa_4_core.Helpers.ValidTestableCode;

namespace Anketa_4_core.Models.AnketaModels.MVC_Models
{
    public class MVC_TestableAdd
    {
        [Required(ErrorMessage = "Поле \"Код оцениваемого\" не должно быть пустым")]
        [Display(Name = "Код оцениваемого")]
        public string Code { get; set; }
        [Display(Name = "Филиал")]
        public int Filial { get; set; }
        [Display(Name = "Год резерва")]
        public int Year { get; set; }
        [Display(Name = "Уровень резерва")]
        public int RezervLevel { get; set; }

    }

    public class MVC_TestableEdit
    {
        [HiddenInput]
        public int ID { get; set; }
        [HiddenInput]
        public string oldCode { get; set; }
        [Display(Name = "Код Оцениваемого")]
        [Required(ErrorMessage = "Поле \"Код оцениваемого\" не должно быть пустым")]
        public string Code { get; set; }
        [Display(Name = "Филиал")]
        public int Filial { get; set; }
        [Display(Name = "Год резерва")]
        public int Year { get; set; }
        [Display(Name = "Уровень резерва")]
        public int RezervLevel { get; set; }
    }

    public class MVCTestableAddMany
    {
        [Display(Name = "Список оцениваемых")]
        public string data { get; set; }
        [Display(Name = "Разделить данных")]
        public string separator { get; set; }
    }

    public class MVC_TestableDetails
    {
        [HiddenInput]
        public int ID { get; set; }
        [Display(Name = "Код Оцениваемого")]
        public string Code { get; set; }
        [Display(Name = "Филиал")]
        public int Filial { get; set; }
        [Display(Name = "Год резерва")]
        public int Year { get; set; }
        [Display(Name = "Уровень резерва")]
        public int RezervLevel { get; set; }
        //TODO MVC представления тестов и периодов


    }

    public class MVC_TestableView
    {
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name = "Код Оцениваемого")]
        public string Code { get; set; }
        [Display(Name = "Филиал")]
        public string FilialName { get; set; }
        [Display(Name = "Уровень резерва")]
        public string ReservLevelName { get; set; }
        [Display(Name = "Год резерва")]
        public int YearTraining { get; set; }
    }


    public class MVC_PeriodList
    {
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name = "Название периода")]
        public string PeriodName { get; set; }
        [Display(Name = "Дата добавления ")]
        public int CountTestables { get; set; }

    }

    public class MVC_PeriodAdd
    {
        [Display(Name = "Название периода")]
        public string PeriodName { get; set; }
        [Display(Name = "Тестируемые")]
        public string[] TestableNames { get; set; }
    }

    public class MVC_PeriodEdit
    {
        [HiddenInput]
        public int Id { get; set; }
        [HiddenInput]
        public string oldPeriodName { get; set; }
        [Display(Name = "Название периода")]
        public string PeriodName { get; set; }
        [Display(Name = "Тестируемые")]
        public string[] TestableNames { get; set; }
        [HiddenInput]
        public string[] oldTestableNames { get; set; }
    }


}
