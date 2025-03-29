using ECommerceCore.Application.Constants;
using ECommerceCore.Domain.Models.Entities;
using ECommerceCore.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Data.DbInitializer
{
    public class DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, EcomDbContext dbContext) : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManger = userManager;
        private readonly RoleManager<IdentityRole> _roleManger = roleManager;
        private readonly EcomDbContext _dbContext = dbContext;

        public void Initialize()
        {
            //migrations if they are not applied
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _dbContext.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception("Error applying migrations", ex);
            }

            //Create roles if they are not created
            //To add role in Db side
            if (!_roleManger.RoleExistsAsync(AppConstants.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManger.CreateAsync(new IdentityRole(AppConstants.Role_Customer)).GetAwaiter().GetResult();
                _roleManger.CreateAsync(new IdentityRole(AppConstants.Role_Employee)).GetAwaiter().GetResult();
                _roleManger.CreateAsync(new IdentityRole(AppConstants.Role_Admin)).GetAwaiter().GetResult();
                _roleManger.CreateAsync(new IdentityRole(AppConstants.Role_Company)).GetAwaiter().GetResult();

                //If roles are not created, then we will crete admin user as well
                _userManger.CreateAsync(new ApplicationUser
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

                ApplicationUser user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Email == "alagappanmk98@gmail.com");
                _userManger.AddToRoleAsync(user, AppConstants.Role_Admin).GetAwaiter().GetResult();

            }
            return;
        }
    }
}
