using System.Text.Json;
using Anketa_4_core.Controllers;
using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Anketa_4_core.Tests;

public sealed class KettelControllerTests : PostgresTestBase
{
    [Fact]
    public async Task Start_Get_RejectsMissingQuestionBank()
    {
        var user = await CreateUserAsync("qa_kettel_empty", "testable_user");
        var seeded = await SeedAccessAsync(code: "8001", username: user.UserName);

        await using var db = CreateAnketaContext();
        var result = Assert.IsType<ViewResult>(await CreateController(db, user).Start(seeded.AccessId));
        Assert.Equal("KettelError", result.ViewName);
    }

    [Fact]
    public async Task Start_Post_CreatesMainAndQuestionGetReturnsFirstQuestion()
    {
        var user = await CreateUserAsync("qa_kettel_start", "testable_user");
        var seeded = await SeedAccessAsync(code: "8002", username: user.UserName);
        int answerId;
        await using (var db = CreateAnketaContext())
        {
            var question = new KT_Question { Number = 1, Text = "Ситуация 1" };
            var answer1 = new KT_Answer { Question = question, AnswerNumber = "1", AnswerText = "Вариант 1" };
            var answer2 = new KT_Answer { Question = question, AnswerNumber = "2", AnswerText = "Вариант 2" };
            db.KT_Questions.Add(question);
            db.KT_Answers.AddRange(answer1, answer2);
            await db.SaveChangesAsync();
            answerId = answer1.ID;
        }

        await using (var db = CreateAnketaContext())
        {
            var controller = CreateController(db, user);
            var result = await controller.Start(new MVC_KettelModel_Main
            {
                AccessID = seeded.AccessId, Years = 35, Gender = "Женщина"
            });
            Assert.Equal("Question", Assert.IsType<RedirectToActionResult>(result).ActionName);
            Assert.Equal(1, await db.KT_Mains.CountAsync());
        }

        await using var readDb = CreateAnketaContext();
        var questionResult = Assert.IsType<ViewResult>(
            await CreateController(readDb, user).Question(seeded.AccessId));
        var model = Assert.IsType<MVC_KettelModel_Question>(questionResult.Model);
        Assert.Equal(1, model.Number);
        Assert.Equal(1, model.Position);
        Assert.Equal(2, model.TotalQuestions);
        Assert.Equal(2, model.Answers.Length);
        Assert.Contains(model.Answers, x => x.ID == answerId);
    }

    [Fact]
    public async Task Start_Post_RejectsInvalidGender()
    {
        var user = await CreateUserAsync("qa_kettel_gender", "testable_user");
        var seeded = await SeedAccessAsync(code: "8003", username: user.UserName);
        await using var db = CreateAnketaContext();

        var controller = CreateController(db, user);
        var result = Assert.IsType<ViewResult>(await controller.Start(new MVC_KettelModel_Main
        {
            AccessID = seeded.AccessId, Years = 35, Gender = "Unknown"
        }));

        Assert.True(controller.ModelState.ErrorCount > 0);
        Assert.Equal(0, await db.KT_Mains.CountAsync());
        Assert.IsType<MVC_KettelModel_Main>(result.Model);
    }

    [Fact]
    public async Task Question_Post_FinalAnswerCalculatesKettelAndFinishShowsSuccess()
    {
        var user = await CreateUserAsync("qa_kettel_complete", "testable_user");
        var seeded = await SeedAccessAsync(code: "8004", username: user.UserName);
        int answerId;
        await using (var db = CreateAnketaContext())
        {
            var question = new KT_Question { Number = 1, Text = "Ситуация 1" };
            var answer = new KT_Answer
            {
                Question = question, AnswerNumber = "1", AnswerText = "Ответ 1"
            };
            var category = new KT_Category { CategoryName = "A" };
            db.KT_Questions.Add(question);
            db.KT_Answers.Add(answer);
            db.KT_Categories.Add(category);
            await db.SaveChangesAsync();
            db.KT_Keys.Add(new KT_KeyForCategory { Category = category, Answer = answer, Score = 2 });
            db.KT_Marks.Add(new KT_Mark
            {
                Category = category, ScoreFrom = 0, ScoreTo = 2,
                Mark = 5, LevelWord = "Средний", WordInReport = "A", LevelWordInReport = "B"
            });
            await db.SaveChangesAsync();
            answerId = answer.ID;
        }

        await using (var db = CreateAnketaContext())
        {
            var start = await CreateController(db, user).Start(new MVC_KettelModel_Main
            {
                AccessID = seeded.AccessId, Years = 35, Gender = "Мужчина"
            });
            Assert.IsType<RedirectToActionResult>(start);
        }

        await using (var db = CreateAnketaContext())
        {
            var result = await CreateController(db, user).Question(new MVC_KettelModel_Submit
            {
                AccessID = seeded.AccessId, SelectedAnswerID = answerId
            });
            Assert.Equal("Finish", Assert.IsType<RedirectToActionResult>(result).ActionName);
        }

        await using var verifyDb = CreateAnketaContext();
        Assert.Equal(1, await verifyDb.KT_TestableAnswers.CountAsync());
        var aggregate = JsonSerializer.Deserialize<JSON_ResultModel>(
            (await verifyDb.TestResults.SingleAsync()).Results)!;
        Assert.True(aggregate.KettelTest.IsCompleted);
        var categoryResult = Assert.Single(aggregate.KettelTest.Results);
        Assert.Equal(2, categoryResult.RawScore);
        Assert.Equal(5, categoryResult.CategoryBall);

        await using var finishDb = CreateAnketaContext();
        Assert.IsType<ViewResult>(await CreateController(finishDb, user).Finish(seeded.AccessId));
    }

    [Fact]
    public async Task Question_Post_MissingNormTableRollsBackLastAnswer()
    {
        var user = await CreateUserAsync("qa_kettel_norm", "testable_user");
        var seeded = await SeedAccessAsync(code: "8005", username: user.UserName);
        int answerId;
        await using (var db = CreateAnketaContext())
        {
            var question = new KT_Question { Number = 1, Text = "Ситуация" };
            var answer = new KT_Answer
            {
                Question = question, AnswerNumber = "1", AnswerText = "Ответ"
            };
            db.KT_Questions.Add(question);
            db.KT_Answers.Add(answer);
            await db.SaveChangesAsync();
            answerId = answer.ID;
        }

        await using (var db = CreateAnketaContext())
        {
            await CreateController(db, user).Start(new MVC_KettelModel_Main
            {
                AccessID = seeded.AccessId, Years = 35, Gender = "Женщина"
            });
        }

        await using (var db = CreateAnketaContext())
        {
            var result = Assert.IsType<ViewResult>(await CreateController(db, user).Question(
                new MVC_KettelModel_Submit { AccessID = seeded.AccessId, SelectedAnswerID = answerId }));
            Assert.Equal("KettelError", result.ViewName);
        }

        await using var verifyDb = CreateAnketaContext();
        Assert.Equal(0, await verifyDb.KT_TestableAnswers.CountAsync());
        Assert.Equal(0, await verifyDb.TestResults.CountAsync());
    }

    private KettelController CreateController(
        AnketaContext db, Microsoft.AspNetCore.Identity.IdentityUser user)
    {
        var controller = new KettelController(db, UserManager);
        SetControllerUser(controller, user);
        return controller;
    }
}
