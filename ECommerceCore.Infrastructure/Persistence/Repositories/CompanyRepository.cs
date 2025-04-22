using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Infrastructure.Data.Context;

namespace ECommerceCore.Infrastructure.Persistence.Repositories
{
    public class CompanyRepository(EcomDbContext dbContext) : GenericRepository<Company>(dbContext), ICompanyRepository
    {
        private readonly EcomDbContext _dbContext = dbContext;
        public void Update(Company obj)
        {
            _dbContext.Companies.Update(obj);
        }
    }
}