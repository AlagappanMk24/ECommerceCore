using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Domain.Models.Entities;
using ECommerceCore.Infrastructure.Data.Context;

namespace ECommerceCore.Infrastructure.Repositories
{
    public class CompanyRepository(EcomDbContext dbContext) : Repository<Company>(dbContext), ICompanyRepository
    {
        private readonly EcomDbContext _dbContext = dbContext;
        public void Update(Company obj)
        {
            _dbContext.Companies.Update(obj);
        }
    }
}
