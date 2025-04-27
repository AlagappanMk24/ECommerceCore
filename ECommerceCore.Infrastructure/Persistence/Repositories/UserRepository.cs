using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Infrastructure.Data.Context;

namespace ECommerceCore.Infrastructure.Persistence.Repositories
{
    public class UserRepository(EcomDbContext dbContext) : GenericRepository<ApplicationUser>(dbContext), IUserRepository
    {
        private readonly EcomDbContext _dbContext = dbContext;
        public void Update(ApplicationUser applicationUser)
        {
            _dbContext.ApplicationUsers.Update(applicationUser);
        }
        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            try
            {
                // Save changes to the database
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
