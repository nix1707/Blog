using AspNetCoreGeneratedDocument;
using Blog.Data;
using Microsoft.AspNetCore.Identity;

namespace Blog;

public class Seed
{

    public static async Task SeedData(
        AppDbContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager, 
        ILogger<Seed> logger)
        
    {
        try
        {
            context.Database.EnsureCreated();

            var adminRole = new IdentityRole("Admin");

            if (context.Roles.Any() == false)
            {
                await roleManager.CreateAsync(adminRole);
            }

            if (context.Users.Any(u => u.UserName == "admin") == false)
            {
                var admin = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@email.com"
                };

                var result = await userManager.CreateAsync(admin, "password");
                await userManager.AddToRoleAsync(admin, adminRole!.Name!);
            }
        }
        catch(Exception ex)
        {
            logger.LogError("Error with seeding data: {ex}", ex);
        }

    }
}

