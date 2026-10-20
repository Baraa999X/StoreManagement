using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
namespace StoreManagement.Infrastructure.Identity
{
    public class IdentitySeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "Customer" };

            foreach (var role in roles)
            {
                bool roleExists = await roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));

                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
        }

        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            string? email = configuration["AdminSeed:Email"];
            string? password = configuration["AdminSeed:Password"];

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("Admin seed settings are missing.");
            }

            var admin = await userManager.FindByEmailAsync(email);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, password);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException( string.Join("; ",result.Errors.Select(e => e.Description)));
                }

                var roleResult = await userManager.AddToRoleAsync(admin, "Admin");

                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(string.Join("; ",roleResult.Errors.Select(e => e.Description)));
                }
            }
            else if (!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                throw new InvalidOperationException("The configured admin account exists but does not have the Admin role.");
            }
        }

    }
}
