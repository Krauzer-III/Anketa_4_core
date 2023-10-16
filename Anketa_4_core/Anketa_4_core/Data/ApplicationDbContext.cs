using Anketa_4_core.Data.LK_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Anketa_4_core.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {                      

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }                  
    }

    public class ApplicationDbContext_Initializer
    {
        public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminName = "admin";
            string adminpassword = "!QAZxsw2#EDCvfr4";

            string moderatorName = "moderator";
            string moderatorpassword = "Q!w2e3r4T%";


            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("moderator") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("moderator"));
            }
            if (await roleManager.FindByNameAsync("verificator") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("verificator"));
            }
            if (await roleManager.FindByNameAsync("testable_user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("testable_user"));
            }
            if (await roleManager.FindByNameAsync("filial_user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("filial_user"));
            }
            if (await userManager.FindByNameAsync(adminName) == null)
            {
                IdentityUser admin = new IdentityUser { UserName = adminName };
                IdentityResult result = await userManager.CreateAsync(admin, adminpassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }

            if (await userManager.FindByNameAsync(moderatorName) == null)
            {
                IdentityUser moder = new IdentityUser { UserName = moderatorName };
                IdentityResult result = await userManager.CreateAsync(moder, moderatorpassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(moder, "moderator");
                }
            }
        }
    }


}