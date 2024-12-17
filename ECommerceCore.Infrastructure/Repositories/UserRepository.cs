using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Domain.Models.Entities;
using ECommerceCore.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Repositories
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private EcomDbContext _dbContext;
        public UserRepository(EcomDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(ApplicationUser applicationUser)
        {
            _dbContext.ApplicationUsers.Update(applicationUser);
        }
    }
}
