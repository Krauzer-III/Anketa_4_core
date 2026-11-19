using System.Text.Json;
using Anketa_4_core.Controllers.Users;
using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Anketa_4_core.Tests;

public sealed class CompetentionControllerTests : PostgresTestBase
{
    [Fact]
    public async Task Index_RejectsUnknownYear()
    {
        var user = await CreateUserAsync("qa_comp_year", "testable_user");
        await SeedAccessAsync(code: "9001", username: user.UserName);
        await using var db = CreateAnketaContext();

        var result = await CreateController(db, user).Index(3);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Question_Get_ReturnsExactlyFourVariants()
    {
        var user = await CreateUserAsync("qa_comp_get", "testable_user");
        var seeded = await SeedAccessAsync(code: "9002", username: user.UserName);
        int questionId;
        await using (var db = CreateAnketaContext())
        {
            var block = new Comp_Block { BlockName = "Блок 1", WordInReport = "A", Year = 1 };
            var question = new Comp_Question
            {
                QuestionNumber = 1, QuestionText = "Ситуация", Block = block
            };
            db.Comp_Blocks.Add(block);
            db.Comp_Questions.Add(question);
            for (var i = 1; i <= 4; i++)
                db.Comp_Answers.Add(new Comp_Answer
                {
                    Question = question, Number = i.ToString(),
                    Text = "Вариант " + i, Correct_Answer = i == 1 ? 1 : 0
                });
            await db.SaveChangesAsync();
            questionId = question.ID;
        }

        await using var readDb = CreateAnketaContext();
        var result = Assert.IsType<ViewResult>(
            await CreateController(readDb, user).Question(seeded.AccessId, 1));
        var model = Assert.IsType<MVC_CompetentionModel_Question>(result.Model);
        Assert.Equal(questionId, model.QuestionID);
        Assert.Equal(4, model.Answers.Length);
        Assert.Equal(1, model.Position);
        Assert.Equal(1, model.TotalQuestions);
    }

    [Fact]
    public async Task Question_Post_SavesFourUniquePrioritiesAndCalculatesYear()
    {
        var user = await CreateUserAsync("qa_comp_complete", "testable_user");
        var seeded = await SeedAccessAsync(code: "9003", username: user.UserName);
        int questionId;
        int[] answerIds;
        await using (var db = CreateAnketaContext())
        {
            var block = new Comp_Block
            {
                BlockName = "Блок 1", WordInReport = "A", Year = 1
            };
            var question = new Comp_Question
            {
                QuestionNumber = 1, QuestionText = "Ситуация 1", Block = block
            };
            db.Comp_Blocks.Add(block);
            db.Comp_Questions.Add(question);
            var answers = Enumerable.Range(1, 4).Select(i => new Comp_Answer
            {
                Question = question, Number = i.ToString(),
                Text = "Вариант " + i, Correct_Answer = i == 1 ? 1 : 0
            }).ToArray();
            db.Comp_Answers.AddRange(answers);
            await db.SaveChangesAsync();
            questionId = question.ID;
            answerIds = answers.Select(x => x.ID).ToArray();
        }

        await using (var db = CreateAnketaContext())
        {
            var controller = CreateController(db, user);
            var result = await controller.Question(new MVC_CompetentionSubmitModel
            {
                AccessID = seeded.AccessId,
                Year = 1,
                QuestionID = questionId,
                Answers = answerIds.Select((id, index) => new MVC_CompetentionModel_Answer
                {
                    ID = id, Priority = index + 1
                }).ToArray()
            });
            Assert.Equal("Finish", Assert.IsType<RedirectToActionResult>(result).ActionName);
        }

        await using var verifyDb = CreateAnketaContext();
        Assert.Equal(4, await verifyDb.Comp_TestableAnswers.CountAsync());
        var aggregate = JsonSerializer.Deserialize<JSON_ResultModel>(
            (await verifyDb.TestResults.SingleAsync()).Results)!;
        var saved = Assert.Single(aggregate.CompetentionAnswers);
        Assert.Equal(4, saved.Answers.Length);
        Assert.Equal(1, Assert.Single(aggregate.CompetentionResult).Year);
        Assert.Null(aggregate.CompetentionResult[0].LevelNumber);
    }

    [Fact]
    public async Task Question_Post_RejectsDuplicateOrOutOfRangePriorities()
    {
        var user = await CreateUserAsync("qa_comp_invalid", "testable_user");
        var seeded = await SeedAccessAsync(code: "9004", username: user.UserName);
        int questionId;
        int[] answerIds;
        await using (var db = CreateAnketaContext())
        {
            var block = new Comp_Block { BlockName = "Блок", WordInReport = "A", Year = 1 };
            var question = new Comp_Question { QuestionNumber = 1, QuestionText = "Q", Block = block };
            db.Comp_Blocks.Add(block);
            db.Comp_Questions.Add(question);
            var answers = Enumerable.Range(1, 4).Select(i => new Comp_Answer
            {
                Question = question, Number = i.ToString(), Text = "A" + i
            }).ToArray();
            db.Comp_Answers.AddRange(answers);
            await db.SaveChangesAsync();
            questionId = question.ID;
            answerIds = answers.Select(x => x.ID).ToArray();
        }

        await using var readDb = CreateAnketaContext();
        var controller = CreateController(readDb, user);
        var result = Assert.IsType<ViewResult>(await controller.Question(new MVC_CompetentionSubmitModel
        {
            AccessID = seeded.AccessId,
            Year = 1,
            QuestionID = questionId,
            Answers = answerIds.Select((id, index) => new MVC_CompetentionModel_Answer
            {
                ID = id, Priority = index == 3 ? 5 : 1
            }).ToArray()
        }));

        Assert.IsType<MVC_CompetentionModel_Question>(result.Model);
        Assert.True(controller.ModelState.ErrorCount > 0);
        Assert.Equal(0, await readDb.Comp_TestableAnswers.CountAsync());
    }

    [Fact]
    public async Task Question_Post_YearOneAndYearTwoRemainIndependent()
    {
        var user = await CreateUserAsync("qa_comp_both_years", "testable_user");
        var seeded = await SeedAccessAsync(code: "9005", username: user.UserName);
        var questionIds = new Dictionary<int, int>();
        var answerIds = new Dictionary<int, int[]>();
        await using (var db = CreateAnketaContext())
        {
            foreach (var year in new[] { 1, 2 })
            {
                var block = new Comp_Block { BlockName = "Блок " + year, WordInReport = year.ToString(), Year = year };
                var question = new Comp_Question
                {
                    QuestionNumber = 1, QuestionText = "Вопрос " + year, Block = block
                };
                db.Comp_Blocks.Add(block);
                db.Comp_Questions.Add(question);
                var answers = Enumerable.Range(1, 4).Select(i => new Comp_Answer
                {
                    Question = question, Number = i.ToString(), Text = "A" + i,
                    Correct_Answer = i == 1 ? 1 : 0
                }).ToArray();
                db.Comp_Answers.AddRange(answers);
                await db.SaveChangesAsync();
                questionIds[year] = question.ID;
                answerIds[year] = answers.Select(x => x.ID).ToArray();
            }
        }

        foreach (var year in new[] { 1, 2 })
        {
            await using var db = CreateAnketaContext();
            var result = await CreateController(db, user).Question(new MVC_CompetentionSubmitModel
            {
                AccessID = seeded.AccessId, Year = year, QuestionID = questionIds[year],
                Answers = answerIds[year].Select((id, index) => new MVC_CompetentionModel_Answer
                {
                    ID = id, Priority = index + 1
                }).ToArray()
            });
            Assert.Equal("Finish", Assert.IsType<RedirectToActionResult>(result).ActionName);
        }

        await using var verifyDb = CreateAnketaContext();
        var aggregate = JsonSerializer.Deserialize<JSON_ResultModel>(
            (await verifyDb.TestResults.SingleAsync()).Results)!;
        Assert.Equal(new[] { 1, 2 }, aggregate.CompetentionAnswers.Select(x => x.Year).OrderBy(x => x));
        Assert.Equal(new[] { 1, 2 }, aggregate.CompetentionResult.Select(x => x.Year).OrderBy(x => x));
    }

    private CompetentionController CreateController(
        AnketaContext db, Microsoft.AspNetCore.Identity.IdentityUser user)
    {
        var controller = new CompetentionController(db, UserManager);
        SetControllerUser(controller, user);
        return controller;
    }
}
