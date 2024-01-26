using Anketa_4_core.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Anketa_4_core.Helpers;
using static Anketa_4_core.Helpers.ValidTestableCode;

namespace Anketa_4_core.Models.AnketaModels.MVC_Models
{
    public class MVC_TestableAdd
    {
        [ValidTestableCode(operationType = OperationType.Add, ErrorMessage = "Такой код оцениваемого уже существует")]
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
        [ValidTestableCode(operationType = OperationType.Edit, ErrorMessage = "Такой код оцениваемого уже существует", oldCode = oldCode)]   //TODO решить проблему передачи параметра
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

    public class MVC_TestableDetails
    {
        [Required(ErrorMessage ="Поле не должно быть пустым")]
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
        [Display(Name = "В архиве")]
        public bool isArchived { get; set; }
    }
}
