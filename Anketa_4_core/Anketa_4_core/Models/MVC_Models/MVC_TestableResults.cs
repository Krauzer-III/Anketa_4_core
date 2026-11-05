using System;
using System.Collections.Generic;

namespace Anketa_4_core.Models.AnketaModels.MVC_Models
{
    // Результаты одного назначения периода. Данные других периодов не смешиваются.
    public class MVC_TestablePeriodResults
    {
        public int AccessID { get; set; }
        public string PeriodName { get; set; } = "";
        public bool IsActive { get; set; }
        public string? JsonError { get; set; }
        public List<MVC_TestableTestResult> Tests { get; set; } = new();
    }

    public class MVC_TestableTestResult
    {
        // Test360, Motivation, Kettel, Competention
        public string Kind { get; set; } = "";
        public int? Year { get; set; }
        public string Name { get; set; } = "";
        public string Status { get; set; } = "Не начат";
        public bool HasData { get; set; }
        public DateTime? FirstAnswerAt { get; set; }
        public DateTime? LastAnswerAt { get; set; }
        public int AnsweredCount { get; set; }
        public int TotalCount { get; set; }
        public string? Note { get; set; }
        public List<MVC_TestableResultValue> Scores { get; set; } = new();
        public List<MVC_TestableAnswerDetail> Answers { get; set; } = new();
        public List<MVC_Testable360Submission> Submissions { get; set; } = new();
    }

    public class MVC_TestableResultValue
    {
        public string Label { get; set; } = "";
        public string Value { get; set; } = "";
        public string? Group { get; set; }
    }

    public class MVC_TestableAnswerDetail
    {
        public string Question { get; set; } = "";
        public string Answer { get; set; } = "";
    }

    public class MVC_Testable360Submission
    {
        public int ID { get; set; }
        public string RoleName { get; set; } = "";
        public bool IsSelfMark { get; set; }
        public DateTime DateTimeTest { get; set; }
        public List<MVC_TestableResultValue> Answers { get; set; } = new();
        public string? ParsingError { get; set; }
    }
}
