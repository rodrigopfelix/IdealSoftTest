using IdealSoftTestServer.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdealSoftTestServer.Infrastructure.Identity
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var configuration = services.GetRequiredService<IConfiguration>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            /*
             * dotnet user-secrets init
             * dotnet user-secrets set "Admin:Email" "EMAIL_HERE"
             * dotnet user-secrets set "Admin:Password" "PASSWORD_HERE"
             */

            var adminRole = "Admin";
            var adminEmail = configuration["Admin:Email"]
                ?? throw new Exception("Enviroment variable not found: Admin:Email");
            var adminPassword = configuration["Admin:Password"] 
                ?? throw new Exception("Enviroment variable not found: Admin:Password");

            // Role
            if (!await roleManager.RoleExistsAsync(adminRole))
                await roleManager.CreateAsync(new IdentityRole<Guid>(adminRole));

            // User
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser != null)
                return;

            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Administrador"
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (!result.Succeeded)
                throw new Exception("Error when creating admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }
}
