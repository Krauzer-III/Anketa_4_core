using Anketa_4_core.Controllers;
using Anketa_4_core.Controllers.Moderator;
using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Anketa_4_core.Models.AnketaModels.MVC_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Anketa_4_core.Tests;

public sealed class ModeratorControllerTests : PostgresTestBase
{
    [Fact]
    public async Task Period_IndexAndDetails_ReturnCountsAndBoundTestables()
    {
        var first = await SeedAccessAsync(code: "10001", periodName: "Период B");
        await SeedAccessAsync(code: "10002", periodName: "Период A");

        await using var db = CreateAnketaContext();
        var controller = new PeriodController(db);
        var index = Assert.IsType<ViewResult>(await controller.Index());
        var items = Assert.IsType<List<MVC_TestPeriodListItem>>(index.Model);
        Assert.Equal(new[] { "Период A", "Период B" }, items.Select(x => x.PeriodName));
        Assert.All(items, x => Assert.Equal(1, x.CountTestables));

        var details = Assert.IsType<ViewResult>(await controller.Details(first.PeriodId));
        var model = Assert.IsType<MVC_TestPeriodDetails>(details.Model);
        Assert.Equal("Период B", model.PeriodName);
        Assert.Single(model.BoundTestables);
        Assert.Equal("10001", model.BoundTestables[0].Code);
    }

    [Fact]
    public async Task Period_Create_RejectsDuplicateNameAndCreatesNewLinksOnce()
    {
        var existing = await SeedAccessAsync(code: "10003", periodName: "Существующий");
        var additional = await SeedAccessAsync(code: "10004", periodName: "Другой");

        await using (var db = CreateAnketaContext())
        {
            var controller = new PeriodController(db);
            var duplicate = Assert.IsType<ViewResult>(await controller.Create(
                new MVC_TestPeriodCreate { PeriodName = "Существующий" }));
            Assert.True(controller.ModelState.ErrorCount > 0);
            Assert.IsType<MVC_TestPeriodCreate>(duplicate.Model);
        }

        await using (var db = CreateAnketaContext())
        {
            var controller = new PeriodController(db);
            var result = await controller.Create(new MVC_TestPeriodCreate
            {
                PeriodName = "Новый период",
                SelectedTestableIds = new List<int>
                {
                    existing.TestableId, existing.TestableId, additional.TestableId, 999999
                }
            });
            Assert.Equal("Details", Assert.IsType<RedirectToActionResult>(result).ActionName);
        }

        await using var verifyDb = CreateAnketaContext();
        var period = await verifyDb.TestPeriods.SingleAsync(x => x.GroupName == "Новый период");
        Assert.Equal(2, await verifyDb.AccessForTestables.CountAsync(x => x.TestPeriod.ID == period.ID));
    }

    [Fact]
    public async Task Period_Edit_UpdatesActivityAndAddsOnlyNewTestables()
    {
        var first = await SeedAccessAsync(code: "10005", periodName: "Редактируемый");
        var second = await SeedAccessAsync(code: "10006", periodName: "В другом периоде");

        await using (var db = CreateAnketaContext())
        {
            var controller = new PeriodController(db);
            var result = await controller.Edit(new MVC_TestPeriodEdit
            {
                Id = first.PeriodId,
                PeriodName = "Обновлённый",
                BoundTestables = new List<MVC_TestPeriodBoundTestable>
                {
                    new() { TestableId = first.TestableId, IsActive = false }
                },
                AddTestableIds = new List<int> { second.TestableId, second.TestableId, 999999 }
            }, null);
            Assert.Equal("Details", Assert.IsType<RedirectToActionResult>(result).ActionName);
        }

        await using var verifyDb = CreateAnketaContext();
        var period = await verifyDb.TestPeriods.SingleAsync(x => x.ID == first.PeriodId);
        Assert.Equal("Обновлённый", period.GroupName);
        var links = await verifyDb.AccessForTestables.Include(x => x.Testable)
            .Where(x => x.TestPeriod.ID == first.PeriodId).OrderBy(x => x.Testable.ID).ToArrayAsync();
        Assert.Equal(2, links.Length);
        Assert.False(links.Single(x => x.Testable.ID == first.TestableId).isActive);
        Assert.True(links.Single(x => x.Testable.ID == second.TestableId).isActive);
    }

    [Fact]
    public async Task Period_Edit_DuplicateBoundIdsCurrentlyThrow()
    {
        var seeded = await SeedAccessAsync(code: "10007", periodName: "Дубликаты");
        await using var db = CreateAnketaContext();
        var controller = new PeriodController(db);

        await Assert.ThrowsAsync<ArgumentException>(() => controller.Edit(new MVC_TestPeriodEdit
        {
            Id = seeded.PeriodId,
            PeriodName = "Дубликаты",
            BoundTestables = new List<MVC_TestPeriodBoundTestable>
            {
                new() { TestableId = seeded.TestableId, IsActive = true },
                new() { TestableId = seeded.TestableId, IsActive = false }
            }
        }, null));
    }

    [Fact]
    public async Task News_CreateAndEdit_ValidateRequiredTextAndPersistChanges()
    {
        await using (var db = CreateAnketaContext())
        {
            var controller = new NewsAdminController(db);
            var invalid = Assert.IsType<ViewResult>(await controller.Create(new News
            {
                Header = "   ", Body = "   "
            }));
            Assert.True(controller.ModelState.ErrorCount >= 2);
            Assert.IsType<News>(invalid.Model);
        }

        int newsId;
        await using (var db = CreateAnketaContext())
        {
            var controller = new NewsAdminController(db);
            var created = await controller.Create(new News
            {
                Header = "Новость", Body = "Текст"
            });
            Assert.Equal("Index", Assert.IsType<RedirectToActionResult>(created).ActionName);
            newsId = await db.News.Select(x => x.ID).SingleAsync();
        }

        await using (var db = CreateAnketaContext())
        {
            var controller = new NewsAdminController(db);
            var mismatch = await controller.Edit(newsId + 1, new News
            {
                ID = newsId, Header = "Новый заголовок", Body = "Новый текст"
            });
            Assert.IsType<BadRequestResult>(mismatch);
        }

        await using (var db = CreateAnketaContext())
        {
            var controller = new NewsAdminController(db);
            var edited = await controller.Edit(newsId, new News
            {
                ID = newsId, Header = "Новый заголовок", Body = "Новый текст"
            });
            Assert.Equal("Index", Assert.IsType<RedirectToActionResult>(edited).ActionName);
        }

        await using var verifyDb = CreateAnketaContext();
        var saved = await verifyDb.News.SingleAsync();
        Assert.Equal("Новый заголовок", saved.Header);
        Assert.Equal("Новый текст", saved.Body);
    }

    [Fact]
    public async Task News_DeleteAndIndex_HandleMissingRowsAndSortByDate()
    {
        int id;
        await using (var db = CreateAnketaContext())
        {
            db.News.AddRange(
                new News { Header = "Старая", Body = "Текст", DateNews = DateTime.UtcNow.AddDays(-1) },
                new News { Header = "Новая", Body = "Текст", DateNews = DateTime.UtcNow });
            await db.SaveChangesAsync();
            id = await db.News.Where(x => x.Header == "Старая").Select(x => x.ID).SingleAsync();
        }

        await using (var db = CreateAnketaContext())
        {
            var controller = new NewsAdminController(db);
            var index = Assert.IsType<ViewResult>(await controller.Index());
            var items = Assert.IsAssignableFrom<IEnumerable<News>>(index.Model);
            Assert.Equal("Новая", items.First().Header);
            Assert.IsType<NotFoundResult>(await controller.Delete(999999));
            Assert.IsType<NotFoundResult>(await controller.DeleteConfirmed(999999));
            Assert.IsType<ViewResult>(await controller.Delete(id));
            Assert.IsType<RedirectToActionResult>(await controller.DeleteConfirmed(id));
        }

        await using var verifyDb = CreateAnketaContext();
        Assert.Equal(1, await verifyDb.News.CountAsync());
    }

    [Fact]
    public async Task ResultController_ProvidesFiltersAndSearchPartial()
    {
        await SeedAccessAsync(code: "10008");
        await using var db = CreateAnketaContext();
        var controller = new ResultController(db);

        var index = Assert.IsType<ViewResult>(await controller.IndexModerator());
        var model = Assert.IsType<MVC_ResultIndex>(index.Model);
        Assert.NotNull(model.Periods);
        Assert.NotNull(model.Filials);
        Assert.NotNull(model.ReservLevels);

        var search = Assert.IsType<PartialViewResult>(
            await controller.SearchResult(new MVC_ResultFilter()));
        Assert.Equal("_SearchResult", search.ViewName);
        Assert.IsType<MVC_ResultSearchPage>(search.Model);

        controller.ModelState.AddModelError("Page", "invalid");
        Assert.IsType<BadRequestObjectResult>(
            await controller.SearchResult(new MVC_ResultFilter()));
    }

    [Fact]
    public async Task ResultController_DetailPartialsValidateAccessAndCompetenceYear()
    {
        var seeded = await SeedAccessAsync(code: "10009");
        await using var db = CreateAnketaContext();
        var controller = new ResultController(db);

        Assert.IsType<BadRequestObjectResult>(await controller.GetTests360(0));
        Assert.IsType<NotFoundObjectResult>(await controller.GetTests360(999999));
        Assert.IsType<BadRequestObjectResult>(await controller.GetCompetentionTest(seeded.AccessId, 3));

        Assert.Equal("GetTests360",
            Assert.IsType<PartialViewResult>(await controller.GetTests360(seeded.AccessId)).ViewName);
        Assert.Equal("GetMotivationTest",
            Assert.IsType<PartialViewResult>(await controller.GetMotivationTest(seeded.AccessId)).ViewName);
        Assert.Equal("GetKettelTest",
            Assert.IsType<PartialViewResult>(await controller.GetKettelTest(seeded.AccessId)).ViewName);
        Assert.Equal("GetCompetentionTest",
            Assert.IsType<PartialViewResult>(await controller.GetCompetentionTest(seeded.AccessId, 2)).ViewName);
    }
}
