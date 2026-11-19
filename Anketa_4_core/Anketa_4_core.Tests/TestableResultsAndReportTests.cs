using System.Text.Json;
using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Services;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Anketa_4_core.Tests;

public sealed class TestableResultsAndReportTests : PostgresTestBase
{
    [Fact]
    public async Task LoadAsync_ReturnsFiveIndependentTestSectionsPerPeriod()
    {
        var seeded = await SeedAccessAsync(code: "3001", periodName: "Период 3001");

        await using var db = CreateAnketaContext();
        var periods = await new TestableResultsService(db).LoadAsync(seeded.TestableId);

        var period = Assert.Single(periods);
        Assert.Equal(seeded.AccessId, period.AccessID);
        Assert.Equal("Период 3001", period.PeriodName);
        Assert.Equal(5, period.Tests.Count);
        Assert.Equal(
            new[] { "Test360", "Motivation", "Kettel", "Competention", "Competention" },
            period.Tests.Select(x => x.Kind));
        Assert.Equal(1, period.Tests.Count(x => x.Year == 1));
        Assert.Equal(1, period.Tests.Count(x => x.Year == 2));
        Assert.All(period.Tests, test => Assert.False(test.HasData));
    }

    [Fact]
    public async Task LoadAsync_ReportsCorruptedAggregateAndPrimaryAnswerJson()
    {
        var seeded = await SeedAccessAsync(code: "3002");
        await using (var db = CreateAnketaContext())
        {
            var access = await db.AccessForTestables.FirstAsync(x => x.ID == seeded.AccessId);
            var role = new RespondentRole { Name = "Эксперт", isSelfMark = false };
            db.RespondentRoles.Add(role);
            db.Test360.Add(new Test360
            {
                AccessTest = access,
                Role = role,
                IsSelfMark = false,
                DatetimeTest = DateTime.UtcNow,
                results = "{invalid"
            });
            db.TestResults.Add(new TestResult { Access = access, Results = "{broken" });
            await db.SaveChangesAsync();
        }

        await using var readDb = CreateAnketaContext();
        var period = Assert.Single(await new TestableResultsService(readDb).LoadAsync(seeded.TestableId));

        Assert.Contains("повреждён", period.JsonError);
        var survey = Assert.Single(period.Tests.Single(x => x.Kind == "Test360").Submissions);
        Assert.Contains("не удалось прочитать", survey.ParsingError);
    }

    [Fact]
    public async Task DeleteAsync_RejectsInvalidKindYearAndQuestionnaireCombination()
    {
        var seeded = await SeedAccessAsync(code: "3003");
        await using var db = CreateAnketaContext();
        var service = new TestableResultsService(db);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.DeleteAsync(seeded.TestableId, seeded.AccessId, "Unknown", null, null));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.DeleteAsync(seeded.TestableId, seeded.AccessId, "Competention", 3, null));
        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.DeleteAsync(seeded.TestableId, seeded.AccessId, "Motivation", null, 1));
    }

    [Fact]
    public async Task DeleteAsync_RejectsAccessBelongingToAnotherTestable()
    {
        var first = await SeedAccessAsync(code: "3004");
        var second = await SeedAccessAsync(code: "3005");

        await using var db = CreateAnketaContext();
        await Assert.ThrowsAsync<ArgumentException>(() =>
            new TestableResultsService(db).DeleteAsync(
                first.TestableId, second.AccessId, "Motivation", null, null));
    }

    [Fact]
    public async Task DeleteAsync_RemovesMotivationPrimaryRowsAndOnlyItsAggregateSection()
    {
        var seeded = await SeedAccessAsync(code: "3006");
        await using (var db = CreateAnketaContext())
        {
            var access = await db.AccessForTestables.Include(x => x.Testable)
                .FirstAsync(x => x.ID == seeded.AccessId);
            db.MotivationTests.Add(new MotivationTest
            {
                AccessTest = access,
                IsAgree = true,
                DatetimeTest = DateTime.UtcNow,
                Results = "{}"
            });
            db.TestResults.Add(new TestResult
            {
                Access = access,
                Results = new JSON_ResultModel
                {
                    Tests360 = new List<JSON_Test360> { new() { IsSelfMark = true, q01 = 4 } },
                    MotivationTest = new JSON_Motivation_Result { ID = 99, IsAgree = true }
                }.GetJSON()
            });
            await db.SaveChangesAsync();
        }

        await using (var db = CreateAnketaContext())
        {
            var deleted = await new TestableResultsService(db).DeleteAsync(
                seeded.TestableId, seeded.AccessId, "Motivation", null, null);
            Assert.Equal(1, deleted);
        }

        await using var verifyDb = CreateAnketaContext();
        Assert.Equal(0, await verifyDb.MotivationTests.CountAsync());
        var stored = await verifyDb.TestResults.SingleAsync(x => x.Access.ID == seeded.AccessId);
        var aggregate = JsonSerializer.Deserialize<JSON_ResultModel>(stored.Results)!;
        Assert.Equal(0, aggregate.MotivationTest.ID);
        Assert.Single(aggregate.Tests360);
        Assert.True(aggregate.Tests360[0].IsSelfMark);
    }

    [Fact]
    public async Task DeleteAsync_RemovesOnlySelected360SubmissionAndRecalculatesAggregate()
    {
        var seeded = await SeedAccessAsync(code: "3007");
        int respondentId;
        await using (var db = CreateAnketaContext())
        {
            var access = await db.AccessForTestables.Include(x => x.Testable)
                .FirstAsync(x => x.ID == seeded.AccessId);
            var selfRole = new RespondentRole { Name = "Самооценка", isSelfMark = true };
            var respondentRole = new RespondentRole { Name = "Эксперт", isSelfMark = false };
            db.RespondentRoles.AddRange(selfRole, respondentRole);
            var self = new Test360
            {
                AccessTest = access, Role = selfRole, IsSelfMark = true,
                DatetimeTest = DateTime.UtcNow.AddMinutes(-2),
                results = JsonSerializer.Serialize(new Test360_Result { q01 = 4, q02 = 3 })
            };
            var respondent = new Test360
            {
                AccessTest = access, Role = respondentRole, IsSelfMark = false,
                DatetimeTest = DateTime.UtcNow,
                results = JsonSerializer.Serialize(new Test360_Result { q01 = 2, q02 = 1 })
            };
            db.Test360.AddRange(self, respondent);
            await db.SaveChangesAsync();
            respondentId = respondent.ID;

            var aggregate = new JSON_ResultModel();
            aggregate.Add_JSON_Test360_ToReport_Testable(new JSON_Test360
            {
                ID = self.ID, IsSelfMark = true, q01 = 4, q02 = 3
            });
            aggregate.Add_JSON_Test360_ToReport_Respondents(new JSON_Test360
            {
                ID = respondent.ID, IsSelfMark = false, q01 = 2, q02 = 1
            });
            db.TestResults.Add(new TestResult { Access = access, Results = aggregate.GetJSON() });
            await db.SaveChangesAsync();
        }

        await using (var db = CreateAnketaContext())
        {
            var deleted = await new TestableResultsService(db).DeleteAsync(
                seeded.TestableId, seeded.AccessId, "Test360", null, respondentId);
            Assert.Equal(1, deleted);
        }

        await using var verifyDb = CreateAnketaContext();
        var remaining = await verifyDb.Test360.ToArrayAsync();
        var one = Assert.Single(remaining);
        Assert.True(one.IsSelfMark);
        var result = JsonSerializer.Deserialize<JSON_ResultModel>(
            (await verifyDb.TestResults.SingleAsync()).Results)!;
        var aggregateSelf = Assert.Single(result.Tests360);
        Assert.True(aggregateSelf.IsSelfMark);
        Assert.Equal(4, aggregateSelf.q01);
        Assert.Empty(result.Test360ForReportRespondents == null
            ? Array.Empty<double>()
            : new[] { result.Test360ForReportRespondents.q01 });
    }

    [Fact]
    public async Task DeleteAsync_RemovesCompetenceDataForOneYearOnly()
    {
        var seeded = await SeedAccessAsync(code: "3008");
        await using (var db = CreateAnketaContext())
        {
            var access = await db.AccessForTestables.FirstAsync(x => x.ID == seeded.AccessId);
            var block1 = new Comp_Block { BlockName = "Год 1", Year = 1 };
            var block2 = new Comp_Block { BlockName = "Год 2", Year = 2 };
            var question1 = new Comp_Question { QuestionNumber = 1, QuestionText = "Q1", Block = block1 };
            var question2 = new Comp_Question { QuestionNumber = 1, QuestionText = "Q2", Block = block2 };
            var answer1 = new Comp_Answer { Question = question1, Number = "1", Text = "A1", Correct_Answer = 1 };
            var answer2 = new Comp_Answer { Question = question2, Number = "1", Text = "A2", Correct_Answer = 1 };
            db.Comp_Blocks.AddRange(block1, block2);
            db.Comp_Questions.AddRange(question1, question2);
            db.Comp_Answers.AddRange(answer1, answer2);
            await db.SaveChangesAsync();
            db.Comp_TestableAnswers.AddRange(
                new Comp_TestableAnswer { Access = access, Answer = answer1, Mark = 1, DateTimeAnswer = DateTime.UtcNow },
                new Comp_TestableAnswer { Access = access, Answer = answer2, Mark = 1, DateTimeAnswer = DateTime.UtcNow });
            var aggregate = new JSON_ResultModel
            {
                CompetentionAnswers = new List<JSON_Motivation_Result.JSON_Competention_Answers>
                {
                    new() { Year = 1, QuestionID = question1.ID },
                    new() { Year = 2, QuestionID = question2.ID }
                },
                CompetentionResult = new List<JSON_Motivation_Result.JSON_Competention_Result>
                {
                    new() { Year = 1, BlockID = block1.ID, Ball = 1, MaxBall = 1 },
                    new() { Year = 2, BlockID = block2.ID, Ball = 1, MaxBall = 1 }
                }
            };
            db.TestResults.Add(new TestResult { Access = access, Results = aggregate.GetJSON() });
            await db.SaveChangesAsync();
        }

        await using (var db = CreateAnketaContext())
        {
            var deleted = await new TestableResultsService(db).DeleteAsync(
                seeded.TestableId, seeded.AccessId, "Competention", 1, null);
            Assert.Equal(1, deleted);
        }

        await using var verifyDb = CreateAnketaContext();
        Assert.Equal(1, await verifyDb.Comp_TestableAnswers.CountAsync());
        var saved = JsonSerializer.Deserialize<JSON_ResultModel>(
            (await verifyDb.TestResults.SingleAsync()).Results)!;
        Assert.DoesNotContain(saved.CompetentionAnswers, x => x.Year == 1);
        Assert.Contains(saved.CompetentionAnswers, x => x.Year == 2);
        Assert.DoesNotContain(saved.CompetentionResult, x => x.Year == 1);
        Assert.Contains(saved.CompetentionResult, x => x.Year == 2);
    }

    [Fact]
    public void Generate_RejectsEmptyOversizedAndMissingTemplateInputs()
    {
        Assert.Throws<ArgumentException>(() =>
            ReportGenerator.Generate(Array.Empty<ReportGenerator.Subject>(), "missing.xlsx"));
        Assert.Throws<ArgumentException>(() =>
            ReportGenerator.Generate(
                Enumerable.Range(1, 101).Select(i => new ReportGenerator.Subject
                {
                    Code = i.ToString(), Period = "P"
                }).ToArray(),
                "missing.xlsx"));
        Assert.Throws<FileNotFoundException>(() =>
            ReportGenerator.Generate(
                new[] { new ReportGenerator.Subject { Code = "1", Period = "P" } },
                Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx")));
    }

    [Fact]
    public void Generate_RejectsMixedPeriodsAndMissingCompetenceSheet()
    {
        var template = CreateTemplate("Компетенции (1 год)", "Компетенции (2 год)");
        try
        {
            Assert.Throws<InvalidOperationException>(() =>
                ReportGenerator.Generate(new[]
                {
                    new ReportGenerator.Subject { Code = "1", Period = "P1" },
                    new ReportGenerator.Subject { Code = "2", Period = "P2" }
                }, template));
        }
        finally { File.Delete(template); }

        var invalid = CreateTemplate();
        try
        {
            var exception = Assert.Throws<InvalidOperationException>(() =>
                ReportGenerator.Generate(
                    new[] { new ReportGenerator.Subject { Code = "1", Period = "P" } },
                    invalid));
            Assert.Contains("лист компетенций", exception.Message);
        }
        finally { File.Delete(invalid); }
    }

    [Fact]
    public void Generate_WritesIdentityAndAllReportSectionsAndCopiesSecondYearSheet()
    {
        var template = CreateTemplate("Компетенции (1 год)");
        try
        {
            var data = new JSON_ResultModel
            {
                Tests360 = new List<JSON_Test360>
                {
                    new() { IsSelfMark = true },
                    new() { IsSelfMark = false },
                    new() { IsSelfMark = false }
                },
                Test360ForReportTestable = new JSON_Test360_ToReport { q02 = 3 },
                Test360ForReportRespondents = new JSON_Test360_ToReport { q02 = 6 },
                MotivationTest = new JSON_Motivation_Result
                {
                    ID = 1, Mark_01 = 1, Mark_02 = 2, Mark_03 = 3,
                    Mark_04 = 4, Mark_05 = 5, Mark_06 = 6
                },
                KettelTest = new JSON_Motivation_Result.JSON_Kettel
                {
                    Results = new List<JSON_Motivation_Result.JSON_Kettel_KategoryResult>
                    {
                        new() { CategoryName = "A", CategoryBall = 3, RawScore = 8 }
                    }
                },
                CompetentionResult = new List<JSON_Motivation_Result.JSON_Competention_Result>
                {
                    new() { Year = 1, BlockWordInReport = "C", Ball = 9, MaxBall = 12, LevelNumber = 4 },
                    new() { Year = 2, BlockWordInReport = "D", Ball = 12, MaxBall = 12, LevelNumber = 5 },
                    new() { Year = 1, BlockWordInReport = "INVALID!", Ball = 10, MaxBall = 12, LevelNumber = 5 }
                }
            };
            var subject = new ReportGenerator.Subject
            {
                AccessID = 1, Code = "000123", Filial = "Филиал",
                Name = "Тестируемый", Position = "Начальник",
                Period = "Период X", Data = data
            };

            var bytes = ReportGenerator.Generate(new[] { subject }, template);
            using var output = new XLWorkbook(new MemoryStream(bytes));

            var sheet360 = output.Worksheet("360° ");
            Assert.Equal("Период X", sheet360.Cell("B8").GetString());
            Assert.Equal("000123", sheet360.Cell("B11").GetString());
            Assert.Equal("Тестируемый", sheet360.Cell("C11").GetString());
            Assert.Equal(5d, sheet360.Cell("H11").GetDouble(), 3);
            Assert.Equal(3d, sheet360.Cell("H12").GetDouble(), 3);
            Assert.Equal(6d, sheet360.Cell("H13").GetDouble(), 3);
            Assert.Equal(2d, sheet360.Cell("AB13").GetDouble(), 3);

            var motivation = output.Worksheet("Мотивац. опросник");
            Assert.Equal(3d, motivation.Cell("F13").GetDouble(), 3);
            Assert.Equal(7d, motivation.Cell("G13").GetDouble(), 3);
            Assert.Equal(5d, motivation.Cell("I13").GetDouble(), 3);
            Assert.Equal(6d, motivation.Cell("J13").GetDouble(), 3);

            var kettel = output.Worksheet("Псих обследование");
            Assert.Equal(3d, kettel.Cell("F13").GetDouble(), 3);
            Assert.Equal("Н", kettel.Cell("G13").GetString());

            Assert.Equal(4d, output.Worksheet("Компетенции (1 год)").Cell("C14").GetDouble(), 3);
            Assert.Equal(5d, output.Worksheet("Компетенции (2 год)").Cell("D14").GetDouble(), 3);
            Assert.True(output.Worksheets.Contains("Компетенции (2 год)"));
        }
        finally { File.Delete(template); }
    }

    private static string CreateTemplate(params string[] competenceSheets)
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
        using var workbook = new XLWorkbook();
        workbook.AddWorksheet("360° ");
        workbook.AddWorksheet("Мотивац. опросник");
        workbook.AddWorksheet("Псих обследование");
        foreach (var name in competenceSheets)
            workbook.AddWorksheet(name);
        workbook.SaveAs(path);
        return path;
    }
}
