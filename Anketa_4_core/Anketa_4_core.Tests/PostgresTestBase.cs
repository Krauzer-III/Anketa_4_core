using System.Collections.Concurrent;
using System.Security.Claims;
using System.Text.Json;
using Anketa_4_core.Data;
using Anketa_4_core.Data.AnketaModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Xunit;

namespace Anketa_4_core.Tests;

public abstract class PostgresTestBase : IAsyncLifetime
{
    private string _anketaConnection = "";
    private string _identityConnection = "";
    private string _anketaDatabase = "";
    private string _identityDatabase = "";
    private ServiceProvider? _identityServices;
    private readonly ConcurrentBag<AnketaContext> _createdContexts = new();

    protected AnketaContext CreateAnketaContext()
    {
        var context = new AnketaContext(new DbContextOptionsBuilder<AnketaContext>()
            .UseNpgsql(_anketaConnection)
            .EnableDetailedErrors()
            .Options);
        _createdContexts.Add(context);
        return context;
    }

    protected ApplicationDbContext IdentityDb =>
        _identityServices!.GetRequiredService<ApplicationDbContext>();

    protected UserManager<IdentityUser> UserManager =>
        _identityServices!.GetRequiredService<UserManager<IdentityUser>>();

    protected RoleManager<IdentityRole> RoleManager =>
        _identityServices!.GetRequiredService<RoleManager<IdentityRole>>();

    public async Task InitializeAsync()
    {
        var baseConnection = Environment.GetEnvironmentVariable("ANKETA_TEST_CONNECTION")
            ?? "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=postgres";

        var anketaBuilder = new NpgsqlConnectionStringBuilder(baseConnection);
        var identityBuilder = new NpgsqlConnectionStringBuilder(baseConnection);
        var suffix = Guid.NewGuid().ToString("N")[..12];
        _anketaDatabase = "anketa_ut_" + suffix;
        _identityDatabase = "identity_ut_" + suffix;
        anketaBuilder.Database = _anketaDatabase;
        identityBuilder.Database = _identityDatabase;
        _anketaConnection = anketaBuilder.ConnectionString;
        _identityConnection = identityBuilder.ConnectionString;

        await using (var db = CreateAnketaContext())
            await db.Database.EnsureCreatedAsync();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(_identityConnection));
        services.AddIdentityCore<IdentityUser>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

        _identityServices = services.BuildServiceProvider();
        await IdentityDb.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        foreach (var context in _createdContexts)
        {
            try { await context.DisposeAsync(); }
            catch { }
        }

        if (_identityServices != null)
        {
            await _identityServices.DisposeAsync();
            _identityServices = null;
        }

        if (!string.IsNullOrWhiteSpace(_anketaDatabase))
        {
            await using var db = new AnketaContext(
                new DbContextOptionsBuilder<AnketaContext>()
                    .UseNpgsql(_anketaConnection).Options);
            await db.Database.EnsureDeletedAsync();
        }

        if (!string.IsNullOrWhiteSpace(_identityDatabase))
        {
            await using var db = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseNpgsql(_identityConnection).Options);
            await db.Database.EnsureDeletedAsync();
        }
    }

    protected async Task<SeededAccess> SeedAccessAsync(
        string code = "1001",
        string periodName = "Период 1",
        string filialName = "Филиал 1",
        string? username = null,
        string? lkData = null,
        int yearTraining = 1,
        bool active = true,
        string? reservLevelName = null)
    {
        await using var db = CreateAnketaContext();
        var filial = new Filial { FilialName = filialName };
        var period = new TestPeriod { GroupName = periodName };
        var reservLevel = new ReservLevel
        {
            ReservLevelName = reservLevelName ?? "Уровень " + code
        };
        var testable = new Testable
        {
            Code = code,
            YearTraining = yearTraining,
            isArchived = false,
            filial = filial,
            reservLevel = reservLevel
        };
        db.Testables.Add(testable);
        db.TestPeriods.Add(period);
        db.ReservLevels.Add(reservLevel);
        await db.SaveChangesAsync();

        var access = new AccessForTestable
        {
            Testable = testable,
            TestPeriod = period,
            isActive = active
        };
        db.AccessForTestables.Add(access);

        if (username != null)
        {
            db.TestableUsers.Add(new TestableUser
            {
                UserName = username,
                testable = testable,
                LK_Data = lkData ?? "{}"
            });
        }

        await db.SaveChangesAsync();
        return new SeededAccess(testable.ID, access.ID, period.ID, filial.ID, reservLevel.ID);
    }

    protected async Task AddAccessToExistingTestableAsync(
        int testableId, int periodId, bool active = true)
    {
        await using var db = CreateAnketaContext();
        var testable = await db.Testables.FindAsync(testableId)
            ?? throw new InvalidOperationException("Testable not found");
        var period = await db.TestPeriods.FindAsync(periodId)
            ?? throw new InvalidOperationException("TestPeriod not found");
        db.AccessForTestables.Add(new AccessForTestable
        {
            Testable = testable,
            TestPeriod = period,
            isActive = active
        });
        await db.SaveChangesAsync();
    }

    protected async Task SaveResultAsync(int accessId, JSON_ResultModel result)
    {
        await using var db = CreateAnketaContext();
        var access = await db.AccessForTestables.Include(x => x.Testable)
            .FirstAsync(x => x.ID == accessId);
        result.AccessID = accessId;
        result.TestableCode = access.Testable.Code;
        db.TestResults.Add(new TestResult { Access = access, Results = result.GetJSON() });
        await db.SaveChangesAsync();
    }

    protected async Task<IdentityUser> CreateUserAsync(
        string username,
        string role,
        string password = "Test123")
    {
        if (await RoleManager.FindByNameAsync(role) == null)
        {
            var roleResult = await RoleManager.CreateAsync(new IdentityRole(role));
            if (!roleResult.Succeeded)
                throw new InvalidOperationException(string.Join("; ", roleResult.Errors.Select(x => x.Description)));
        }

        var user = new IdentityUser { UserName = username };
        var createResult = await UserManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
            throw new InvalidOperationException(string.Join("; ", createResult.Errors.Select(x => x.Description)));

        var addRole = await UserManager.AddToRoleAsync(user, role);
        if (!addRole.Succeeded)
            throw new InvalidOperationException(string.Join("; ", addRole.Errors.Select(x => x.Description)));
        return user;
    }

    protected static void SetControllerUser(Controller controller, IdentityUser user)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? "")
        }, "Test");
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
        };
    }

    protected static JsonElement Element(string json)
    {
        using var document = JsonDocument.Parse(json);
        return document.RootElement.Clone();
    }

    protected readonly record struct SeededAccess(
        int TestableId,
        int AccessId,
        int PeriodId,
        int FilialId,
        int ReservLevelId);
}
