using Anketa_4_core.Controllers;
using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Anketa_4_core.Tests;

public sealed class TestableControllerTests : PostgresTestBase
{
    [Fact]
    public async Task List_FiltersByCodeAndReturnsPartialRows()
    {
        await SeedAccessAsync(code: "11001");
        await SeedAccessAsync(code: "11002");

        await using var db = CreateAnketaContext();
        var controller = CreateController(db);
        var result = Assert.IsType<PartialViewResult>(await controller.List(skip: 0, take: 1, q: "1100"));

        Assert.Equal("_TestableRows", result.ViewName);
        var row = Assert.Single(Assert.IsAssignableFrom<IEnumerable<MVC_TestableListItem>>(result.Model));
        Assert.Equal("11001", row.Code);
    }

    [Fact]
    public async Task NextCode_ReturnsOneMoreThanMaximumNumericCode()
    {
        await SeedAccessAsync(code: "0009");
        await SeedAccessAsync(code: "101");
        await SeedAccessAsync(code: "ABC");

        await using var db = CreateAnketaContext();
        Assert.Equal("102", await CreateController(db).NextCode());
    }

    [Fact]
    public async Task Import_RejectsMissingFileAndInvalidRowsWithoutPartialSave()
    {
        await using (var db = CreateAnketaContext())
        {
            var controller = CreateController(db);
            var missing = Assert.IsType<ViewResult>(await controller.Import(new MVC_TestableImportVM()));
            Assert.True(controller.ModelState.ErrorCount > 0);
            Assert.IsType<MVC_TestableImportVM>(missing.Model);
        }

        var filial = await SeedAccessAsync(code: "11003");
        await using (var db = CreateAnketaContext())
        {
            var csv = "code,filial,rezerv_level,testperiod,username\n" +
                      "11004,Нет такого филиала,0,,\n" +
                      ",Филиал 1,0,,";
            var file = MakeFile(csv);
            var controller = CreateController(db);
            var result = Assert.IsType<ViewResult>(await controller.Import(new MVC_TestableImportVM
            {
                File = file, DefaultYear = 2026
            }));
            Assert.True(controller.ModelState.ErrorCount > 0);
            Assert.Equal(1, await db.Testables.CountAsync());
            Assert.True(filial.TestableId > 0);
            Assert.IsType<MVC_TestableImportVM>(result.Model);
        }
    }

    [Fact]
    public async Task Import_ValidCsvCreatesTestableUserAndPeriodAssignment()
    {
        await CreateUserAsync("qa_import_user", "testable_user");
        var seeded = await SeedAccessAsync(code: "11005", periodName: "Период импорта");
        await using (var db = CreateAnketaContext())
        {
            db.Testables.Remove(await db.Testables.FindAsync(seeded.TestableId) ?? throw new Exception());
            await db.SaveChangesAsync();
        }

        var csv = "code,filial,rezerv_level,testperiod,username\n" +
                  "11006,Филиал 1,0,Период импорта,qa_import_user";
        await using (var db = CreateAnketaContext())
        {
            var controller = CreateController(db);
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), new TestTempDataProvider());
            var result = await controller.Import(new MVC_TestableImportVM
            {
                File = MakeFile(csv), DefaultYear = 2027
            });
            Assert.Equal("Index", Assert.IsType<RedirectToActionResult>(result).ActionName);
        }

        await using var verifyDb = CreateAnketaContext();
        var testable = await verifyDb.Testables.SingleAsync(x => x.Code == "11006");
        Assert.Equal(2027, testable.YearTraining);
        Assert.Equal(1, await verifyDb.TestableUsers.CountAsync(x => x.testable.ID == testable.ID));
        Assert.Equal(1, await verifyDb.AccessForTestables.CountAsync(x => x.Testable.ID == testable.ID));
    }

    [Fact]
    public async Task Edit_GetAndPost_UpdateTestableAndBindAccount()
    {
        var user = await CreateUserAsync("qa_testable_edit", "testable_user");
        var seeded = await SeedAccessAsync(code: "11007", username: user.UserName);
        var nextPeriod = await SeedAccessAsync(code: "11008", periodName: "Новый период");

        await using (var db = CreateAnketaContext())
        {
            var controller = CreateController(db);
            var get = Assert.IsType<ViewResult>(await controller.Edit(seeded.TestableId));
            var model = Assert.IsType<MVC_TestableEditFull>(get.Model);
            Assert.Equal("11007", model.Code);
            Assert.Equal(seeded.PeriodId, model.SelectedTestPeriodId);

            var mismatch = await controller.Edit(seeded.TestableId + 1, new MVC_TestableEditFull
            {
                ID = seeded.TestableId, Code = "11007", Year = 2
            });
            Assert.IsType<BadRequestResult>(mismatch);
        }

        await using (var db = CreateAnketaContext())
        {
            var result = await CreateController(db).Edit(seeded.TestableId, new MVC_TestableEditFull
            {
                ID = seeded.TestableId,
                oldCode = "11007",
                Code = "11007-EDIT",
                Year = 2,
                Filial = seeded.FilialId,
                UserName = user.UserName,
                SelectedTestPeriodId = nextPeriod.PeriodId
            });
            Assert.Equal("Details", Assert.IsType<RedirectToActionResult>(result).ActionName);
        }

        await using var verifyDb = CreateAnketaContext();
        var saved = await verifyDb.Testables.SingleAsync(x => x.ID == seeded.TestableId);
        Assert.Equal("11007-EDIT", saved.Code);
        Assert.Equal(2, saved.YearTraining);
        Assert.Equal(2, await verifyDb.AccessForTestables.CountAsync(x => x.Testable.ID == seeded.TestableId));
    }

    [Fact]
    public async Task DetailsAndDeletePreview_ExposeBoundPeriodsAndResults()
    {
        var seeded = await SeedAccessAsync(code: "11009", username: "qa_details", lkData: """{"ProfTasks":[]}""");
        await using var db = CreateAnketaContext();
        var controller = CreateController(db);

        var details = Assert.IsType<ViewResult>(await controller.Details(seeded.TestableId));
        var model = Assert.IsType<MVC_TestableDetailsFull>(details.Model);
        Assert.Equal("11009", model.Code);
        Assert.Equal("qa_details", model.UserName);
        Assert.Single(model.BoundTestPeriods);
        Assert.Single(model.TestResults);

        var preview = Assert.IsType<ViewResult>(await controller.Delete(seeded.TestableId));
        var deleteModel = Assert.IsType<MVC_TestableDeletePreview>(preview.Model);
        Assert.Equal(1, deleteModel.AccessLinksCount);
        Assert.Equal("11009", deleteModel.Code);
    }

    [Fact]
    public async Task LKDataAndSaveLKSection_ReturnJsonAndValidatePayload()
    {
        var seeded = await SeedAccessAsync(code: "11010", username: "qa_lk");
        await using var db = CreateAnketaContext();
        var controller = CreateController(db);

        var data = Assert.IsType<JsonResult>(await controller.LKData(seeded.TestableId));
        Assert.NotNull(data.Value);

        var saved = Assert.IsType<JsonResult>(await controller.SaveLKSection(
            seeded.TestableId,
            new MVC_LK_SectionUpdate { Section = "ProfTasks", Value = Element("""[{"Number":1}]""") }));
        Assert.True((bool)saved.Value!.GetType().GetProperty("saved")!.GetValue(saved.Value)!);

        Assert.IsType<BadRequestObjectResult>(await controller.SaveLKSection(
            seeded.TestableId,
            new MVC_LK_SectionUpdate { Section = "NoSuchSection", Value = Element("{}") }));
        Assert.IsType<NotFoundResult>(await controller.LKData(999999));
        Assert.IsType<NotFoundResult>(await controller.SaveLKSection(
            999999, new MVC_LK_SectionUpdate { Section = "ProfTasks", Value = Element("[]") }));
    }

    [Fact]
    public async Task CreateAndReportActions_AreCurrentlyStubs()
    {
        var seeded = await SeedAccessAsync(code: "11011");
        await using var db = CreateAnketaContext();
        var controller = CreateController(db);

        var createGet = Assert.IsType<ViewResult>(await controller.Create());
        Assert.Equal("Create", createGet.ViewName);
        var createPost = Assert.IsType<RedirectToActionResult>(
            await controller.Create(new MVC_TestableEditFull { Code = "11012" }));
        Assert.Equal("Details", createPost.ActionName);
        Assert.False(await db.Testables.AnyAsync(x => x.Code == "11012"));

        Assert.IsType<NoContentResult>(controller.GenerateReportPdf(seeded.TestableId));
        Assert.IsType<NoContentResult>(controller.GenerateReportXlsx(seeded.TestableId));
        Assert.IsType<NoContentResult>(controller.SaveLKData(seeded.TestableId, "{}"));
    }

    [Fact]
    public async Task DeleteConfirmed_RemovesTestableAndItsAccessLinks()
    {
        var seeded = await SeedAccessAsync(code: "11013", username: "qa_delete");
        await using (var db = CreateAnketaContext())
        {
            var result = await CreateController(db).DeleteConfirmed(seeded.TestableId);
            Assert.Equal("Index", Assert.IsType<RedirectToActionResult>(result).ActionName);
        }

        await using var verifyDb = CreateAnketaContext();
        Assert.False(await verifyDb.Testables.AnyAsync(x => x.ID == seeded.TestableId));
        Assert.False(await verifyDb.AccessForTestables.AnyAsync(x => x.ID == seeded.AccessId));
        Assert.False(await verifyDb.TestableUsers.AnyAsync(x => x.UserName == "qa_delete"));
    }

    private TestableController CreateController(AnketaContext db)
    {
        var controller = new TestableController(db, UserManager, RoleManager, IdentityDb);
        return controller;
    }

    private static FormFile MakeFile(string content)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(content);
        return new FormFile(new MemoryStream(bytes), 0, bytes.Length, "File", "import.csv");
    }

    private sealed class TestTempDataProvider : ITempDataProvider
    {
        public IDictionary<string, object> LoadTempData(HttpContext context) =>
            new Dictionary<string, object>();

        public void SaveTempData(HttpContext context, IDictionary<string, object> values) { }
    }
}
