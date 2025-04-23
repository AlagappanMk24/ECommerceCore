using ECommerceCore.Application.Contracts.Persistence;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Infrastructure.Data.Context;

namespace ECommerceCore.Infrastructure.Persistence.Repositories
{
    public class BrandRepository(EcomDbContext dbContext) : GenericRepository<Brand>(dbContext), IBrandRepository
    {
        private EcomDbContext _dbContext = dbContext;
    }
}
