using Anketa_4_core.Controllers;
using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Anketa_4_core.Tests;

public sealed class RegistrationControllerTests : PostgresTestBase
{
    [Fact]
    public async Task Register_AdminBatch_CreatesUsersAndAssignsRole()
    {
        await EnsureRoleAsync("admin");
        var controller = CreateController();

        var result = await controller.Register(new RegistrationPageModel
        {
            Role = " admin ",
            Delimiter = ";",
            TextData = "login;password\nqa_admin;Password123!"
        });

        Assert.IsType<RedirectToActionResult>(result);
        var user = await UserManager.FindByNameAsync("qa_admin");
        Assert.NotNull(user);
        Assert.True(await UserManager.IsInRoleAsync(user!, "admin"));
    }

    [Theory]
    [InlineData("same;Password123!\nSAME;Password123!", "повторяется")]
    [InlineData("login;password;extra\nuser;Password123!;x", "Ожидается 2 полей")]
    [InlineData("user;", "Пароль не может быть пустым")]
    public async Task Register_RejectsInvalidRowsWithoutCreatingAnyUsers(
        string text, string expectedMessage)
    {
        await EnsureRoleAsync("admin");
        var controller = CreateController();

        var result = await controller.Register(new RegistrationPageModel
        {
            Role = "admin",
            Delimiter = ";",
            TextData = text
        });

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<RegistrationPageModel>(view.Model);
        Assert.Contains(model.Errors, x => x.Message.Contains(expectedMessage, StringComparison.OrdinalIgnoreCase));
        Assert.Equal(0, await IdentityDb.Users.CountAsync());
    }

    [Fact]
    public async Task Register_RejectsTwoSourcesInvalidRoleAndInvalidDelimiter()
    {
        await EnsureRoleAsync("admin");
        var controller = CreateController();
        var file = new FormFile(new MemoryStream(new byte[] { 1 }), 0, 1, "CsvFile", "users.csv");

        var both = Assert.IsType<ViewResult>(await controller.Register(new RegistrationPageModel
        {
            Role = "admin", Delimiter = ";", TextData = "user;Password123!", CsvFile = file
        }));
        Assert.Contains(((RegistrationPageModel)both.Model!).Errors,
            x => x.Message.Contains("либо текстовые данные", StringComparison.OrdinalIgnoreCase));

        var badRole = Assert.IsType<ViewResult>(await controller.Register(new RegistrationPageModel
        {
            Role = "root", Delimiter = ";", TextData = "user;Password123!"
        }));
        Assert.Contains(((RegistrationPageModel)badRole.Model!).Errors,
            x => x.Message.Contains("Недопустимая роль"));

        var badDelimiter = Assert.IsType<ViewResult>(await controller.Register(new RegistrationPageModel
        {
            Role = "admin", Delimiter = ";;", TextData = "user;Password123!"
        }));
        Assert.Contains(((RegistrationPageModel)badDelimiter.Model!).Errors,
            x => x.Message.Contains("ровно один символ"));
    }

    [Fact]
    public async Task Register_RejectsOversizedCsvBeforeReadingIt()
    {
        await EnsureRoleAsync("admin");
        var stream = new MemoryStream(new byte[] { 1 });
        var oversized = new FormFile(stream, 0, 10 * 1024 * 1024 + 1, "CsvFile", "large.csv");
        var result = await CreateController().Register(new RegistrationPageModel
        {
            Role = "admin", Delimiter = ";", CsvFile = oversized
        });

        var model = Assert.IsType<RegistrationPageModel>(Assert.IsType<ViewResult>(result).Model);
        Assert.Contains(model.Errors, x => x.Message.Contains("слишком большой"));
        Assert.Equal(0, await IdentityDb.Users.CountAsync());
    }

    [Fact]
    public async Task Register_ExistingTestableMode_CreatesOnlyAccountBinding()
    {
        await EnsureRoleAsync("testable_user");
        var seeded = await SeedAccessAsync(code: "4001");
        var result = await CreateController().Register(new RegistrationPageModel
        {
            Role = "testable_user",
            TestableMode = "existing",
            Delimiter = ";",
            TextData = "login;password;code\nqa_testable;Password123!;4001"
        });

        Assert.IsType<RedirectToActionResult>(result);
        var user = await UserManager.FindByNameAsync("qa_testable");
        Assert.NotNull(user);
        Assert.True(await UserManager.IsInRoleAsync(user!, "testable_user"));

        await using var db = CreateAnketaContext();
        var link = await db.TestableUsers.Include(x => x.testable)
            .SingleAsync(x => x.UserName == "qa_testable");
        Assert.Equal(seeded.TestableId, link.testable.ID);
        Assert.Equal("{}", link.LK_Data);
    }

    [Fact]
    public async Task Register_NewTestableMode_CreatesTestableAccountAndPeriodLink()
    {
        await EnsureRoleAsync("testable_user");
        int filialId, periodId, reservId;
        await using (var db = CreateAnketaContext())
        {
            var filial = new Filial { FilialName = "Филиал новый" };
            var period = new TestPeriod { GroupName = "Период новый" };
            var reserv = new ReservLevel { ReservLevelName = "Резерв новый" };
            db.AddRange(filial, period, reserv);
            await db.SaveChangesAsync();
            filialId = filial.ID;
            periodId = period.ID;
            reservId = reserv.ID;
        }

        var result = await CreateController().Register(new RegistrationPageModel
        {
            Role = "testable_user",
            TestableMode = "new",
            Delimiter = ";",
            TextData = $"login;password;code;filial;reserv;period\nqa_new;Password123!;5001;Филиал новый;{reservId};{periodId}"
        });

        Assert.IsType<RedirectToActionResult>(result);
        await using var verifyDb = CreateAnketaContext();
        var testable = await verifyDb.Testables.Include(x => x.filial)
            .SingleAsync(x => x.Code == "5001");
        Assert.Equal(filialId, testable.filial.ID);
        Assert.Equal(1, await verifyDb.TestableUsers.CountAsync(x => x.testable.ID == testable.ID));
        Assert.Equal(1, await verifyDb.AccessForTestables.CountAsync(x => x.Testable.ID == testable.ID));
    }

    [Fact]
    public async Task Register_FilialRole_RejectsUnknownFilialAndCreatesValidLink()
    {
        await EnsureRoleAsync("filial_user");
        var invalid = await CreateController().Register(new RegistrationPageModel
        {
            Role = "filial_user", Delimiter = ";",
            TextData = "login;password;filial\nwrong_user;Password123!;Нет такого филиала"
        });
        var invalidModel = Assert.IsType<RegistrationPageModel>(Assert.IsType<ViewResult>(invalid).Model);
        Assert.Contains(invalidModel.Errors, x => x.Message.Contains("не найден"));
        Assert.Null(await UserManager.FindByNameAsync("wrong_user"));

        await using (var db = CreateAnketaContext())
        {
            db.Filials.Add(new Filial { FilialName = "Филиал А" });
            await db.SaveChangesAsync();
        }

        var valid = await CreateController().Register(new RegistrationPageModel
        {
            Role = "filial_user", Delimiter = ";",
            TextData = "login;password;filial\nqa_filial;Password123!;Филиал А"
        });
        Assert.IsType<RedirectToActionResult>(valid);

        await using var verifyDb = CreateAnketaContext();
        var binding = await verifyDb.UserFilials.Include(x => x.Filial)
            .SingleAsync(x => x.UserName == "qa_filial");
        Assert.Equal("Филиал А", binding.Filial.FilialName);
    }

    [Fact]
    public async Task Register_CsvUpload_ReadsUtf8BomAndHeader()
    {
        await EnsureRoleAsync("admin");
        var bytes = System.Text.Encoding.UTF8.GetPreamble()
            .Concat(System.Text.Encoding.UTF8.GetBytes("login;password\nqa_csv;Password123!"))
            .ToArray();
        var file = new FormFile(new MemoryStream(bytes), 0, bytes.Length, "CsvFile", "users.csv");

        var result = await CreateController().Register(new RegistrationPageModel
        {
            Role = "admin", Delimiter = ";", CsvFile = file
        });

        Assert.IsType<RedirectToActionResult>(result);
        Assert.NotNull(await UserManager.FindByNameAsync("qa_csv"));
    }

    private async Task EnsureRoleAsync(string role)
    {
        if (await RoleManager.FindByNameAsync(role) == null)
            Assert.True((await RoleManager.CreateAsync(new IdentityRole(role))).Succeeded);
    }

    private RegistrationController CreateController()
    {
        var anketaDb = CreateAnketaContext();
        var controller = new RegistrationController(
            IdentityDb, anketaDb, UserManager, RoleManager,
            NullLogger<RegistrationController>.Instance);
        var http = new DefaultHttpContext();
        controller.ControllerContext = new ControllerContext { HttpContext = http };
        controller.TempData = new TempDataDictionary(http, new TestTempDataProvider());
        return controller;
    }

    private sealed class TestTempDataProvider : ITempDataProvider
    {
        public IDictionary<string, object> LoadTempData(HttpContext context) =>
            new Dictionary<string, object>();

        public void SaveTempData(HttpContext context, IDictionary<string, object> values) { }
    }
}
