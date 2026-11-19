using System.Text.Json;
using Anketa_4_core.Controllers.Users;
using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Anketa_4_core.Tests;

public sealed class MotivationControllerTests : PostgresTestBase
{
    [Fact]
    public async Task Index_Get_PersonalUserOpensOwnForm()
    {
        var user = await CreateUserAsync("qa_motivation_index", "testable_user");
        var seeded = await SeedAccessAsync(code: "7001", username: user.UserName);

        await using var db = CreateAnketaContext();
        var result = Assert.IsType<ViewResult>(await CreateController(db, user).Index());

        Assert.Equal("MotivationTest", result.ViewName);
        Assert.Equal(seeded.AccessId, Assert.IsType<MVC_MotivationModel>(result.Model).AccessTestID);
    }

    [Fact]
    public async Task MotivationTest_Refusal_SavesReasonAndZeroScore()
    {
        var user = await CreateUserAsync("qa_motivation_no", "testable_user");
        var seeded = await SeedAccessAsync(code: "7002", username: user.UserName);

        await using (var db = CreateAnketaContext())
        {
            var result = await CreateController(db, user).MotivationTest(new MVC_MotivationModel
            {
                AccessTestID = seeded.AccessId,
                IsAgree = false,
                RecomendPosition = "Руководитель",
                Recomendator = "Комиссия",
                Question_YES_NO = "Не готов участвовать по семейным обстоятельствам"
            });
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Finish", redirect.ActionName);
        }

        await using var verifyDb = CreateAnketaContext();
        var entity = await verifyDb.MotivationTests.SingleAsync();
        Assert.False(entity.IsAgree);
        var raw = JsonSerializer.Deserialize<Motivation_Result>(entity.Results)!;
        Assert.Contains("семейным обстоятельствам", raw.Question_YES_NO);

        var aggregate = JsonSerializer.Deserialize<JSON_ResultModel>(
            (await verifyDb.TestResults.SingleAsync()).Results)!;
        Assert.Equal(entity.ID, aggregate.MotivationTest.ID);
        Assert.False(aggregate.MotivationTest.IsAgree);
        Assert.Equal(0, aggregate.MotivationTest.SummBall);
    }

    [Fact]
    public async Task MotivationTest_Agreement_ValidatesAllQuestionsAndSavesMarks()
    {
        var user = await CreateUserAsync("qa_motivation_yes", "testable_user");
        var seeded = await SeedAccessAsync(code: "7003", username: user.UserName);

        await using (var db = CreateAnketaContext())
        {
            var result = await CreateController(db, user).MotivationTest(new MVC_MotivationModel
            {
                AccessTestID = seeded.AccessId,
                IsAgree = true,
                RecomendPosition = "Руководитель",
                Recomendator = "Комиссия",
                Question_01 = "Да",
                Question_02 = "Подробный ответ 2",
                Question_03 = "Подробный ответ 3",
                Question_04 = "Да",
                Question_05 = "Да",
                Question_06_1 = "Профессиональный рост",
                Question_06_2 = "Карьерный рост",
                Question_07 = "Да",
                Question_08 = new[] { "Надымский регион", "Не имеет значения" },
                Question_09 = "Нет"
            });
            Assert.IsType<RedirectToActionResult>(result);
        }

        await using var verifyDb = CreateAnketaContext();
        var entity = await verifyDb.MotivationTests.SingleAsync();
        Assert.True(entity.IsAgree);
        var raw = JsonSerializer.Deserialize<Motivation_Result>(entity.Results)!;
        Assert.Contains("Профессиональный рост", raw.Question_06);
        Assert.Contains("Карьерный рост", raw.Question_06);
        Assert.Contains("Надымский регион", raw.Question_08);

        var aggregate = JsonSerializer.Deserialize<JSON_ResultModel>(
            (await verifyDb.TestResults.SingleAsync()).Results)!;
        Assert.Equal(6, aggregate.MotivationTest.SummBall);
    }

    [Fact]
    public async Task MotivationTest_RejectsMissingAnswerAndUnknownRegion()
    {
        var user = await CreateUserAsync("qa_motivation_invalid", "testable_user");
        var seeded = await SeedAccessAsync(code: "7004", username: user.UserName);

        await using var db = CreateAnketaContext();
        var controller = CreateController(db, user);
        var result = Assert.IsType<ViewResult>(await controller.MotivationTest(new MVC_MotivationModel
        {
            AccessTestID = seeded.AccessId,
            IsAgree = true,
            RecomendPosition = "Руководитель",
            Recomendator = "Комиссия",
            Question_01 = "Да",
            Question_02 = "Ответ",
            Question_03 = "Ответ",
            Question_04 = "Да",
            Question_05 = "Да",
            Question_06_1 = "Рост",
            Question_06_2 = "Рост",
            Question_07 = "Да",
            Question_08 = new[] { "Марс" },
            Question_09 = "Да"
        }));

        Assert.Equal("MotivationTest", result.ViewName);
        Assert.True(controller.ModelState.ErrorCount > 0);
        Assert.Equal(0, await db.MotivationTests.CountAsync());
    }

    [Fact]
    public async Task MotivationTest_RejectsDuplicateSubmissionAndForeignAccess()
    {
        var user = await CreateUserAsync("qa_motivation_once", "testable_user");
        var first = await SeedAccessAsync(code: "7005", username: user.UserName);
        var second = await SeedAccessAsync(code: "7006");

        await using (var db = CreateAnketaContext())
        {
            var firstResult = await CreateController(db, user).MotivationTest(Refusal(first.AccessId));
            Assert.IsType<RedirectToActionResult>(firstResult);
        }

        await using (var db = CreateAnketaContext())
        {
            var duplicate = await CreateController(db, user).MotivationTest(Refusal(first.AccessId));
            Assert.Equal("MotivationError", Assert.IsType<ViewResult>(duplicate).ViewName);
        }

        await using (var db = CreateAnketaContext())
        {
            var foreign = await CreateController(db, user).MotivationTest(Refusal(second.AccessId));
            Assert.Equal("MotivationError", Assert.IsType<ViewResult>(foreign).ViewName);
        }

        await using var verifyDb = CreateAnketaContext();
        Assert.Equal(1, await verifyDb.MotivationTests.CountAsync());
    }

    private MotivationController CreateController(AnketaContext db, Microsoft.AspNetCore.Identity.IdentityUser user)
    {
        var controller = new MotivationController(db, UserManager);
        SetControllerUser(controller, user);
        return controller;
    }

    private static MVC_MotivationModel Refusal(int accessId) => new()
    {
        AccessTestID = accessId,
        IsAgree = false,
        RecomendPosition = "Руководитель",
        Recomendator = "Комиссия",
        Question_YES_NO = "Отказ"
    };
}
