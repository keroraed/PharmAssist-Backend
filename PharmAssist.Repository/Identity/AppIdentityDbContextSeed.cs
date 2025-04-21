using Microsoft.AspNetCore.Identity;
using PharmAssist.Core.Entities.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace PharmAssist.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed roles
            await SeedRolesAsync(roleManager);

            // Seed regular user if no users exist
            if (!userManager.Users.Any())
            {
                var regularUser = new AppUser()
                {
                    DisplayName = "Basant Kamal",
                    Email = "basantkamal@gmail.com",
                    UserName = "basantkamal",
                    PhoneNumber = "01110986580"
                };

                await userManager.CreateAsync(regularUser, "Pa$$w0rd");
                await userManager.AddToRoleAsync(regularUser, "User");
            }

            // Always check and create admin user regardless of existing users
            await SeedAdminUserAsync(userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            // Create roles if they don't exist
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                // Check if role exists
                var roleExists = await roleManager.RoleExistsAsync(roleName);

                if (!roleExists)
                {
                    // Create role
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
        {
            // Check if admin user exists
            var adminUser = await userManager.FindByNameAsync("admin");

            if (adminUser == null)
            {
                // Create admin user if it doesn't exist
                adminUser = new AppUser()
                {
                    DisplayName = "Admin User",
                    Email = "admin@pharmassist.com",
                    UserName = "admin",
                    PhoneNumber = "01000000000"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
            else
            {
                // Ensure the existing admin user has the Admin role
                var isInAdminRole = await userManager.IsInRoleAsync(adminUser, "Admin");

                if (!isInAdminRole)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
