using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
using System.Text.Json;

namespace Anketa_4_core.Services
{
    /// <summary>
    /// Просмотр и согласованное удаление результатов с карточки тестируемого.
    /// Все действия ограничены AccessForTestable конкретного тестируемого.
    /// </summary>
    public class TestableResultsService
    {
        private readonly AnketaContext _db;
        private static readonly CultureInfo Ru = CultureInfo.GetCultureInfo("ru-RU");

        public TestableResultsService(AnketaContext db) => _db = db;

        public async Task<List<MVC_TestablePeriodResults>> LoadAsync(int testableId)
        {
            var accesses = await _db.AccessForTestables.AsNoTracking()
                .Include(x => x.TestPeriod)
                .Where(x => x.Testable.ID == testableId)
                .OrderByDescending(x => x.ID).ToArrayAsync();

            var accessIDs = accesses.Select(x => x.ID).ToArray();
            if (accessIDs.Length == 0) return new List<MVC_TestablePeriodResults>();

            // Читаем результаты пакетно, а не по одному запросу на тест и период.
            var reportRows = await _db.TestResults.AsNoTracking()
                .Where(x => accessIDs.Contains(x.Access.ID))
                .Select(x => new { x.ID, AccessID = x.Access.ID, Json = x.Results })
                .ToArrayAsync();

            var surveyRows = await _db.Test360.AsNoTracking()
                .Include(x => x.AccessTest).Include(x => x.Role)
                .Where(x => accessIDs.Contains(x.AccessTest.ID))
                .OrderBy(x => x.DatetimeTest).ThenBy(x => x.ID).ToArrayAsync();

            var motivationRows = await _db.MotivationTests.AsNoTracking()
                .Include(x => x.AccessTest)
                .Where(x => accessIDs.Contains(x.AccessTest.ID))
                .OrderBy(x => x.DatetimeTest).ToArrayAsync();

            var kettelMains = await _db.KT_Mains.AsNoTracking()
                .Include(x => x.Access)
                .Where(x => accessIDs.Contains(x.Access.ID)).ToArrayAsync();
            var kettelAnswers = await _db.KT_TestableAnswers.AsNoTracking()
                .Include(x => x.MainInfo).ThenInclude(x => x.Access)
                .Include(x => x.Answer).ThenInclude(x => x.Question)
                .Where(x => accessIDs.Contains(x.MainInfo.Access.ID))
                .OrderBy(x => x.Answer.Question.Number).ToArrayAsync();

            var compAnswers = await _db.Comp_TestableAnswers.AsNoTracking()
                .Include(x => x.Access)
                .Include(x => x.Answer).ThenInclude(x => x.Question)
                .ThenInclude(x => x.Block)
                .Where(x => accessIDs.Contains(x.Access.ID)).ToArrayAsync();

            int totalKettel = await _db.KT_Questions.CountAsync();
            var compQuestionCounts = (await _db.Comp_Questions.AsNoTracking()
                .Select(x => new { x.ID, Year = x.Block.Year })
                .ToArrayAsync())
                .GroupBy(x => x.Year)
                .ToDictionary(x => x.Key, x => x.Count());

            var result = new List<MVC_TestablePeriodResults>();
            foreach (var access in accesses)
            {
                var period = new MVC_TestablePeriodResults
                {
                    AccessID = access.ID,
                    PeriodName = access.TestPeriod?.GroupName ?? $"Период {access.ID}",
                    IsActive = access.isActive
                };

                JSON_ResultModel? report = null;
                var reports = reportRows.Where(x => x.AccessID == access.ID)
                    .OrderByDescending(x => x.ID).ToArray();
                if (reports.Length > 0)
                {
                    try
                    {
                        report = JsonSerializer.Deserialize<JSON_ResultModel>(reports[0].Json);
                        if (report == null) period.JsonError = "Отчётный JSON пуст.";
                        if (reports.Length > 1)
                            period.JsonError = "Для периода найдено несколько записей TestResult; показана последняя.";
                    }
                    catch (Exception ex) when (ex is JsonException or NotSupportedException)
                    {
                        period.JsonError = "Отчётный JSON повреждён: " + ex.Message;
                    }
                }

                var surveys = surveyRows.Where(x => x.AccessTest.ID == access.ID).ToArray();
                var motivation = motivationRows.Where(x => x.AccessTest.ID == access.ID).ToArray();
                var mains = kettelMains.Where(x => x.Access.ID == access.ID).ToArray();
                var ktAnswers = kettelAnswers.Where(x => x.MainInfo.Access.ID == access.ID).ToArray();
                var competence = compAnswers.Where(x => x.Access.ID == access.ID).ToArray();

                period.Tests.Add(Show360(surveys, report));
                period.Tests.Add(ShowMotivation(motivation, report));
                period.Tests.Add(ShowKettel(mains, ktAnswers, totalKettel, report));
                period.Tests.Add(ShowCompetence(1, competence, compQuestionCounts.GetValueOrDefault(1), report));
                period.Tests.Add(ShowCompetence(2, competence, compQuestionCounts.GetValueOrDefault(2), report));
                result.Add(period);
            }
            return result;
        }

        private static MVC_TestableTestResult Show360(Test360[] surveys, JSON_ResultModel? report)
        {
            var row = new MVC_TestableTestResult
            {
                Kind = "Test360", Name = "Оценка 360°",
                HasData = surveys.Length > 0, AnsweredCount = surveys.Length,
                Status = surveys.Length == 0 ? "Не пройдена" : $"Анкет: {surveys.Length}"
            };
            if (surveys.Length > 0)
            {
                row.FirstAnswerAt = surveys.Min(x => x.DatetimeTest);
                row.LastAnswerAt = surveys.Max(x => x.DatetimeTest);
            }

            foreach (var survey in surveys)
            {
                var entry = new MVC_Testable360Submission
                {
                    ID = survey.ID,
                    RoleName = survey.Role?.Name ?? "Роль не указана",
                    IsSelfMark = survey.IsSelfMark,
                    DateTimeTest = survey.DatetimeTest
                };
                try
                {
                    var raw = JsonSerializer.Deserialize<Test360_Result>(survey.results);
                    if (raw == null) throw new JsonException("Пустой ответ.");
                    entry.Answers = typeof(Test360_Result).GetProperties()
                        .Where(x => x.Name.StartsWith("q", StringComparison.Ordinal))
                        .OrderBy(x => x.Name)
                        .Select(x => Value(x.Name, Convert.ToString(x.GetValue(raw), Ru) ?? ""))
                        .ToList();
                }
                catch (JsonException)
                {
                    entry.ParsingError = "Первичный JSON анкеты не удалось прочитать.";
                }
                row.Submissions.Add(entry);
            }

            if (report != null)
            {
                if (surveys.Any(x => x.IsSelfMark) && report.Test360ForReportTestable != null)
                    row.Scores.AddRange(Marks360(report.Test360ForReportTestable, "Самооценка"));
                if (surveys.Any(x => !x.IsSelfMark) && report.Test360ForReportRespondents != null)
                    row.Scores.AddRange(Marks360(report.Test360ForReportRespondents, "Средняя оценка респондентов"));
            }
            row.Note = "Каждая анкета хранится отдельно. Удаление одной оценки обновит средние показатели.";
            return row;
        }

        private static List<MVC_TestableResultValue> Marks360(
            JSON_Test360_ToReport score, string group) =>
            typeof(JSON_Test360_ToReport).GetProperties()
                .Where(x => x.Name.StartsWith("q", StringComparison.Ordinal))
                .OrderBy(x => x.Name)
                .Select(x => Value(x.Name,
                    Convert.ToDouble(x.GetValue(score)).ToString("0.##", Ru), group))
                .ToList();

        private static MVC_TestableTestResult ShowMotivation(
            MotivationTest[] entries, JSON_ResultModel? report)
        {
            var row = new MVC_TestableTestResult
            {
                Kind = "Motivation", Name = "Мотивационное тестирование",
                HasData = entries.Length > 0,
                Status = entries.Length > 0 ? "Пройден" : "Не пройден"
            };
            if (entries.Length == 0) return row;
            var entry = entries[^1];
            row.FirstAnswerAt = entries.Min(x => x.DatetimeTest);
            row.LastAnswerAt = entries.Max(x => x.DatetimeTest);
            row.Answers.Add(Answer("Согласие на включение в резерв", entry.IsAgree ? "Да" : "Нет"));

            try
            {
                var raw = JsonSerializer.Deserialize<Motivation_Result>(entry.Results);
                if (raw != null)
                {
                    row.Answers.Add(Answer("Рекомендуемая должность", raw.RecomendPosition));
                    row.Answers.Add(Answer("Рекомендатель", raw.Recomendator));
                    row.Answers.Add(Answer("Согласие / причина отказа", raw.Question_YES_NO));
                    foreach (var p in typeof(Motivation_Result).GetProperties()
                        .Where(x => x.Name.StartsWith("Question_", StringComparison.Ordinal) &&
                                    x.Name != "Question_YES_NO")
                        .OrderBy(x => x.Name))
                        row.Answers.Add(Answer(p.Name, p.GetValue(raw)?.ToString()));
                }
            }
            catch (JsonException)
            {
                row.Note = "Первичные ответы не удалось прочитать.";
            }

            var result = report?.MotivationTest;
            if (result != null && result.ID > 0)
            {
                row.Scores.AddRange(new[]
                {
                    Value("Mark_01", result.Mark_01.ToString(Ru)),
                    Value("Mark_02", result.Mark_02.ToString(Ru)),
                    Value("Mark_03", result.Mark_03.ToString(Ru)),
                    Value("Mark_04", result.Mark_04.ToString(Ru)),
                    Value("Mark_05", result.Mark_05.ToString(Ru)),
                    Value("Mark_06", result.Mark_06.ToString(Ru)),
                    Value("Сумма баллов", result.SummBall.ToString(Ru))
                });
            }
            return row;
        }

        private static MVC_TestableTestResult ShowKettel(
            KT_Main[] mains, KT_TestableAnswers[] entries, int total, JSON_ResultModel? report)
        {
            var row = new MVC_TestableTestResult
            {
                Kind = "Kettel", Name = "Психологическое тестирование (Кеттел)",
                HasData = mains.Length > 0 || entries.Length > 0,
                TotalCount = total,
                AnsweredCount = entries.Select(x => x.Answer.Question.ID).Distinct().Count()
            };
            if (!row.HasData) { row.Status = "Не начат"; return row; }

            if (entries.Length > 0)
            {
                row.FirstAnswerAt = entries.Min(x => x.DateTimeAnswer);
                row.LastAnswerAt = entries.Max(x => x.DateTimeAnswer);
            }
            row.Status = total > 0 && row.AnsweredCount >= total ? "Пройден" : "В процессе";
            var main = mains.OrderByDescending(x => x.ID).FirstOrDefault();
            if (main != null)
            {
                row.Note = $"Возраст: {main.Years}; пол: {main.Gender}.";
            }

            foreach (var a in entries.OrderBy(x => x.Answer.Question.Number))
                row.Answers.Add(Answer(
                    $"Вопрос {a.Answer.Question.Number}: {a.Answer.Question.Text}",
                    $"{a.Answer.AnswerNumber}. {a.Answer.AnswerText}"));

            var kt = report?.KettelTest;
            if (kt?.Results != null)
                foreach (var score in kt.Results)
                    row.Scores.Add(Value(score.CategoryName,
                        $"Сырой балл: {score.RawScore}; оценка: {score.CategoryBall}; {score.CategoryLevel}"));
            return row;
        }

        private static MVC_TestableTestResult ShowCompetence(
            int year, Comp_TestableAnswer[] entries, int total, JSON_ResultModel? report)
        {
            var saved = entries.Where(x => x.Answer.Question.Block.Year == year).ToArray();
            var completed = saved.GroupBy(x => x.Answer.Question.ID)
                .Count(g => g.Count() == 4 &&
                            g.Select(x => x.Answer.ID).Distinct().Count() == 4 &&
                            g.Select(x => x.Mark).OrderBy(x => x)
                                .SequenceEqual(new[] { 1, 2, 3, 4 }));

            var row = new MVC_TestableTestResult
            {
                Kind = "Competention", Year = year, Name = $"Компетенции ({year} год)",
                HasData = saved.Length > 0, TotalCount = total, AnsweredCount = completed,
                Status = saved.Length == 0 ? "Не начат" :
                    (total > 0 && completed >= total ? "Пройден" : "В процессе")
            };
            if (saved.Length > 0)
            {
                row.FirstAnswerAt = saved.Min(x => x.DateTimeAnswer);
                row.LastAnswerAt = saved.Max(x => x.DateTimeAnswer);
            }

            foreach (var a in saved.OrderBy(x => x.Answer.Question.QuestionNumber)
                         .ThenBy(x => x.Answer.Number))
                row.Answers.Add(Answer(
                    $"Ситуация {a.Answer.Question.QuestionNumber}: {a.Answer.Question.QuestionText} — {a.Answer.Number}",
                    $"{a.Answer.Text} | Приоритет: {a.Mark}"));

            if (report?.CompetentionResult != null)
                foreach (var score in report.CompetentionResult.Where(x => x.Year == year))
                    row.Scores.Add(Value(score.Block,
                        $"{score.Ball} из {score.MaxBall} совпадений"));
            return row;
        }

        private static MVC_TestableResultValue Value(string label, string value, string? group = null) =>
            new() { Label = label, Value = value, Group = group };

        private static MVC_TestableAnswerDetail Answer(string title, string? value) =>
            new() { Question = title, Answer = value ?? "" };

        /// <summary>
        /// Возвращает количество удалённых первичных записей.
        /// Не удаляет само назначение периода; изменения агрегата атомарны
        /// с удалением первичных результатов.
        /// </summary>
        public async Task<int> DeleteAsync(
            int testableId, int accessId, string kind, int? year, int? questionnaireId)
        {
            if (kind is not ("Test360" or "Motivation" or "Kettel" or "Competention"))
                throw new ArgumentException("Неизвестный тип теста.");
            if (kind == "Competention" && year is not (1 or 2))
                throw new ArgumentException("Для компетенций нужно указать 1 или 2 год.");
            if (kind != "Test360" && questionnaireId.HasValue)
                throw new ArgumentException("Удаление отдельной анкеты доступно только для 360°.");

            await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            // Используем ту же блокировку AccessID, что и при прохождении тестов:
            // параллельная отправка ответа не должна перезаписать результат удаления.
            await _db.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT 1 FROM \"AccessForTestables\" WHERE \"ID\" = {accessId} FOR UPDATE");

            var access = await _db.AccessForTestables
                .Include(x => x.Testable)
                .FirstOrDefaultAsync(x => x.ID == accessId && x.Testable.ID == testableId);
            if (access == null)
                throw new ArgumentException("Период не принадлежит указанному тестируемому.");

            // Отказываемся удалять, если общий JSON нельзя восстановить:
            // иначе можно получить несогласованные первичные и отчётные данные.
            var stored = await _db.TestResults
                .Where(x => x.Access.ID == access.ID).OrderByDescending(x => x.ID)
                .FirstOrDefaultAsync();
            JSON_ResultModel? json = null;
            if (stored != null)
            {
                json = JsonSerializer.Deserialize<JSON_ResultModel>(stored.Results)
                    ?? throw new InvalidOperationException("Отчётный JSON пуст.");
            }

            int deleted;
            switch (kind)
            {
                case "Test360":
                    var surveys = await _db.Test360
                        .Where(x => x.AccessTest.ID == access.ID &&
                            (!questionnaireId.HasValue || x.ID == questionnaireId.Value))
                        .ToArrayAsync();
                    if (questionnaireId.HasValue && surveys.Length != 1)
                        throw new ArgumentException("Анкета 360° не найдена в указанном периоде.");

                    deleted = surveys.Length;
                    _db.Test360.RemoveRange(surveys);
                    await _db.SaveChangesAsync();

                    if (json != null)
                    {
                        // Пересчитываем только раздел 360° по оставшимся анкетам.
                        // Все остальные результаты остаются прежними.
                        var remaining = await _db.Test360.AsNoTracking()
                            .Include(x => x.Role)
                            .Where(x => x.AccessTest.ID == access.ID)
                            .OrderBy(x => x.DatetimeTest).ThenBy(x => x.ID)
                            .ToArrayAsync();
                        json.Tests360 = new List<JSON_Test360>();
                        json.Test360ForReportTestable = new JSON_Test360_ToReport();
                        json.Test360ForReportRespondents = new JSON_Test360_ToReport();
                        foreach (var item in remaining)
                        {
                            if (item.Role == null)
                                throw new InvalidOperationException("Не задана роль респондента у анкеты 360°.");
                            var reportItem = new JSON_Test360(item, item.Role);
                            if (item.IsSelfMark) json.Add_JSON_Test360_ToReport_Testable(reportItem);
                            else json.Add_JSON_Test360_ToReport_Respondents(reportItem);
                        }
                    }
                    break;

                case "Motivation":
                    var motivation = await _db.MotivationTests
                        .Where(x => x.AccessTest.ID == access.ID).ToArrayAsync();
                    deleted = motivation.Length;
                    _db.MotivationTests.RemoveRange(motivation);
                    if (json != null)
                        json.MotivationTest = new JSON_Motivation_Result();
                    break;

                case "Kettel":
                    var ktAnswers = await _db.KT_TestableAnswers
                        .Where(x => x.MainInfo.Access.ID == access.ID).ToArrayAsync();
                    var mains = await _db.KT_Mains
                        .Where(x => x.Access.ID == access.ID).ToArrayAsync();
                    deleted = ktAnswers.Length + mains.Length;
                    _db.KT_TestableAnswers.RemoveRange(ktAnswers);
                    _db.KT_Mains.RemoveRange(mains);
                    if (json != null)
                        json.KettelTest = new JSON_Motivation_Result.JSON_Kettel();
                    break;

                default: // Competention
                    var comp = await _db.Comp_TestableAnswers
                        .Where(x => x.Access.ID == access.ID &&
                                    x.Answer.Question.Block.Year == year!.Value).ToArrayAsync();
                    deleted = comp.Length;
                    _db.Comp_TestableAnswers.RemoveRange(comp);
                    if (json != null)
                    {
                        json.CompetentionAnswers ??= new List<JSON_Motivation_Result.JSON_Competention_Answers>();
                        json.CompetentionResult ??= new List<JSON_Motivation_Result.JSON_Competention_Result>();
                        json.CompetentionAnswers.RemoveAll(x => x.Year == year.Value);
                        json.CompetentionResult.RemoveAll(x => x.Year == year.Value);
                    }
                    break;
            }

            if (stored != null && json != null)
                stored.Results = json.GetJSON();

            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            return deleted;
        }
    }
}
