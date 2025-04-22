using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerceCore.Infrastructure.Persistence.Repositories
{
    public class ProductRepository(EcomDbContext dbContext) : GenericRepository<Product>(dbContext), IProductRepository
    {
        private readonly EcomDbContext _dbContext = dbContext;

        public void Update(Product obj)
        {
            var objFromDb = _dbContext.Products.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.ISBN = obj.ISBN;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;
                objFromDb.ListPrice = obj.ListPrice;
                objFromDb.Price100 = obj.Price100;
                objFromDb.Description = obj.Description;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.Author = obj.Author;
                objFromDb.ProductImages = obj.ProductImages;

            }
        }
        public async Task<Product> GetAsync(Expression<Func<Product, bool>> filter)
        {
            return await _dbContext.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(filter);
        }
    }
}
