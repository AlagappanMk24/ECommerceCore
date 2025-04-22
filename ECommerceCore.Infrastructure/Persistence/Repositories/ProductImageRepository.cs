using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Infrastructure.Data.Context;

namespace ECommerceCore.Infrastructure.Persistence.Repositories
{
    public class ProductImageRepository(EcomDbContext dbContext) : GenericRepository<ProductImage>(dbContext), IProductImageRepository
    {
        private EcomDbContext _dbContext = dbContext;

        public void Update(ProductImage obj)
        {
            _dbContext.ProductImages.Update(obj);
        }
    }
}
