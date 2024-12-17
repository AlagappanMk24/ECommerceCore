using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Domain.Models.Entities;
using ECommerceCore.Infrastructure.Data.Context;

namespace ECommerceCore.Infrastructure.Repositories
{
    public class ProductImageRepository(EcomDbContext dbContext) : Repository<ProductImage>(dbContext), IProductImageRepository
    {
        private EcomDbContext _dbContext = dbContext;

        public void Update(ProductImage obj)
        {
            _dbContext.ProductImages.Update(obj);
        }
    }
}
