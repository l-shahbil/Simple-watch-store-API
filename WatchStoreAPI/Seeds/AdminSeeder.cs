namespace Watchs_Store.Seeder
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using WatchStoreAPI.Models;
    using WatchStoreAPI.Models.DTO;

    public class AdminSeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AdminSeeder> _logger;

        public AdminSeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<AdminSeeder> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    _logger.LogInformation("Role 'Admin' created successfully.");
                }

                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                    _logger.LogInformation("Role 'User' created successfully.");
                }

                if (!_userManager.Users.Any())
                {
                    var admin = new ApplicationUser
                    {
                        UserName = "Admin",
                        name = "Admin",
                        Email = "Admin@admin.com",
                        EmailConfirmed = true
                    };

                    var adminResult = await _userManager.CreateAsync(admin, "admin12345");
                    if (adminResult.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(admin, "Admin");

                        var adminClaim = new Claim("Role", "AdminUser");
                        await _userManager.AddClaimAsync(admin, adminClaim);

                        _logger.LogInformation("Admin user created and assigned to 'SuperAdmin' role.");
                    }
                    else
                    {
                        _logger.LogError("Error creating Super Admin user: {Errors}", string.Join(", ", adminResult.Errors.Select(e => e.Description)));
                    }


                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the seeding process.");
            }
        }
    }
}



