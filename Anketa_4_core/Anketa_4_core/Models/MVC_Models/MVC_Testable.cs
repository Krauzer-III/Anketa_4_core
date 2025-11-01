using Anketa_4_core.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Anketa_4_core.Helpers;
using static Anketa_4_core.Helpers.ValidTestableCode;
using Anketa_4_core.Models.AnketaModels.MVC_Models;

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
        public string? oldCode { get; set; }
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


   
    public class MVC_TestableListItem
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? Filial { get; set; }
        public int YearTraining { get; set; }
        public bool IsArchived { get; set; }
    }


    public class MVC_TestableEditFull : MVC_TestableEdit
    {
        [Display(Name = "Привязка к аккаунту (UserName)")]
        public string? UserName { get; set; }


        [Display(Name = "Период доступа (TestPeriod)")]
        public int? SelectedTestPeriodId { get; set; } = new();


        public List<(int id, string name)>? Filials { get; set; } = new();
        public List<(int id, string name)>? ReservLevels { get; set; } = new();
        public List<(int id, string name)>? TestPeriods { get; set; } = new();
    }


    public class MVC_TestableDetailsFull : MVC_TestableDetails
    {
        [Display(Name = "UserName")]
        public string? UserName { get; set; }


        [Display(Name = "LK_Data")]
        public string? LK_Data { get; set; }


        public List<(int id, string name)> BoundTestPeriods { get; set; } = new();
        public List<string> Reports { get; set; } = new();
    }


    public class MVC_TestableDeletePreview
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public int YearTraining { get; set; }
        public bool IsArchived { get; set; }
        public string? Filial { get; set; }
        public string? ReservLevel { get; set; }
        public string? UserName { get; set; }
        public string? LK_Data { get; set; }
        public List<(int id, string name)> BoundTestPeriods { get; set; } = new();
        public int AccessLinksCount => BoundTestPeriods.Count;
    }


    public class MVC_TestableImportVM
    {
        [Display(Name = "CSV-файл")]
        public IFormFile? File { get; set; }


        [Display(Name = "Год по умолчанию для импорта")]
        public int DefaultYear { get; set; } = DateTime.UtcNow.Year;
    }

    public class MVC_TestableAddPageVM
    {
        public MVC_TestableEditFull Single { get; set; } = new MVC_TestableEditFull();
        public MVC_TestableImportVM Import { get; set; } = new MVC_TestableImportVM();
        // Доп. данные для выпадающего списка пользователей
        public List<string> FreeUsers { get; set; } = new List<string>();
        // Для модального окна справки
        public List<string> FilialNames { get; set; } = new List<string>();
        public List<(int id, string name)> ReservLevels { get; set; } = new List<(int id, string name)>();
        public List<(int id, string name)> TestPeriods { get; set; } = new List<(int id, string name)>();
    }

}
