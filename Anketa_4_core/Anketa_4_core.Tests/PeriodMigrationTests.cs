using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql;
using Xunit;

namespace Anketa_4_core.Tests;

/// <summary>
/// Verify the real EF migrations, not only EnsureCreated's new-model schema.
/// An older database with duplicate links must be rejected without data loss.
/// </summary>
public sealed class PeriodMigrationTests : PostgresTestBase
{
    [Fact]
    public async Task FreshDatabase_AllMigrationsApplyAndUniqueIndexRejectsDuplicate()
    {
        await WithMigrationDatabase(async db =>
        {
            await db.Database.MigrateAsync();
            var applied = await db.Database.GetAppliedMigrationsAsync();
            Assert.Contains("20261123000000_SinglePeriodPerTestable", applied);

            var period1 = new TestPeriod { GroupName = "Первый" };
            var period2 = new TestPeriod { GroupName = "Второй" };
            var testable = new Testable
            {
                Code = "migration_unique", YearTraining = 2026,
                filial = new Filial { FilialName = "Филиал" },
                reservLevel = new ReservLevel { ReservLevelName = "Резерв" }
            };
            db.AddRange(period1, period2, testable);
            await db.SaveChangesAsync();

            db.AccessForTestables.Add(new AccessForTestable
            {
                Testable = testable, TestPeriod = period1, isActive = true
            });
            await db.SaveChangesAsync();

            db.AccessForTestables.Add(new AccessForTestable
            {
                Testable = testable, TestPeriod = period2, isActive = true
            });
            await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
        });
    }

    [Fact]
    public async Task ExistingDatabase_WithDuplicateAccessAndResults_RejectsMigrationWithoutLoss()
    {
        await WithMigrationDatabase(async db =>
        {
            await db.GetService<IMigrator>().MigrateAsync("20231219101011_migr_03");
            var p1 = new TestPeriod { GroupName = "Прежний" };
            var p2 = new TestPeriod { GroupName = "Поздний" };
            var testable = new Testable
            {
                Code = "migration_legacy", YearTraining = 2025,
                filial = new Filial { FilialName = "Филиал" },
                reservLevel = new ReservLevel { ReservLevelName = "Резерв" }
            };
            db.AddRange(p1, p2, testable);
            await db.SaveChangesAsync();
            var a1 = new AccessForTestable { Testable = testable, TestPeriod = p1, isActive = true };
            var a2 = new AccessForTestable { Testable = testable, TestPeriod = p2, isActive = true };
            db.AccessForTestables.AddRange(a1, a2);
            db.TestResults.Add(new TestResult
            {
                Access = a1, Results = """{"AccessID":1,"Tests360":[]}"""
            });
            await db.SaveChangesAsync();

            var exception = await Assert.ThrowsAsync<PostgresException>(
                () => db.Database.MigrateAsync());
            Assert.Contains("несколькими назначениями", exception.Message);

            // Verify from a fresh context, after the failed transaction was rolled back.
            await using var check = new AnketaContext(
                new DbContextOptionsBuilder<AnketaContext>()
                    .UseNpgsql(db.Database.GetConnectionString()).Options);
            Assert.Equal(2, await check.AccessForTestables.CountAsync(x =>
                x.Testable.ID == testable.ID));
            Assert.Equal(1, await check.TestResults.CountAsync());
            Assert.DoesNotContain("20261123000000_SinglePeriodPerTestable",
                await check.Database.GetAppliedMigrationsAsync());
        });
    }

    private async Task WithMigrationDatabase(Func<AnketaContext, Task> action)
    {
        var builder = new NpgsqlConnectionStringBuilder(AnketaConnectionString)
        {
            Database = "migration_ut_" + Guid.NewGuid().ToString("N")[..12]
        };
        await using var db = new AnketaContext(new DbContextOptionsBuilder<AnketaContext>()
            .UseNpgsql(builder.ConnectionString).Options);
        try
        {
            await action(db);
        }
        finally
        {
            await db.Database.EnsureDeletedAsync();
        }
    }
}
