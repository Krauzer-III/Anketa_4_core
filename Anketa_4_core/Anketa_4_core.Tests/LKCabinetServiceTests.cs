using System.Text.Json.Nodes;
using Anketa_4_core.Services;
using Xunit;

namespace Anketa_4_core.Tests;

public sealed class LKCabinetServiceTests : PostgresTestBase
{
    [Fact]
    public void ParseRoot_ReturnsEmptyObjectForNullOrWhitespace()
    {
        Assert.Empty(LKCabinetService.ParseRoot(null));
        Assert.Empty(LKCabinetService.ParseRoot("   "));
    }

    [Fact]
    public void ParseRoot_RejectsArraysAndMalformedJson()
    {
        Assert.Throws<InvalidOperationException>(() => LKCabinetService.ParseRoot("[]"));
        Assert.Throws<InvalidOperationException>(() => LKCabinetService.ParseRoot("{broken"));
        Assert.Throws<InvalidOperationException>(() => LKCabinetService.ParseRoot("null"));
    }

    [Fact]
    public async Task LoadForUserAsync_AddsOfficialDefaultsAndPreservesUnknownProperties()
    {
        await SeedAccessAsync(
            username: "employee",
            lkData: """{"Custom":{"keep":true},"ProfTasks":[{"Number":7}]}""");

        await using var db = CreateAnketaContext();
        var result = await new LKCabinetService(db).LoadForUserAsync("employee");

        Assert.Equal(8, result["DistanceLearning"]!.AsArray().Count);
        Assert.Equal(3, result["ReportBooks"]!.AsArray().Count);
        Assert.Equal(7, (int)result["ProfTasks"]![0]!["Number"]!);
        Assert.True((bool)result["Custom"]!["keep"]!);
    }

    [Fact]
    public async Task LoadForUserAsync_RejectsUnknownUserAndCorruptedStoredJson()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await using var db = CreateAnketaContext();
            await new LKCabinetService(db).LoadForUserAsync("missing");
        });

        await SeedAccessAsync(
            code: "1002",
            username: "broken",
            lkData: "{not-json");

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await using var db = CreateAnketaContext();
            await new LKCabinetService(db).LoadForUserAsync("broken");
        });
    }

    [Fact]
    public async Task LoadForTestableAsync_RejectsTestableWithoutCabinet()
    {
        var seeded = await SeedAccessAsync(code: "1003");
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await using var db = CreateAnketaContext();
            await new LKCabinetService(db).LoadForTestableAsync(seeded.TestableId);
        });
    }

    [Fact]
    public async Task SaveForUserAsync_ReplacesOnlySelectedSectionAndKeepsUnknownData()
    {
        await SeedAccessAsync(
            username: "employee",
            lkData: """{"Custom":{"keep":true},"PersonnelMotivation":{"AdditionalComment":"leave me"},"ProfTasks":[{"Number":1}]}""");

        await using var db = CreateAnketaContext();
        var saved = await new LKCabinetService(db).SaveForUserAsync(
            "employee",
            "ProfTasks",
            Element("""[{"Number":2,"ProfTaskName":"Новая задача","Unexpected":"ignored"}]"""));

        Assert.Equal(2, (int)saved["ProfTasks"]![0]!["Number"]!);
        Assert.Null(saved["ProfTasks"]![0]!["Unexpected"]);
        Assert.True((bool)saved["Custom"]!["keep"]!);
        Assert.Equal("leave me", (string)saved["PersonnelMotivation"]!["AdditionalComment"]!);

        await using var verifyDb = CreateAnketaContext();
        var persisted = await new LKCabinetService(verifyDb).LoadForUserAsync("employee");
        Assert.Equal(2, (int)persisted["ProfTasks"]![0]!["Number"]!);
        Assert.True((bool)persisted["Custom"]!["keep"]!);
    }

    [Fact]
    public async Task SaveForTestableAsync_UpdatesObjectSection()
    {
        var seeded = await SeedAccessAsync(username: "employee");

        await using var db = CreateAnketaContext();
        var saved = await new LKCabinetService(db).SaveForTestableAsync(
            seeded.TestableId,
            "PersonnelMotivation",
            Element("""{"WorkSignificanceAction1":"Проводить встречи","AdditionalComment":"Комментарий"}"""));

        Assert.Equal("Проводить встречи",
            (string)saved["PersonnelMotivation"]!["WorkSignificanceAction1"]!);
        Assert.Equal("Комментарий",
            (string)saved["PersonnelMotivation"]!["AdditionalComment"]!);
    }

    [Theory]
    [InlineData("UnknownSection", "[]", typeof(ArgumentException))]
    [InlineData("ProfTasks", "{}", typeof(ArgumentException))]
    [InlineData("PersonnelMotivation", "[]", typeof(ArgumentException))]
    public async Task SaveForUserAsync_RejectsUnsupportedSectionAndWrongShape(
        string section, string json, Type expectedException)
    {
        await SeedAccessAsync(username: "employee");
        await using var db = CreateAnketaContext();
        var service = new LKCabinetService(db);

        if (expectedException == typeof(ArgumentException))
            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.SaveForUserAsync("employee", section, Element(json)));
    }

    [Fact]
    public async Task SaveForUserAsync_RejectsInvalidTypedPayload()
    {
        await SeedAccessAsync(username: "employee");
        await using var db = CreateAnketaContext();

        await Assert.ThrowsAsync<System.Text.Json.JsonException>(() =>
            new LKCabinetService(db).SaveForUserAsync(
                "employee", "ProfTasks", Element("""["not an object"]""")));
    }

    [Fact]
    public async Task SaveForUserAsync_RejectsUnknownAccount()
    {
        await using var db = CreateAnketaContext();
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new LKCabinetService(db).SaveForUserAsync("missing", "ProfTasks", Element("[]")));
    }

    [Fact]
    public async Task ConcurrentSectionUpdates_DoNotOverwriteEachOther()
    {
        await SeedAccessAsync(
            username: "employee",
            lkData: """{"Custom":{"keep":true}}""");

        var first = Task.Run(async () =>
        {
            await using var db = CreateAnketaContext();
            return await new LKCabinetService(db).SaveForUserAsync(
                "employee", "ProfTasks", Element("""[{"Number":1,"ProfTaskName":"Задача"}]"""));
        });
        var second = Task.Run(async () =>
        {
            await using var db = CreateAnketaContext();
            return await new LKCabinetService(db).SaveForUserAsync(
                "employee", "PersonnelMotivation", Element("""{"WorkVarietyAction1":"Разнообразие"}"""));
        });

        await Task.WhenAll(first, second);

        await using var verifyDb = CreateAnketaContext();
        var final = await new LKCabinetService(verifyDb).LoadForUserAsync("employee");
        Assert.Equal(1, final["ProfTasks"]!.AsArray().Count);
        Assert.Equal("Разнообразие",
            (string)final["PersonnelMotivation"]!["WorkVarietyAction1"]!);
        Assert.True((bool)final["Custom"]!["keep"]!);
    }
}
