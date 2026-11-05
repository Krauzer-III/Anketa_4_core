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
            catch (JsonException) { return (0, 0); }
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

            double[] sections =
            {
                ScoreList(root["ProfTasks"], "Number", "ProfTaskName", "DateFrom",
                    "DateTo", "AnalisEffect", "Difficult"),
                ScoreList(root["Internships"], "DateFrom", "DateTo"),
                ScoreList(root["LK_ViceBoss"], "DateFrom", "DateTo", "Target"),
                ScoreList(root["ReportBooks"], "BookName", "Author", "Gold", "PractiсeSkills"),
                ScoreList(root["TrainingProgramms"], "LiveSityations", "Difficult",
                    "InfluenceAsWorker", "InfluenceAsFilial"),
                ScoreMap(root["DevelopmentalActionMaproperty"])
            };
            return sections.Average() * 100.0;
        }

        private static double ScoreMap(JsonNode? node)
        {
            if (node is not JsonObject map) return 0;
            double dates = (Filled(map["DateFrom"]) + Filled(map["DateTo"])) / 2.0;
            double[] sections =
            {
                dates,
                ScoreList(map["ProfessionalDevelopment"], "CommentToT19",
                    "RecomendationToT19", "DialogResultToT19"),
                ScoreList(map["ProfessionalDevelopment_T19"], "Header", "Description"),
                ScoreList(map["ProfessionalDevelopment_T20"], "Theme", "Format",
                    "Date", "Interes", "PracticalUse", "Quality", "Comment",
                    "Recomendation", "Effect", "WhatBetter"),
                ScoreList(map["ResultOrientation_T09"], "TypeOfActivity", "Processes",
                    "OrderDocument", "AdjustmentOfOrderDocument",
                    "MissindOrderDocument", "BossRecomendation"),
                ScoreList(map["ResultOrientation_T10"], "Type",
                    "NonOrderedProcess_OldDocument",
                    "DevelopmentPlanOf_Order_Old_Document_Task",
                    "DevelopmentPlanOfOrder_Old_Document_Date", "MarkOfComplete"),
                ScoreList(map["LK_Map_ResultOrientation_T11"], "Task",
                    "PlanResult_S_M_A_R_T", "FactResult", "DeviationOfFactResult",
                    "DeviationReason", "MeasuresOfDeviationOfFactResult",
                    "FeedBack", "Conclusion")
            };
            return sections.Average();
        }

        private static double ScoreList(JsonNode? node, params string[] fields)
        {
            if (node is not JsonArray arr || arr.Count == 0) return 0;
            var records = arr.OfType<JsonObject>().ToArray();
            if (records.Length == 0) return 0;
            return records.Average(x => fields.Count(field => Filled(x[field])) / (double)fields.Length);
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
