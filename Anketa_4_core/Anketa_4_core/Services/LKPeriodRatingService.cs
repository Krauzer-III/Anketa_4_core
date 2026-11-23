using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Anketa_4_core.Services
{
    public class LKRatingRow
    {
        public int TestableID { get; set; }
        public string Code { get; set; } = "";
        public int Place { get; set; }
        public double TableCompletion { get; set; }
        public double TestCompletion { get; set; }
        public double CompetenceQuality { get; set; }
        public double TotalScore { get; set; }
    }

    public class LKRatingSnapshot
    {
        public string Period { get; set; } = "";
        public int PeriodID { get; set; }
        public int Participants { get; set; }
        public LKRatingRow? Current { get; set; }
        public List<LKRatingRow> Top { get; set; } = new();
        public string Formula { get; set; } =
            "Предварительный рейтинг: заполненность таблиц — 50%, " +
            "завершённость четырёх тестов — 25%, результаты компетенций — 25%. " +
            "Веса и критерии согласуем отдельно.";
    }

    /// <summary>
    /// Рассчитывает отдельную таблицу рейтинга внутри TestPeriod.
    /// Использует пакетную загрузку TestResult.Results, а не первичные ответы.
    ///
    /// TODO: согласовать окончательные веса, обязательность отдельных таблиц
    /// и нормирование результатов тестов по программам 1/2 года.
    /// Пока рейтинг вспомогательный, не следует использовать его как
    /// официальный кадровый показатель.
    /// </summary>
    public class LKPeriodRatingService
    {
        private readonly AnketaContext _db;
        public LKPeriodRatingService(AnketaContext db) => _db = db;

        public async Task<LKRatingSnapshot> GetForTestableAsync(int testableId)
        {
            var latest = await _db.AccessForTestables.AsNoTracking()
                .Where(x => x.Testable.ID == testableId)
                .OrderByDescending(x => x.ID)
                .Select(x => new
                {
                    AccessID = x.ID, PeriodID = x.TestPeriod.ID,
                    Period = x.TestPeriod.GroupName
                }).FirstOrDefaultAsync();

            if (latest == null)
                return new LKRatingSnapshot { Formula = "Период тестирования пока не назначен." };

            var accessRows = await _db.AccessForTestables.AsNoTracking()
                .Where(x => x.TestPeriod.ID == latest.PeriodID)
                .Select(x => new
                {
                    AccessID = x.ID,
                    TestableID = x.Testable.ID,
                    x.Testable.Code,
                    x.Testable.YearTraining
                }).ToArrayAsync();

            // If a testable has duplicate assignments within the same period,
            // count their newest assignment once.
            var participants = accessRows.GroupBy(x => x.TestableID)
                .Select(g => g.OrderByDescending(x => x.AccessID).First())
                .ToArray();
            var userIds = participants.Select(x => x.TestableID).ToArray();
            var accessIds = participants.Select(x => x.AccessID).ToArray();

            var lkRows = await _db.TestableUsers.AsNoTracking()
                .Where(x => userIds.Contains(x.testable.ID))
                .Select(x => new { TestableID = x.testable.ID, x.LK_Data })
                .ToArrayAsync();
            var dataByTestable = lkRows.GroupBy(x => x.TestableID)
                .ToDictionary(x => x.Key, x => x.First().LK_Data);

            var reportRows = await _db.TestResults.AsNoTracking()
                .Where(x => accessIds.Contains(x.Access.ID))
                .Select(x => new { AccessID = x.Access.ID, x.ID, x.Results })
                .ToArrayAsync();
            var reportByAccess = reportRows.GroupBy(x => x.AccessID)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.ID).First().Results);

            var ranking = new List<LKRatingRow>();
            foreach (var item in participants)
            {
                dataByTestable.TryGetValue(item.TestableID, out var lkJson);
                reportByAccess.TryGetValue(item.AccessID, out var resultJson);

                var table = GetTableCompletion(lkJson);
                var (test, competence) = GetTestScores(resultJson, item.YearTraining);
                ranking.Add(new LKRatingRow
                {
                    TestableID = item.TestableID,
                    Code = item.Code,
                    TableCompletion = Math.Round(table, 1),
                    TestCompletion = Math.Round(test, 1),
                    CompetenceQuality = Math.Round(competence, 1),
                    TotalScore = Math.Round(table * 0.5 + test * 0.25 + competence * 0.25, 2)
                });
            }

            ranking = ranking.OrderByDescending(x => x.TotalScore)
                .ThenBy(x => x.Code, StringComparer.Ordinal).ToList();

            int place = 0;
            double? previous = null;
            for (int i = 0; i < ranking.Count; i++)
            {
                if (previous == null || ranking[i].TotalScore != previous)
                    place = i + 1;
                ranking[i].Place = place;
                previous = ranking[i].TotalScore;
            }

            return new LKRatingSnapshot
            {
                PeriodID = latest.PeriodID,
                Period = latest.Period,
                Participants = ranking.Count,
                Current = ranking.FirstOrDefault(x => x.TestableID == testableId),
                Top = ranking.Take(20).ToList()
            };
        }

        private static (double completed, double competence) GetTestScores(
            string? json, int yearTraining)
        {
            if (string.IsNullOrWhiteSpace(json)) return (0, 0);
            JSON_ResultModel? result;
            try { result = JsonSerializer.Deserialize<JSON_ResultModel>(json); }
            catch (Exception ex) when (ex is JsonException or NotSupportedException)
            {
                return (0, 0);
            }
            if (result == null) return (0, 0);

            // Personal year is 1 or 2. If the legacy YearTraining contains a
            // calendar year, use year 1 provisionally until mapping is agreed.
            int year = yearTraining == 2 ? 2 : 1;

            bool self360 = result.Tests360?.Any(x => x.IsSelfMark) == true;
            bool motivation = (result.MotivationTest?.ID ?? 0) > 0;
            bool kettel = result.KettelTest?.IsCompleted == true ||
                (result.KettelTest?.Results?.Count ?? 0) > 0;
            bool competenceCompleted = result.CompetentionResult?
                .Any(x => x.Year == year) == true;
            double complete = (new[] { self360, motivation, kettel, competenceCompleted }
                .Count(x => x) / 4.0) * 100.0;

            var blocks = result.CompetentionResult?.Where(x => x.Year == year).ToArray()
                ?? Array.Empty<JSON_Motivation_Result.JSON_Competention_Result>();
            int max = blocks.Sum(x => x.MaxBall);
            double quality = max == 0 ? 0 : Math.Clamp(
                blocks.Sum(x => x.Ball) * 100.0 / max, 0, 100);
            return (complete, quality);
        }

        private static double GetTableCompletion(string? json)
        {
            JsonObject root;
            try { root = LKCabinetService.ParseRoot(json); }
            catch (InvalidOperationException) { return 0; }

            // Required entries from the IПП instruction: three professional tasks,
            // one training programme, at least two selected distance courses,
            // one professional book and two books for personal/business competencies.
            // Optional directions affect completion only after the testable adds
            // at least one record to that section.
            var sections = new List<double>
            {
                ScoreListRequired(root["ProfTasks"], 3, "Number", "ProfTaskName",
                    "CompetenceZone", "DateFrom", "DateTo", "AnalisEffect",
                    "WhatCouldBeDoneDifferently", "Difficult"),
                ScoreListRequired(root["TrainingProgramms"], 1, "LiveSityations",
                    "Difficult", "InfluenceAsWorker", "InfluenceAsFilial"),
                ScorePlannedListRequired(root["DistanceLearning"], 2, "Title",
                    "DateFrom", "DateTo", "Certificate"),
                ScoreBooksRequired(root["ReportBooks"])
            };

            AddOptionalList(sections, root["Internships"], "DateFrom", "DateTo");
            AddOptionalNestedList(sections, root["Internships"], "Tasks",
                "DateFrom", "DateTo", "Goal", "Activities", "Report", "SupervisorAssessment");
            AddOptionalList(sections, root["LK_ViceBoss"], "Position", "DateFrom",
                "DateTo", "Target", "GeneralResult");
            AddOptionalNestedList(sections, root["LK_ViceBoss"], "ViceBoss_Tasks",
                "TaskName", "ReportAboutTask", "MarkAboutTask");
            AddOptionalList(sections, root["AdvancedTrainingCourses"], "ProgramName",
                "Organization", "DateFrom", "DateTo", "DurationHours", "AcquiredSkills",
                "ApplicationInWork", "Result", "Certificate");
            AddOptionalList(sections, root["LanguageTrainings"], "Language",
                "Organization", "Format", "DateFrom", "DateTo", "CurrentLevel",
                "TargetLevel", "Result");
            AddOptionalList(sections, root["TeachingActivities"], "ActivityType",
                "Topic", "Audience", "Role", "DateFrom", "DateTo", "DurationHours",
                "PracticalUse", "Effect", "WhatBetter", "WhatHelped", "WhatHindered");
            AddOptionalList(sections, root["Mentorships"], "MenteePosition",
                "OrderNumber", "OrderDate", "Goal", "DateFrom", "DateTo",
                "Activities", "Result", "SupervisorFeedback");
            AddOptionalList(sections, root["ProfessionalRetrainings"], "ProgramName",
                "Organization", "DateFrom", "DateTo", "Qualification", "Result", "Certificate");
            AddOptionalList(sections, root["ProjectActivities"], "ProjectName", "Role",
                "DateFrom", "DateTo", "Goal", "Tasks", "PracticalUse", "Effect",
                "WhatBetter", "WhatHelped", "WhatHindered", "Result", "Difficulties");
            AddOptionalList(sections, root["ConferencesAndCompetitions"], "EventType",
                "EventName", "Organizer", "Date", "Role", "PracticalUse", "Effect",
                "WhatBetter", "WhatHelped", "WhatHindered", "Result");
            AddOptionalList(sections, root["PsychologicalConsultations"], "DateFrom",
                "DateTo", "DurationMinutes", "Format");

            var mapScore = ScoreMap(root["DevelopmentalActionMaproperty"]);
            if (mapScore.HasValue) sections.Add(mapScore.Value);

            var motivationScore = ScoreObject(root["PersonnelMotivation"],
                "WorkSignificanceAction1", "WorkSignificanceAction2",
                "WorkVarietyAction1", "WorkVarietyAction2",
                "WorkIdentityAction1", "WorkIdentityAction2",
                "WorkAutonomyAction1", "WorkAutonomyAction2",
                "FeedbackAction1", "FeedbackAction2");
            if (motivationScore.HasValue) sections.Add(motivationScore.Value);

            return sections.Count == 0 ? 0 : sections.Average() * 100.0;
        }

        private static void AddOptionalList(
            List<double> scores, JsonNode? node, params string[] fields)
        {
            if (node is JsonArray arr && arr.Count > 0)
                scores.Add(ScoreList(node, fields));
        }

        private static void AddOptionalNestedList(
            List<double> scores, JsonNode? parentNode, string childKey, params string[] fields)
        {
            if (parentNode is not JsonArray parents) return;
            var records = parents.OfType<JsonObject>()
                .SelectMany(parent => parent[childKey] is JsonArray children
                    ? children.OfType<JsonObject>() : Enumerable.Empty<JsonObject>())
                .ToArray();
            if (records.Length > 0)
                scores.Add(records.Average(x =>
                    fields.Count(field => Filled(x[field]) > 0) / (double)fields.Length));
        }

        private static double? ScoreMap(JsonNode? node)
        {
            if (node is not JsonObject map) return null;
            var sections = new List<double>();

            double dates = (Filled(map["DateFrom"]) + Filled(map["DateTo"])) / 2.0;
            if (dates > 0) sections.Add(dates);

            AddOptionalList(sections, map["ProfessionalDevelopment"], "CommentToT19",
                "RecomendationToT19", "DialogResultToT19");
            AddOptionalList(sections, map["ProfessionalDevelopment_T19"], "Header", "Description");
            AddOptionalList(sections, map["ProfessionalDevelopment_T20"], "Theme", "Format",
                "Date", "Interes", "PracticalUse", "Quality", "Comment",
                "Recomendation", "Effect", "WhatBetter");
            AddOptionalList(sections, map["ResultOrientation_T09"], "TypeOfActivity", "Processes",
                "OrderDocument", "AdjustmentOfOrderDocument",
                "MissindOrderDocument", "BossRecomendation");
            AddOptionalList(sections, map["ResultOrientation_T10"], "Type",
                "NonOrderedProcess_OldDocument",
                "DevelopmentPlanOf_Order_Old_Document_Task",
                "DevelopmentPlanOfOrder_Old_Document_Date", "MarkOfComplete");
            AddOptionalList(sections, map["LK_Map_ResultOrientation_T11"], "Task",
                "PlanResult_S_M_A_R_T", "FactResult", "DeviationOfFactResult",
                "DeviationReason", "MeasuresOfDeviationOfFactResult",
                "FeedBack", "Conclusion");
            AddOptionalList(sections, map["CompetenceDevelopment"], "CompetenceName",
                "CurrentLevel", "TargetLevel", "DevelopmentActions", "Deadline", "ExpectedResult");

            return sections.Count == 0 ? null : sections.Average();
        }

        private static double? ScoreObject(JsonNode? node, params string[] fields)
        {
            if (node is not JsonObject obj ||
                !fields.Any(field => Filled(obj[field]) == 1))
                return null;
            return fields.Count(field => Filled(obj[field]) > 0) / (double)fields.Length;
        }

        private static double ScoreBooksRequired(JsonNode? node)
        {
            var books = node is JsonArray array
                ? array.OfType<JsonObject>().ToArray()
                : Array.Empty<JsonObject>();

            const string professional = "Профессиональные компетенции";
            const string personal = "Личностно-деловые и управленческие компетенции";
            string[] fields =
                { "BookName", "Author", "DateFrom", "DateTo", "Gold", "PractiсeSkills" };

            double professionalScore = ScoreBookCategory(books, professional, 1, fields);
            double personalScore = ScoreBookCategory(books, personal, 2, fields);
            return (professionalScore + personalScore * 2.0) / 3.0;
        }

        private static double ScoreBookCategory(
            JsonObject[] books, string category, int requiredCount, string[] fields)
        {
            var selected = books.Where(book =>
                string.Equals(book["BookCategory"]?.ToString(), category,
                    StringComparison.OrdinalIgnoreCase)).ToArray();
            int denominator = Math.Max(selected.Length, requiredCount);
            if (denominator == 0) return 0;
            double completed = selected.Sum(book =>
                fields.Count(field => Filled(book[field]) > 0) / (double)fields.Length);
            return completed / denominator;
        }

        private static double ScorePlannedListRequired(
            JsonNode? node, int requiredRows, params string[] fields)
        {
            var records = node is JsonArray array
                ? array.OfType<JsonObject>()
                    .Where(item => bool.TryParse(item["IsPlanned"]?.ToString(), out var planned)
                        && planned).ToArray()
                : Array.Empty<JsonObject>();
            int denominator = Math.Max(records.Length, requiredRows);
            if (denominator == 0 || fields.Length == 0) return 0;

            double completed = records.Sum(record =>
                fields.Count(field => Filled(record[field]) > 0) / (double)fields.Length);
            return completed / denominator;
        }

        private static double ScoreListRequired(
            JsonNode? node, int requiredRows, params string[] fields)
        {
            var records = node is JsonArray arr
                ? arr.OfType<JsonObject>().ToArray()
                : Array.Empty<JsonObject>();
            int denominator = Math.Max(records.Length, requiredRows);
            if (denominator == 0 || fields.Length == 0) return 0;

            double completed = records.Sum(record =>
                fields.Count(field => Filled(record[field]) > 0) / (double)fields.Length);
            return completed / denominator;
        }

        private static double ScoreList(JsonNode? node, params string[] fields)
        {
            if (node is not JsonArray arr || arr.Count == 0 || fields.Length == 0) return 0;
            var records = arr.OfType<JsonObject>().ToArray();
            if (records.Length == 0) return 0;
            return records.Average(x => fields.Count(field => Filled(x[field]) > 0) / (double)fields.Length);
        }

        private static int Filled(JsonNode? value)
        {
            if (value == null) return 0;
            string text = value.ToString().Trim();
            return text.Length == 0 || text == "0" ||
                   text.StartsWith("0001-01-01", StringComparison.Ordinal)
                   ? 0 : 1;
        }
    }
}
