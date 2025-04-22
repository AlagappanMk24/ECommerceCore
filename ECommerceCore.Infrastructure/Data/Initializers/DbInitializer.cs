using ECommerceCore.Application.Constants;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerceCore.Infrastructure.Data.DbInitializer
{
    // The DbInitializer is then responsible for broader database initialization tasks like migrations and initial user/role setup.
    public class DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, EcomDbContext dbContext, ILogger<DbInitializer> logger) : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManger = userManager;
        private readonly RoleManager<IdentityRole> _roleManger = roleManager;
        private readonly EcomDbContext _dbContext = dbContext;
        private readonly ILogger<DbInitializer> _logger = logger;

        public void Initialize()
        {
            //migrations if they are not applied
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _logger.LogInformation("Applying pending migrations...");
                    _dbContext.Database.Migrate();
                    _logger.LogInformation("Pending migrations applied successfully.");
                }
                else
                {
                    _logger.LogInformation("No pending migrations to apply.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while applying migrations.");
                throw new Exception("Error applying migrations", ex);
            }

            //Create roles if they are not created
            if (!_roleManger.RoleExistsAsync(AppConstants.Role_Customer).GetAwaiter().GetResult())
            {
                _logger.LogInformation("Creating default roles...");
                _roleManger.CreateAsync(new IdentityRole(AppConstants.Role_Customer)).GetAwaiter().GetResult();
                _roleManger.CreateAsync(new IdentityRole(AppConstants.Role_Employee)).GetAwaiter().GetResult();
                _roleManger.CreateAsync(new IdentityRole(AppConstants.Role_Admin)).GetAwaiter().GetResult();
                _roleManger.CreateAsync(new IdentityRole(AppConstants.Role_Company)).GetAwaiter().GetResult();
                _logger.LogInformation("Default roles created successfully.");

                //If roles are not created, then we will try to create admin user
                var creationResult = _userManger.CreateAsync(new ApplicationUser
                {
                    UserName = "alagappanmk98@gmail.com",
                    Email = "alagappanmk98@gmail.com",
                    Name = "Alagappan",
                    PhoneNumber = "8668453402",
                    StreetAddress = "123 St, Besant Nagar",
                    State = "TamilNadu",
                    PostalCode = "600001",
                    City = "Chennai"
                }, "Alagappan@123").GetAwaiter().GetResult();

                if (creationResult.Succeeded)
                {
                    _logger.LogInformation("Admin user created successfully.");
                    ApplicationUser user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Email == "alagappanmk98@gmail.com");
                    if (user != null)
                    {
                        var roleResult = _userManger.AddToRoleAsync(user, AppConstants.Role_Admin).GetAwaiter().GetResult();
                        if (roleResult.Succeeded)
                        {
                            _logger.LogInformation($"Admin user '{user.Email}' added to the '{AppConstants.Role_Admin}' role.");
                        }
                        else
                        {
                            _logger.LogError($"Failed to add admin user '{user.Email}' to the '{AppConstants.Role_Admin}' role.");
                            foreach (var error in roleResult.Errors)
                            {
                                _logger.LogError($"Role assignment error: {error.Description}");
                            }
                        }
                    }
                    else
                    {
                        _logger.LogError("Failed to create the default admin user.");
                        foreach (var error in creationResult.Errors)
                        {
                            _logger.LogError($"User creation error: {error.Description}");
                        }
                    }
                }
                else
                {
                    _logger.LogInformation("Default roles already exist.");
                }
            }
            return;
        }
    }
}
