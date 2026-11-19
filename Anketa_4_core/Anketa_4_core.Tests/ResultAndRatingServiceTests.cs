using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Anketa_4_core.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Anketa_4_core.Tests;

public sealed class ResultAndRatingServiceTests : PostgresTestBase
{
    [Fact]
    public async Task SearchAsync_CombinesCodeAndStatusFilters()
    {
        var completed = await SeedAccessAsync(
            code: "2001", periodName: "Период А", filialName: "Филиал А",
            reservLevelName: "Резерв А");
        await SeedAccessAsync(
            code: "2002", periodName: "Период Б", filialName: "Филиал Б",
            reservLevelName: "Резерв Б");

        var aggregate = new JSON_ResultModel
        {
            Tests360 = new List<JSON_Test360>
            {
                new() { IsSelfMark = true, q01 = 4 },
                new() { IsSelfMark = false, q01 = 3 }
            },
            MotivationTest = new JSON_Motivation_Result { ID = 10, IsAgree = true },
            KettelTest = new JSON_Motivation_Result.JSON_Kettel
            {
                IsCompleted = true,
                QA = new List<JSON_Motivation_Result.JSON_Kettel_Question_Answer>
                {
                    new() { QuestionID = 1, QuestionNumber = "1" },
                    new() { QuestionID = 1, QuestionNumber = "1" },
                    new() { QuestionID = 2, QuestionNumber = "2" }
                }
            },
            CompetentionAnswers = new List<JSON_Motivation_Result.JSON_Competention_Answers>
            {
                new() { Year = 1, QuestionID = 1 },
                new() { Year = 1, QuestionID = 2 }
            },
            CompetentionResult = new List<JSON_Motivation_Result.JSON_Competention_Result>
            {
                new() { Year = 1, BlockID = 1, Ball = 7, MaxBall = 10 }
            }
        };
        await SaveResultAsync(completed.AccessId, aggregate);

        await using var db = CreateAnketaContext();
        var page = await new ResultModuleService(db).SearchAsync(new MVC_ResultFilter
        {
            Code = " 2001 ",
            PeriodID = completed.PeriodId,
            FilialID = completed.FilialId,
            Self360 = "done",
            RespondentsLessThan = 2,
            Motivation = "done",
            Kettel = "done",
            Competence1 = "done",
            Competence2 = "not",
            Page = 0
        });

        var row = Assert.Single(page.Items);
        Assert.Equal(1, page.Page);
        Assert.Equal(100, page.PageSize);
        Assert.Equal(1, page.Total);
        Assert.True(row.HasJson);
        Assert.False(row.InvalidJson);
        Assert.True(row.HasSelf360);
        Assert.Equal(1, row.Respondents360);
        Assert.Equal(2, row.KettelAnswered);
        Assert.Equal("done", row.MotivationStatus);
        Assert.Equal("done", row.KettelStatus);
        Assert.Equal("done", row.Competence1Status);
        Assert.Equal("not", row.Competence2Status);
    }

    [Fact]
    public async Task SearchAsync_ShowsInvalidJsonWithoutStatusFilterButExcludesItFromStatusFilters()
    {
        var seeded = await SeedAccessAsync(code: "2003");
        await using (var db = CreateAnketaContext())
        {
            var access = await db.AccessForTestables.FirstAsync(x => x.ID == seeded.AccessId);
            db.TestResults.Add(new TestResult { Access = access, Results = "{invalid" });
            await db.SaveChangesAsync();
        }

        await using var readDb = CreateAnketaContext();
        var service = new ResultModuleService(readDb);
        var unfiltered = await service.SearchAsync(new MVC_ResultFilter());
        var row = Assert.Single(unfiltered.Items);
        Assert.True(row.HasJson);
        Assert.True(row.InvalidJson);

        var filtered = await service.SearchAsync(new MVC_ResultFilter { Motivation = "done" });
        Assert.Empty(filtered.Items);
        Assert.Equal(0, filtered.Total);
    }

    [Fact]
    public async Task SearchAsync_ClampsHighPageAndHandlesMissingResults()
    {
        await SeedAccessAsync(code: "2004");
        await using var db = CreateAnketaContext();

        var page = await new ResultModuleService(db).SearchAsync(
            new MVC_ResultFilter { Page = int.MaxValue });

        Assert.Equal(100000, page.Page);
        Assert.Empty(page.Items);
        Assert.Equal(1, page.Total);
    }

    [Fact]
    public async Task DetailAsync_ReturnsEmptyAggregateWhenNoResultExists()
    {
        var seeded = await SeedAccessAsync(code: "2005");
        await using var db = CreateAnketaContext();

        var detail = await new ResultModuleService(db).DetailAsync(seeded.AccessId);

        Assert.NotNull(detail);
        Assert.Equal("2005", detail!.TestableCode);
        Assert.Empty(detail.Data.Tests360);
        Assert.Empty(detail.Data.CompetentionAnswers);
    }

    [Fact]
    public async Task DetailAsync_ReturnsNullForUnknownAccessAndRejectsInvalidJson()
    {
        await using (var db = CreateAnketaContext())
            Assert.Null(await new ResultModuleService(db).DetailAsync(999999));

        var seeded = await SeedAccessAsync(code: "2006");
        await using (var db = CreateAnketaContext())
        {
            var access = await db.AccessForTestables.FirstAsync(x => x.ID == seeded.AccessId);
            db.TestResults.Add(new TestResult { Access = access, Results = "not-json" });
            await db.SaveChangesAsync();
        }

        await using var readDb = CreateAnketaContext();
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => new ResultModuleService(readDb).DetailAsync(seeded.AccessId));
        Assert.Contains("Ошибка чтения JSON", exception.Message);
    }

    [Fact]
    public async Task DetailAsync_RejectsDuplicateRowsAndForeignAccessJson()
    {
        var seeded = await SeedAccessAsync(code: "2007");
        await using (var db = CreateAnketaContext())
        {
            var access = await db.AccessForTestables.FirstAsync(x => x.ID == seeded.AccessId);
            db.TestResults.AddRange(
                new TestResult { Access = access, Results = new JSON_ResultModel().GetJSON() },
                new TestResult { Access = access, Results = new JSON_ResultModel().GetJSON() });
            await db.SaveChangesAsync();
        }

        await using (var db = CreateAnketaContext())
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => new ResultModuleService(db).DetailAsync(seeded.AccessId));

        var foreign = await SeedAccessAsync(code: "2008");
        await using (var db = CreateAnketaContext())
        {
            var access = await db.AccessForTestables.FirstAsync(x => x.ID == foreign.AccessId);
            db.TestResults.Add(new TestResult
            {
                Access = access,
                Results = new JSON_ResultModel { AccessID = foreign.AccessId + 100 }.GetJSON()
            });
            await db.SaveChangesAsync();
        }

        await using var verifyDb = CreateAnketaContext();
        var mismatch = await Assert.ThrowsAsync<InvalidOperationException>(
            () => new ResultModuleService(verifyDb).DetailAsync(foreign.AccessId));
        Assert.Contains("другому периоду", mismatch.Message);
    }

    [Fact]
    public async Task Rating_ReturnsHelpfulEmptyStateWhenNoPeriodExists()
    {
        await using var db = CreateAnketaContext();
        var snapshot = await new LKPeriodRatingService(db).GetForTestableAsync(999999);

        Assert.Equal(0, snapshot.Participants);
        Assert.Null(snapshot.Current);
        Assert.Contains("не назначен", snapshot.Formula);
    }

    [Fact]
    public async Task Rating_DeduplicatesAssignmentsAndCalculatesWeightedScore()
    {
        var fullPlan = FullPlanJson();
        var best = await SeedAccessAsync(
            code: "2010", username: "best", lkData: fullPlan, yearTraining: 1);
        var other = await SeedAccessAsync(
            code: "2011", username: "other", lkData: "{}", yearTraining: 1);

        var aggregate = new JSON_ResultModel
        {
            Tests360 = new List<JSON_Test360> { new() { IsSelfMark = true } },
            MotivationTest = new JSON_Motivation_Result { ID = 1 },
            KettelTest = new JSON_Motivation_Result.JSON_Kettel { IsCompleted = true },
            CompetentionResult = new List<JSON_Motivation_Result.JSON_Competention_Result>
            {
                new() { Year = 1, BlockID = 1, Ball = 8, MaxBall = 10 }
            }
        };
        await SaveResultAsync(best.AccessId, aggregate);

        await AddAccessToExistingTestableAsync(other.TestableId, best.PeriodId);

        await using var db = CreateAnketaContext();
        var snapshot = await new LKPeriodRatingService(db).GetForTestableAsync(best.TestableId);

        Assert.Equal(2, snapshot.Participants);
        Assert.NotNull(snapshot.Current);
        Assert.Equal(1, snapshot.Current!.Place);
        Assert.Equal("2010", snapshot.Current.Code);
        Assert.Equal(100d, snapshot.Current.TableCompletion);
        Assert.Equal(100d, snapshot.Current.TestCompletion);
        Assert.Equal(80d, snapshot.Current.CompetenceQuality);
        Assert.Equal(95d, snapshot.Current.TotalScore);
        Assert.Equal(2, snapshot.Top.Count);
    }

    [Fact]
    public async Task Rating_TreatsCorruptedResultJsonAsZeroInsteadOfCrashing()
    {
        var seeded = await SeedAccessAsync(code: "2012", username: "broken");
        await using (var db = CreateAnketaContext())
        {
            var access = await db.AccessForTestables.FirstAsync(x => x.ID == seeded.AccessId);
            db.TestResults.Add(new TestResult { Access = access, Results = "{broken" });
            await db.SaveChangesAsync();
        }

        await using var readDb = CreateAnketaContext();
        var snapshot = await new LKPeriodRatingService(readDb).GetForTestableAsync(seeded.TestableId);

        Assert.NotNull(snapshot.Current);
        Assert.Equal(0, snapshot.Current!.TestCompletion);
        Assert.Equal(0, snapshot.Current.CompetenceQuality);
    }

    private static string FullPlanJson()
    {
        var tasks = string.Join(",", Enumerable.Range(1, 3).Select(i =>
            $$"""{"Number":{{i}},"ProfTaskName":"Задача {{i}}","CompetenceZone":"Зона","DateFrom":"2026-01-01","DateTo":"2026-02-01","AnalisEffect":"Анализ","WhatCouldBeDoneDifferently":"Иначе","Difficult":"Сложность"}"""));
        var books = """
        [
          {"BookCategory":"Профессиональные компетенции","BookName":"Книга 1","Author":"Автор","DateFrom":"2026-01-01","DateTo":"2026-02-01","Gold":"Ценность","PractiсeSkills":"Навыки"},
          {"BookCategory":"Личностно-деловые и управленческие компетенции","BookName":"Книга 2","Author":"Автор","DateFrom":"2026-01-01","DateTo":"2026-02-01","Gold":"Ценность","PractiсeSkills":"Навыки"},
          {"BookCategory":"Личностно-деловые и управленческие компетенции","BookName":"Книга 3","Author":"Автор","DateFrom":"2026-01-01","DateTo":"2026-02-01","Gold":"Ценность","PractiсeSkills":"Навыки"}
        ]
        """;
        return $$"""
        {
          "ProfTasks":[{{tasks}}],
          "TrainingProgramms":[{"LiveSityations":"Применено","Difficult":"Нет","InfluenceAsWorker":"Влияние","InfluenceAsFilial":"Влияние"}],
          "DistanceLearning":[
            {"IsPlanned":true,"Title":"Курс 1","DateFrom":"2026-01-01","DateTo":"2026-02-01","Certificate":"Есть"},
            {"IsPlanned":true,"Title":"Курс 2","DateFrom":"2026-01-01","DateTo":"2026-02-01","Certificate":"Есть"}
          ],
          "ReportBooks":{{books}}
        }
        """;
    }
}
