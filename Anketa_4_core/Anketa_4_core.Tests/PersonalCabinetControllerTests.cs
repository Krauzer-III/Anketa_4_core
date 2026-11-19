using System.Security.Claims;
using Anketa_4_core.Controllers;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Anketa_4_core.Tests;

public sealed class PersonalCabinetControllerTests : PostgresTestBase
{
    [Fact]
    public void Index_ReturnsView()
    {
        using var db = CreateAnketaContext();
        var result = CreateController(db).Index();
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Data_RequiresAuthenticatedUsernameAndReturnsDefaultsForOwner()
    {
        using (var db = CreateAnketaContext())
        {
            var anonymous = CreateController(db);
            Assert.IsType<UnauthorizedResult>(await anonymous.Data());
        }

        await SeedAccessAsync(username: "employee");
        await using var readDb = CreateAnketaContext();
        var controller = CreateController(readDb);
        SetName(controller, "employee");

        var result = Assert.IsType<JsonResult>(await controller.Data());
        var lkData = result.Value!.GetType().GetProperty("lkData")!.GetValue(result.Value);
        var json = Assert.IsType<System.Text.Json.Nodes.JsonObject>(lkData);
        Assert.Equal(8, json["DistanceLearning"]!.AsArray().Count);
        Assert.Equal(3, json["ReportBooks"]!.AsArray().Count);
    }

    [Fact]
    public async Task Data_ReturnsNotFoundWhenUserHasNoCabinet()
    {
        await using var db = CreateAnketaContext();
        var controller = CreateController(db);
        SetName(controller, "missing");

        var result = Assert.IsType<NotFoundObjectResult>(await controller.Data());
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task SaveSection_ValidatesRequestAndSavesOnlySelectedSection()
    {
        await SeedAccessAsync(username: "employee");
        await using var db = CreateAnketaContext();
        var controller = CreateController(db);
        SetName(controller, "employee");

        var saved = Assert.IsType<JsonResult>(await controller.SaveSection(new MVC_LK_SectionUpdate
        {
            Section = "ProfTasks",
            Value = Element("""[{"Number":1,"ProfTaskName":"Задача"}]""")
        }));
        Assert.True((bool)saved.Value!.GetType().GetProperty("saved")!.GetValue(saved.Value)!);

        var invalid = Assert.IsType<BadRequestObjectResult>(await controller.SaveSection(
            new MVC_LK_SectionUpdate { Section = "Unsupported", Value = Element("{}") }));
        Assert.NotNull(invalid.Value);

        var missing = Assert.IsType<BadRequestObjectResult>(
            await controller.SaveSection(null!));
        Assert.NotNull(missing.Value);
    }

    [Fact]
    public async Task SaveSection_RequiresAuthenticatedUsername()
    {
        await using var db = CreateAnketaContext();
        var controller = CreateController(db);

        Assert.IsType<UnauthorizedResult>(await controller.SaveSection(
            new MVC_LK_SectionUpdate { Section = "ProfTasks", Value = Element("[]") }));
    }

    [Fact]
    public async Task Rating_ReturnsNotFoundForUnlinkedUserAndSnapshotForLinkedUser()
    {
        await using (var db = CreateAnketaContext())
        {
            var controller = CreateController(db);
            SetName(controller, "unlinked");
            Assert.IsType<NotFoundObjectResult>(await controller.Rating());
        }

        await SeedAccessAsync(username: "employee");
        await using var readDb = CreateAnketaContext();
        var linked = CreateController(readDb);
        SetName(linked, "employee");

        var result = Assert.IsType<JsonResult>(await linked.Rating());
        Assert.Equal(1, (int)result.Value!.GetType().GetProperty("Participants")!.GetValue(result.Value)!);
    }

    private static PersonalCabinetController CreateController(Anketa_4_core.Data.AnketaContext db) =>
        new(db, NullLogger<PersonalCabinetController>.Instance);

    private static void SetName(Controller controller, string username)
    {
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(
                    new[] { new Claim(ClaimTypes.Name, username) }, "Test"))
            }
        };
    }
}
