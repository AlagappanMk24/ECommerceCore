using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Domain.Models.Entities;
using ECommerceCore.Infrastructure.Data.Context;

namespace ECommerceCore.Infrastructure.Repositories
{
    public class CategoryRepository(EcomDbContext dbContext) : Repository<Category>(dbContext), ICategoryRepository
    {
        private EcomDbContext _dbContext = dbContext;

        public void Update(Category obj)
        {
            _dbContext.Categories.Update(obj);
        }
    }
}
