using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Anketa_4_core.Tests;

public sealed class PeriodAssignmentServiceTests : PostgresTestBase
{
    [Fact]
    public async Task AssignAsync_MovesExistingRowAndPreservesEveryKindOfPrimaryResult()
    {
        var existing = await SeedAccessAsync(
            code: "p_move_1", periodName: "Исходный",
            username: "qa_history", lkData: """{"ProfTasks":[{"Number":1}]}""");
        var destination = await SeedAccessAsync(code: "p_move_2", periodName: "Целевой");
        var aggregate = new JSON_ResultModel
        {
            MotivationTest = new JSON_Motivation_Result { ID = 77 }
        };
        await SaveResultAsync(existing.AccessId, aggregate);
        string reportJson;
        await using (var db = CreateAnketaContext())
        {
            reportJson = await db.TestResults.Where(x => x.Access.ID == existing.AccessId)
                .Select(x => x.Results).SingleAsync();
            var access = await db.AccessForTestables.FindAsync(existing.AccessId)
                ?? throw new InvalidOperationException("Access missing");

            var role = new RespondentRole { Name = "Оценка", isSelfMark = false };
            db.Test360.Add(new Test360
            {
                AccessTest = access, Role = role, results = """{"q01":3}""",
                DatetimeTest = DateTime.UtcNow
            });
            db.MotivationTests.Add(new MotivationTest
            {
                AccessTest = access, IsAgree = false,
                Results = """{"Question_YES_NO":"Отказ"}""",
                DatetimeTest = DateTime.UtcNow
            });
            var main = new KT_Main { Access = access, Years = 35, Gender = "Женщина" };
            var q = new KT_Question { Number = 1, Text = "Вопрос" };
            var a = new KT_Answer { Question = q, AnswerText = "Ответ", AnswerNumber = "1" };
            db.KT_Mains.Add(main);
            db.KT_TestableAnswers.Add(new KT_TestableAnswers
            {
                MainInfo = main, Answer = a, DateTimeAnswer = DateTime.UtcNow
            });

            var block = new Comp_Block { BlockName = "Первый год", WordInReport = "A", Year = 1 };
            var cq = new Comp_Question { Block = block, QuestionNumber = 1, QuestionText = "Ситуация" };
            var ca = new Comp_Answer
            {
                Question = cq, Number = "1", Text = "Приоритет",
                Correct_Answer = 1
            };
            db.Comp_TestableAnswers.Add(new Comp_TestableAnswer
            {
                Access = access, Answer = ca, Mark = 1,
                DateTimeAnswer = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }

        await using (var db = CreateAnketaContext())
        {
            var moved = await new PeriodAssignmentService(db).AssignAsync(
                existing.TestableId, destination.PeriodId);
            Assert.Equal(existing.AccessId, moved.ID);
        }

        await using var check = CreateAnketaContext();
        var link = await check.AccessForTestables.Include(x => x.TestPeriod)
            .SingleAsync(x => x.Testable.ID == existing.TestableId);
        Assert.Equal(existing.AccessId, link.ID);
        Assert.Equal(destination.PeriodId, link.TestPeriod.ID);
        Assert.True(link.isActive);
        Assert.Equal(1, await check.Test360.CountAsync(x => x.AccessTest.ID == existing.AccessId));
        Assert.Equal(1, await check.MotivationTests.CountAsync(x => x.AccessTest.ID == existing.AccessId));
        Assert.Equal(1, await check.KT_Mains.CountAsync(x => x.Access.ID == existing.AccessId));
        Assert.Equal(1, await check.KT_TestableAnswers.CountAsync(x => x.MainInfo.Access.ID == existing.AccessId));
        Assert.Equal(1, await check.Comp_TestableAnswers.CountAsync(x => x.Access.ID == existing.AccessId));
        Assert.Equal(reportJson, await check.TestResults.Where(x => x.Access.ID == existing.AccessId)
            .Select(x => x.Results).SingleAsync());
        Assert.Equal("""{"ProfTasks":[{"Number":1}]}""",
            await check.TestableUsers.Where(x => x.testable.ID == existing.TestableId)
                .Select(x => x.LK_Data).SingleAsync());
    }

    [Fact]
    public async Task AssignAsync_ExactSamePeriodIsIdempotentAndNeverReopensClosedAccess()
    {
        var seeded = await SeedAccessAsync(code: "p_same", active: false);
        await using (var db = CreateAnketaContext())
        {
            var service = new PeriodAssignmentService(db);
            await service.AssignAsync(seeded.TestableId, seeded.PeriodId);
            await service.AssignAsync(seeded.TestableId, seeded.PeriodId);
        }

        await using var check = CreateAnketaContext();
        var link = await check.AccessForTestables.SingleAsync();
        Assert.Equal(seeded.AccessId, link.ID);
        Assert.False(link.isActive);
    }

    [Fact]
    public async Task AssignAsync_UnknownIdentifiersDoNotChangeExistingLink()
    {
        var seeded = await SeedAccessAsync(code: "p_bad");
        await using (var db = CreateAnketaContext())
        {
            var service = new PeriodAssignmentService(db);
            await Assert.ThrowsAsync<ArgumentException>(
                () => service.AssignAsync(seeded.TestableId, 999999));
            await Assert.ThrowsAsync<ArgumentException>(
                () => service.AssignAsync(999999, seeded.PeriodId));
        }

        await using var check = CreateAnketaContext();
        var link = await check.AccessForTestables.Include(x => x.TestPeriod).SingleAsync();
        Assert.Equal(seeded.PeriodId, link.TestPeriod.ID);
        Assert.Equal(seeded.AccessId, link.ID);
    }

    [Fact]
    public async Task DbUniqueIndex_PreventsSecondAssignmentEvenWhenServiceIsBypassed()
    {
        var first = await SeedAccessAsync(code: "p_unique");
        var target = await SeedAccessAsync(code: "p_elsewhere");
        await using (var db = CreateAnketaContext())
        {
            var testable = await db.Testables.FindAsync(first.TestableId);
            var period = await db.TestPeriods.FindAsync(target.PeriodId);
            db.AccessForTestables.Add(new AccessForTestable
            {
                Testable = testable!, TestPeriod = period!, isActive = true
            });
            await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
        }

        await using var check = CreateAnketaContext();
        Assert.Equal(1, await check.AccessForTestables.CountAsync(x => x.Testable.ID == first.TestableId));
    }

    [Fact]
    public async Task AssignAsync_ConcurrentMovesKeepOneRowAndSameAccessID()
    {
        var first = await SeedAccessAsync(code: "p_concurrent");
        var target1 = await SeedAccessAsync(code: "p_target1");
        var target2 = await SeedAccessAsync(code: "p_target2");
        await using var db1 = CreateAnketaContext();
        await using var db2 = CreateAnketaContext();

        await Task.WhenAll(
            new PeriodAssignmentService(db1).AssignAsync(first.TestableId, target1.PeriodId),
            new PeriodAssignmentService(db2).AssignAsync(first.TestableId, target2.PeriodId));

        await using var check = CreateAnketaContext();
        var link = await check.AccessForTestables.Include(x => x.TestPeriod)
            .SingleAsync(x => x.Testable.ID == first.TestableId);
        Assert.Equal(first.AccessId, link.ID);
        Assert.Contains(link.TestPeriod.ID, new[] { target1.PeriodId, target2.PeriodId });
        Assert.Equal(3, await check.AccessForTestables.CountAsync());
    }

    [Fact]
    public async Task AssignAsync_LegacyDuplicatesFailSafelyWithoutDeletingAnyResults()
    {
        var first = await SeedAccessAsync(code: "p_legacy");
        var target = await SeedAccessAsync(code: "p_legacy_target");
        await using (var db = CreateAnketaContext())
        {
            // Model is intentionally moved to the old schema for this one test.
            await db.Database.ExecuteSqlRawAsync(
                "DROP INDEX \"IX_AccessForTestables_TestableID\"");
            var testable = await db.Testables.FindAsync(first.TestableId);
            var period = await db.TestPeriods.FindAsync(target.PeriodId);
            db.AccessForTestables.Add(new AccessForTestable
            {
                Testable = testable!, TestPeriod = period!, isActive = true
            });
            await db.SaveChangesAsync();
        }

        await using (var db = CreateAnketaContext())
        {
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(
                () => new PeriodAssignmentService(db).AssignAsync(first.TestableId, target.PeriodId));
            Assert.Contains("дубли", ex.Message);
        }

        await using var check = CreateAnketaContext();
        Assert.Equal(2, await check.AccessForTestables.CountAsync(x => x.Testable.ID == first.TestableId));
    }
}
