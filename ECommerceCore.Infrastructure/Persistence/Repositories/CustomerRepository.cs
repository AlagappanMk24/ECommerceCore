using ECommerceCore.Application.Contracts.Persistence;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Infrastructure.Data.Context;

namespace ECommerceCore.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository(EcomDbContext dbContext) : GenericRepository<Customer>(dbContext), ICustomerRepository
    {
        private EcomDbContext _dbContext = dbContext;
    }
}
