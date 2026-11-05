using Anketa_4_core.Data.AnketaModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Anketa_4_core.Models.AnketaModels.MVC_Models
{
    /// <summary>
    /// Фильтры старого модуля результатов. Статус: all, done, progress, not.
    /// Фильтрация происходит по готовому JSON, а не по сырым ответам.
    /// </summary>
    public class MVC_ResultFilter
    {
        public string? Code { get; set; }
        public int? PeriodID { get; set; }
        public int? FilialID { get; set; }
        public int? ReservLevelID { get; set; }
        public string Self360 { get; set; } = "all"; // all/done/not
        public int? RespondentsLessThan { get; set; }
        public string Motivation { get; set; } = "all";
        public string Kettel { get; set; } = "all";
        public string Competence1 { get; set; } = "all";
        public string Competence2 { get; set; } = "all";
        public int Page { get; set; } = 1;
    }

    public class MVC_ResultIndex
    {
        public MVC_ResultFilter Filter { get; set; } = new();
        public List<SelectListItem> Periods { get; set; } = new();
        public List<SelectListItem> Filials { get; set; } = new();
        public List<SelectListItem> ReservLevels { get; set; } = new();
    }

    public class MVC_ResultRow
    {
        public int AccessID { get; set; }
        public int TestableID { get; set; }
        public string Code { get; set; } = "";
        public string Period { get; set; } = "";
        public string Filial { get; set; } = "";
        public string ReservLevel { get; set; } = "";
        public bool IsActive { get; set; }
        public bool HasJson { get; set; }
        public bool InvalidJson { get; set; }
        public bool HasSelf360 { get; set; }
        public int Respondents360 { get; set; }
        public string MotivationStatus { get; set; } = "not";
        public bool? MotivationAgree { get; set; }
        public string KettelStatus { get; set; } = "not";
        public int KettelAnswered { get; set; }
        public string Competence1Status { get; set; } = "not";
        public string Competence2Status { get; set; } = "not";
        public int Competence1Answered { get; set; }
        public int Competence2Answered { get; set; }
    }

    public class MVC_ResultSearchPage
    {
        public List<MVC_ResultRow> Items { get; set; } = new();
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; } = 100;
        public int Pages => (int)Math.Ceiling((double)Total / PageSize);
        public string? Error { get; set; }
    }

    public class MVC_ResultDetail
    {
        public int AccessID { get; set; }
        public string TestableCode { get; set; } = "";
        public string PeriodName { get; set; } = "";
        public int Year { get; set; }
        public JSON_ResultModel Data { get; set; } = new();
    }
}
