using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Anketa_4_core.Models.AnketaModels.MVC_Models
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
