using System.Security.Claims;
using Anketa_4_core.Controllers;
using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Xunit;

namespace Anketa_4_core.Tests;

public sealed class ReportControllerTests : PostgresTestBase
{
    [Fact]
    public async Task GetReport_Get_RestrictsPersonalUserToOwnTestableAndRequiresPeriod()
    {
        var own = await SeedAccessAsync(code: "12001", username: "qa_report_owner");
        var foreign = await SeedAccessAsync(code: "12002", username: "qa_report_foreign");
        var environment = CreateEnvironment(Path.GetTempPath());

        await using (var db = CreateAnketaContext())
        {
            var controller = CreateController(db, environment, "qa_report_owner", "testable_user");
            Assert.IsType<ForbidResult>(await controller.GetReport(foreign.TestableId));
        }

        await using (var db = CreateAnketaContext())
        {
            var controller = CreateController(db, environment, "qa_report_owner", "testable_user");
            var result = Assert.IsType<ObjectResult>(await controller.GetReport(own.TestableId));
            Assert.Equal(503, result.StatusCode);
        }

        await using (var db = CreateAnketaContext())
        {
            var controller = CreateController(db, environment, "unknown", "testable_user");
            Assert.IsType<NotFoundResult>(await controller.GetReport((int?)null));
        }

        await using (var db = CreateAnketaContext())
        {
            var controller = CreateController(db, environment, "qa_report_owner");
            Assert.IsType<ForbidResult>(await controller.GetReport((int?)null));
        }
    }

    [Fact]
    public async Task GetReport_Post_ValidatesEmptyMissingAndMixedPeriodSelections()
    {
        var first = await SeedAccessAsync(code: "12003", periodName: "Период 1");
        var second = await SeedAccessAsync(code: "12004", periodName: "Период 2");
        var environment = CreateEnvironment(Path.GetTempPath());

        await using var db = CreateAnketaContext();
        var controller = CreateController(db, environment, "moderator", "moderator");

        Assert.IsType<BadRequestObjectResult>(await controller.GetReport(Array.Empty<int>()));
        Assert.IsType<BadRequestObjectResult>(await controller.GetReport(new[] { -1, 0 }));
        Assert.IsType<BadRequestObjectResult>(await controller.GetReport(new[] { first.AccessId, 999999 }));
        Assert.IsType<BadRequestObjectResult>(await controller.GetReport(new[] { first.AccessId, second.AccessId }));
    }

    [Fact]
    public async Task GetReport_Post_DeduplicatesIdsAndRejectsDuplicateOrInvalidAggregateJson()
    {
        var seeded = await SeedAccessAsync(code: "12005");
        var environment = CreateEnvironment(Path.GetTempPath());

        await using (var db = CreateAnketaContext())
        {
            var access = await db.AccessForTestables.FirstAsync(x => x.ID == seeded.AccessId);
            db.TestResults.AddRange(
                new TestResult { Access = access, Results = new JSON_ResultModel().GetJSON() },
                new TestResult { Access = access, Results = new JSON_ResultModel().GetJSON() });
            await db.SaveChangesAsync();
        }

        await using (var db = CreateAnketaContext())
        {
            var result = Assert.IsType<ConflictObjectResult>(
                await CreateController(db, environment, "moderator", "moderator")
                    .GetReport(new[] { seeded.AccessId, seeded.AccessId }));
            Assert.Contains("несколько TestResult", result.Value!.ToString());
        }

        await using (var db = CreateAnketaContext())
        {
            db.TestResults.RemoveRange(db.TestResults);
            var access = await db.AccessForTestables.FirstAsync(x => x.ID == seeded.AccessId);
            db.TestResults.Add(new TestResult { Access = access, Results = "{broken" });
            await db.SaveChangesAsync();
        }

        await using (var db = CreateAnketaContext())
        {
            var result = Assert.IsType<ConflictObjectResult>(
                await CreateController(db, environment, "moderator", "moderator")
                    .GetReport(new[] { seeded.AccessId }));
            Assert.Contains("Некорректный JSON", result.Value!.ToString());
        }
    }

    [Fact]
    public async Task GetReport_Post_ReturnsFileWhenTemplateExists()
    {
        var seeded = await SeedAccessAsync(code: "12006");
        var root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(root, "Reports"));
        var template = Path.Combine(root, "Reports", "report.xlsx");
        using (var workbook = new XLWorkbook())
        {
            workbook.AddWorksheet("360° ");
            workbook.AddWorksheet("Мотивац. опросник");
            workbook.AddWorksheet("Псих обследование");
            workbook.AddWorksheet("Компетенции (1 год)");
            workbook.SaveAs(template);
        }

        try
        {
            await using var db = CreateAnketaContext();
            var result = Assert.IsType<FileContentResult>(
                await CreateController(db, CreateEnvironment(root), "moderator", "moderator")
                    .GetReport(new[] { seeded.AccessId }));
            Assert.Equal(
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                result.ContentType);
            Assert.True(result.FileContents.Length > 0);
            Assert.EndsWith(".xlsx", result.FileDownloadName);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private static ReportController CreateController(
        AnketaContext db, IWebHostEnvironment environment, string username, params string[] roles)
    {
        var claims = new List<Claim> { new(ClaimTypes.Name, username) };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        var controller = new ReportController(db, environment);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"))
            }
        };
        return controller;
    }

    private static IWebHostEnvironment CreateEnvironment(string root) => new TestEnvironment
    {
        ContentRootPath = root,
        WebRootPath = root,
        ContentRootFileProvider = new NullFileProvider(),
        WebRootFileProvider = new NullFileProvider()
    };

    private sealed class TestEnvironment : IWebHostEnvironment
    {
        public string ApplicationName { get; set; } = "Anketa.Tests";
        public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
        public string WebRootPath { get; set; } = "";
        public string EnvironmentName { get; set; } = "Testing";
        public string ContentRootPath { get; set; } = "";
        public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
    }
}
