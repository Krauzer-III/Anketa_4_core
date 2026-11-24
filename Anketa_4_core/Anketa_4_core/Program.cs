using Anketa_4_core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Anketa_4_core.Helpers;

// ...

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var IdentityConnectionString = builder.Configuration.GetConnectionString("IdentityConnection");
var AnketaConnectionString = builder.Configuration.GetConnectionString("AnketaConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(IdentityConnectionString));
builder.Services.AddDbContext<AnketaContext>(options =>
    options.UseNpgsql(AnketaConnectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.User.RequireUniqueEmail = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();
// Личное управление учётной записью отключено для всех ролей.
builder.Services.AddAuthorization(options =>
    options.AddPolicy("NoAccountManagement", policy => policy.RequireAssertion(_ => false)));
builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage", "NoAccountManagement"));

var app = builder.Build();

// Ïðèìåíÿåì ìèãðàöèè è ñèäèì äàííûå
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Ïðèìåíÿåì ìèãðàöèè äëÿ Identity
    var identityDb = services.GetRequiredService<ApplicationDbContext>();
    await identityDb.Database.MigrateAsync();

    // Миграции базы Анкеты включают исходные справочники, тесты и ключи.
    // EF применяет каждую миграцию только один раз; повторный запуск не дублирует данные.
    var anketaDb = services.GetRequiredService<AnketaContext>();
    await anketaDb.Database.MigrateAsync();

    // Ñèäèì ðîëè è ïîëüçîâàòåëåé
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    await ApplicationDbContext_Initializer.InitializeAsync(userManager, roleManager);
}

// äàëåå âàø êîíâåéåð:
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();



/*using Anketa_4_core.Data;
using Anketa_4_core.Data.LK_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var IdentityConnectionString = builder.Configuration.GetConnectionString("IdentityConnection");
var AnketaConnectionString = builder.Configuration.GetConnectionString("AnketaConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(IdentityConnectionString));
builder.Services.AddDbContext<AnketaContext>(options =>
    options.UseNpgsql(AnketaConnectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
        {
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.User.RequireUniqueEmail = false;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
        })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
                          */