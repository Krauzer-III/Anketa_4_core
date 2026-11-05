using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Anketa_4_core.Services
{
    /// <summary>
    /// Модуль результатов Anketa 4: агрегаты уже рассчитаны в TestResult.Results.
    /// Не читаем первичные Test360, KT_TestableAnswers или Comp_TestableAnswers.
    /// Одна строка результатов = один AccessForTestable, даже для одного кода.
    /// </summary>
    public class ResultModuleService
    {
        private readonly AnketaContext _db;
        public ResultModuleService(AnketaContext db) => _db = db;

        private const int PageSize = 100;

        public async Task<MVC_ResultSearchPage> SearchAsync(MVC_ResultFilter filter)
        {
            int page = Math.Clamp(filter.Page, 1, 100000);
            var query = _db.AccessForTestables.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Code))
            {
                string code = filter.Code.Trim();
                query = query.Where(x => x.Testable.Code.Contains(code));
            }
            if (filter.PeriodID.GetValueOrDefault() > 0)
                query = query.Where(x => x.TestPeriod.ID == filter.PeriodID);
            if (filter.FilialID.GetValueOrDefault() > 0)
                query = query.Where(x => x.Testable.filial.ID == filter.FilialID);
            if (filter.ReservLevelID.GetValueOrDefault() > 0)
                query = query.Where(x => x.Testable.reservLevel.ID == filter.ReservLevelID);

            var needsJsonFilter = HasStatusFilter(filter);
            int total;
            List<MVC_ResultRow> rows;
            if (needsJsonFilter)
            {
                // Статусы находятся в JSON-тексте; фильтруем после десериализации.
                // В обычном режиме без фильтров JSON читается только для страницы.
                var matches = await query.OrderBy(x => x.Testable.Code).ThenByDescending(x => x.ID)
                    .Select(x => new MVC_ResultRow
                    {
                        AccessID = x.ID, TestableID = x.Testable.ID,
                        Code = x.Testable.Code, Period = x.TestPeriod.GroupName,
                        Filial = x.Testable.filial.FilialName,
                        ReservLevel = x.Testable.reservLevel == null ? "" :
                            x.Testable.reservLevel.ReservLevelName,
                        IsActive = x.isActive
                    }).ToListAsync();
                await EnrichAsync(matches);
                matches = matches.Where(x => Matches(x, filter)).ToList();
                total = matches.Count;
                rows = matches.Skip((page - 1) * PageSize).Take(PageSize).ToList();
            }
            else
            {
                total = await query.CountAsync();
                rows = await query.OrderBy(x => x.Testable.Code).ThenByDescending(x => x.ID)
                    .Skip((page - 1) * PageSize).Take(PageSize)
                    .Select(x => new MVC_ResultRow
                    {
                        AccessID = x.ID, TestableID = x.Testable.ID,
                        Code = x.Testable.Code, Period = x.TestPeriod.GroupName,
                        Filial = x.Testable.filial.FilialName,
                        ReservLevel = x.Testable.reservLevel == null ? "" :
                            x.Testable.reservLevel.ReservLevelName,
                        IsActive = x.isActive
                    }).ToListAsync();
                await EnrichAsync(rows);
            }

            return new MVC_ResultSearchPage
            {
                Items = rows, Total = total, Page = page, PageSize = PageSize
            };
        }

        private async Task EnrichAsync(IReadOnlyCollection<MVC_ResultRow> rows)
        {
            if (rows.Count == 0) return;
            var lookup = rows.ToDictionary(x => x.AccessID);
            var ids = lookup.Keys.ToArray();

            // Ограничиваем размер IN-запроса. Не тянем все результаты при обычном
            // постраничном просмотре без статусов.
            for (int start = 0; start < ids.Length; start += 250)
            {
                int[] chunk = ids.Skip(start).Take(250).ToArray();
                var reports = await _db.TestResults.AsNoTracking()
                    .Where(x => chunk.Contains(x.Access.ID))
                    .Select(x => new { x.ID, AccessID = x.Access.ID, x.Results })
                    .ToArrayAsync();

                foreach (var group in reports.GroupBy(x => x.AccessID))
                {
                    var row = lookup[group.Key];
                    var json = group.OrderByDescending(x => x.ID).First().Results;
                    row.HasJson = true;
                    if (group.Count() > 1 || string.IsNullOrWhiteSpace(json))
                    {
                        row.InvalidJson = true;
                        continue;
                    }
                    try
                    {
                        var result = JsonSerializer.Deserialize<JSON_ResultModel>(json);
                        if (result == null ||
                            (result.AccessID != 0 && result.AccessID != row.AccessID))
                        {
                            row.InvalidJson = true;
                            continue;
                        }
                        Fill(row, result);
                    }
                    catch (Exception ex) when (ex is JsonException or NotSupportedException)
                    {
                        row.InvalidJson = true;
                    }
                }
            }
        }

        private static void Fill(MVC_ResultRow row, JSON_ResultModel result)
        {
            row.HasSelf360 = result.Tests360?.Any(x => x.IsSelfMark) == true;
            row.Respondents360 = result.Tests360?.Count(x => !x.IsSelfMark) ?? 0;

            if ((result.MotivationTest?.ID ?? 0) > 0)
            {
                row.MotivationStatus = "done";
                row.MotivationAgree = result.MotivationTest!.IsAgree;
            }

            var kt = result.KettelTest;
            row.KettelAnswered = kt?.QA?.Select(x => x.QuestionID > 0
                ? "id" + x.QuestionID : x.QuestionNumber).Distinct().Count() ?? 0;
            row.KettelStatus = (kt?.IsCompleted == true || kt?.Results?.Count > 0)
                ? "done" : row.KettelAnswered > 0 || (kt?.ID ?? -1) > 0
                    ? "progress" : "not";

            row.Competence1Answered = result.CompetentionAnswers?
                .Count(x => x.Year == 1) ?? 0;
            row.Competence2Answered = result.CompetentionAnswers?
                .Count(x => x.Year == 2) ?? 0;
            row.Competence1Status = CompetenceStatus(result, 1,
                row.Competence1Answered);
            row.Competence2Status = CompetenceStatus(result, 2,
                row.Competence2Answered);
        }

        private static string CompetenceStatus(
            JSON_ResultModel result, int year, int answered)
        {
            if (result.CompetentionResult?.Any(x => x.Year == year) == true)
                return "done";
            return answered > 0 ? "progress" : "not";
        }

        private static bool HasStatusFilter(MVC_ResultFilter f) =>
            f.Self360 != "all" || f.RespondentsLessThan.HasValue ||
            f.Motivation != "all" || f.Kettel != "all" ||
            f.Competence1 != "all" || f.Competence2 != "all";

        private static bool Matches(MVC_ResultRow x, MVC_ResultFilter f)
        {
            if (x.InvalidJson) return false; // не выдаём повреждение за непрохождение
            if (f.Self360 == "done" && !x.HasSelf360) return false;
            if (f.Self360 == "not" && x.HasSelf360) return false;
            if (f.RespondentsLessThan.HasValue &&
                x.Respondents360 >= f.RespondentsLessThan.Value) return false;
            return MatchStatus(x.MotivationStatus, f.Motivation)
                && MatchStatus(x.KettelStatus, f.Kettel)
                && MatchStatus(x.Competence1Status, f.Competence1)
                && MatchStatus(x.Competence2Status, f.Competence2);
        }

        private static bool MatchStatus(string actual, string expected) =>
            expected is null or "" or "all" || actual == expected;

        public async Task<MVC_ResultDetail?> DetailAsync(int accessId)
        {
            var info = await _db.AccessForTestables.AsNoTracking()
                .Where(x => x.ID == accessId)
                .Select(x => new
                {
                    x.ID, Code = x.Testable.Code,
                    Period = x.TestPeriod.GroupName
                }).FirstOrDefaultAsync();
            if (info == null) return null;

            var texts = await _db.TestResults.AsNoTracking()
                .Where(x => x.Access.ID == accessId)
                .Select(x => new { x.ID, x.Results }).ToArrayAsync();

            if (texts.Length > 1)
                throw new InvalidOperationException(
                    "Для периода найдено несколько TestResult — требуется устранить дубли.");

            var aggregate = new JSON_ResultModel();
            if (texts.Length > 0)
            {
                if (string.IsNullOrWhiteSpace(texts[0].Results))
                    throw new InvalidOperationException("Отчётный JSON пуст.");
                try
                {
                    aggregate = JsonSerializer.Deserialize<JSON_ResultModel>(texts[0].Results)
                        ?? throw new JsonException("Отчётный JSON пуст.");
                }
                catch (JsonException)
                {
                    throw new InvalidOperationException("Ошибка чтения JSON результатов.");
                }
                if (aggregate.AccessID != 0 && aggregate.AccessID != accessId)
                    throw new InvalidOperationException("JSON относится к другому периоду.");
            }

            return new MVC_ResultDetail
            {
                AccessID = info.ID,
                TestableCode = info.Code,
                PeriodName = info.Period,
                Data = aggregate
            };
        }
    }
}
