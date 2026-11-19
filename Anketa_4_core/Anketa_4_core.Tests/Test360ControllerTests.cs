using System.Text.Json;
using Anketa_4_core.Controllers;
using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Anketa_4_core.Tests;

public sealed class Test360ControllerTests : PostgresTestBase
{
    [Fact]
    public async Task StartTest_Get_PersonalUserOpensOwnSelfAssessment()
    {
        var user = await CreateUserAsync("qa_360_self", "testable_user");
        var seeded = await SeedAccessAsync(code: "6001", username: user.UserName);
        await using (var db = CreateAnketaContext())
        {
            db.RespondentRoles.Add(new RespondentRole { Name = "Самооценка", isSelfMark = true });
            await db.SaveChangesAsync();
        }

        await using var readDb = CreateAnketaContext();
        var controller = CreateController(readDb, user);
        var result = Assert.IsType<ViewResult>(await controller.StartTest());

        Assert.Equal("TestForm", result.ViewName);
        var model = Assert.IsType<MVC_Test360Model>(result.Model);
        Assert.Equal(seeded.AccessId, model.AccesTestID);
        Assert.True(model.isSelf);
    }

    [Fact]
    public async Task StartTest_Get_NonPersonalUserGetsCodeEntryForm()
    {
        var user = await CreateUserAsync("qa_360_filial", "filial_user");
        await using var db = CreateAnketaContext();
        var controller = CreateController(db, user);

        var result = Assert.IsType<ViewResult>(await controller.StartTest());
        Assert.IsType<MVC_Test360_StartTest_Model>(result.Model);
    }

    [Fact]
    public async Task StartTest_Post_RequiresSameFilialAndRejectsUnknownRole()
    {
        var user = await CreateUserAsync("qa_360_branch", "filial_user");
        var seeded = await SeedAccessAsync(code: "6002");
        await using (var db = CreateAnketaContext())
        {
            var filial = await db.Filials.FindAsync(seeded.FilialId);
            db.UserFilials.Add(new UserFilial { UserName = user.UserName!, Filial = filial! });
            db.RespondentRoles.Add(new RespondentRole { Name = "Эксперт", isSelfMark = false });
            await db.SaveChangesAsync();
        }

        await using (var db = CreateAnketaContext())
        {
            var controller = CreateController(db, user);
            var badRole = Assert.IsType<ViewResult>(await controller.StartTest(
                new MVC_Test360_StartTest_Model { Code = "6002", RoleID = 999999 }));
            Assert.IsType<MVC_Test360_StartTest_Model>(badRole.Model);
            Assert.True(controller.ModelState.ErrorCount > 0);
        }

        var other = await SeedAccessAsync(code: "6003", filialName: "Другой филиал");
        await using var verifyDb = CreateAnketaContext();
        var wrongBranch = Assert.IsType<ViewResult>(await CreateController(verifyDb, user).StartTest(
            new MVC_Test360_StartTest_Model { Code = "6003", RoleID = 1 }));
        Assert.IsType<MVC_Test360_StartTest_Model>(wrongBranch.Model);
        Assert.True(other.TestableId > 0);
    }

    [Fact]
    public async Task SubmitTest_PersonalUserCannotSubmitRespondentRole()
    {
        var user = await CreateUserAsync("qa_360_wrong_role", "testable_user");
        var seeded = await SeedAccessAsync(code: "6004", username: user.UserName);
        int roleId;
        await using (var db = CreateAnketaContext())
        {
            var role = new RespondentRole { Name = "Эксперт", isSelfMark = false };
            db.RespondentRoles.Add(role);
            await db.SaveChangesAsync();
            roleId = role.ID;
        }

        await using var readDb = CreateAnketaContext();
        var result = await CreateController(readDb, user).SubmitTest(new MVC_Test360Model
        {
            AccesTestID = seeded.AccessId, RespondentRoleID = roleId, q01 = 3
        });

        Assert.Equal("TestError", Assert.IsType<ViewResult>(result).ViewName);
        Assert.Equal(0, await readDb.Test360.CountAsync());
    }

    [Fact]
    public async Task SubmitTest_SavesSelfAssessmentAndRejectsSecondSubmission()
    {
        var user = await CreateUserAsync("qa_360_once", "testable_user");
        var seeded = await SeedAccessAsync(code: "6005", username: user.UserName);
        int roleId;
        await using (var db = CreateAnketaContext())
        {
            var role = new RespondentRole { Name = "Самооценка", isSelfMark = true };
            db.RespondentRoles.Add(role);
            await db.SaveChangesAsync();
            roleId = role.ID;
        }

        await using (var db = CreateAnketaContext())
        {
            var result = await CreateController(db, user).SubmitTest(new MVC_Test360Model
            {
                AccesTestID = seeded.AccessId, RespondentRoleID = roleId,
                q01 = 5, q02 = 4, q03 = 3, q04 = 2, q05 = 1
            });
            Assert.Equal("TestCompleted", Assert.IsType<ViewResult>(result).ViewName);
        }

        await using (var db = CreateAnketaContext())
        {
            var duplicate = await CreateController(db, user).SubmitTest(new MVC_Test360Model
            {
                AccesTestID = seeded.AccessId, RespondentRoleID = roleId, q01 = 5
            });
            Assert.Equal("TestError", Assert.IsType<ViewResult>(duplicate).ViewName);
        }

        await using var verifyDb = CreateAnketaContext();
        Assert.Equal(1, await verifyDb.Test360.CountAsync());
        var aggregate = JsonSerializer.Deserialize<JSON_ResultModel>(
            (await verifyDb.TestResults.SingleAsync()).Results)!;
        Assert.Single(aggregate.Tests360);
        Assert.True(aggregate.Tests360[0].IsSelfMark);
        Assert.Equal(5, aggregate.Test360ForReportTestable.q01);
    }

    [Fact]
    public async Task SubmitTest_CurrentImplementationAcceptsOutOfRangeMarks()
    {
        var user = await CreateUserAsync("qa_360_range", "testable_user");
        var seeded = await SeedAccessAsync(code: "6006", username: user.UserName);
        int roleId;
        await using (var db = CreateAnketaContext())
        {
            var role = new RespondentRole { Name = "Самооценка", isSelfMark = true };
            db.RespondentRoles.Add(role);
            await db.SaveChangesAsync();
            roleId = role.ID;
        }

        await using (var db = CreateAnketaContext())
        {
            var result = await CreateController(db, user).SubmitTest(new MVC_Test360Model
            {
                AccesTestID = seeded.AccessId, RespondentRoleID = roleId,
                q01 = -100, q02 = 999
            });
            Assert.Equal("TestCompleted", Assert.IsType<ViewResult>(result).ViewName);
        }

        await using var verifyDb = CreateAnketaContext();
        var raw = await verifyDb.Test360.SingleAsync();
        var answers = JsonSerializer.Deserialize<Test360_Result>(raw.results)!;
        Assert.Equal(-100, answers.q01);
        Assert.Equal(999, answers.q02);
    }

    private Test360Controller CreateController(AnketaContext db, Microsoft.AspNetCore.Identity.IdentityUser user)
    {
        var controller = new Test360Controller(db, UserManager);
        SetControllerUser(controller, user);
        return controller;
    }
}
