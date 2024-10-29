using CourseProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Infraestructure
{
    public static class AppBuilderExtensions
    {
        public static async Task InitializeRolesAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            string[] roleNames = { "User", "Admin" };

            foreach (var role in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int> { Name = role });
                }
            }

            var adminUser = await userManager.FindByEmailAsync("admin@app.com");
            if (adminUser == null)
            {
                var user = new User { UserName = "admin@app.com", Email = "admin@app.com", LastName = "Admin", Name = "Admin", NormalizedEmail = "ADMIN@APP.COM", NormalizedUserName = "ADMIN@APP.COM", Status = "Active" };
                var result = await userManager.CreateAsync(user, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");

                }
            }
        }
    }
}
