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
                objFromDb.Description = obj.Description;
                objFromDb.ShortDescription = obj.ShortDescription;
                objFromDb.Price = obj.Price;
                objFromDb.DiscountPrice = obj.DiscountPrice;
                objFromDb.IsDiscounted = obj.IsDiscounted;
                objFromDb.DiscountStartDate = obj.DiscountStartDate;
                objFromDb.DiscountEndDate = obj.DiscountEndDate;
                objFromDb.StockQuantity = obj.StockQuantity;
                objFromDb.AllowBackorder = obj.AllowBackorder;
                objFromDb.SKU = obj.SKU;
                objFromDb.Barcode = obj.Barcode;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.BrandId = obj.BrandId;
                objFromDb.WeightInKg = obj.WeightInKg;
                objFromDb.WidthInCm = obj.WidthInCm;
                objFromDb.HeightInCm = obj.HeightInCm;
                objFromDb.LengthInCm = obj.LengthInCm;
                objFromDb.IsActive = obj.IsActive;
                objFromDb.IsFeatured = obj.IsFeatured;
                objFromDb.IsNewArrival = obj.IsNewArrival;
                objFromDb.IsTrending = obj.IsTrending;
                objFromDb.MetaTitle = obj.MetaTitle;
                objFromDb.MetaDescription = obj.MetaDescription;
                // We typically don't update analytics fields from a direct update
                // as they might be managed by other processes
                // objFromDb.Views = obj.Views;
                // objFromDb.SoldCount = obj.SoldCount;
                objFromDb.AverageRating = obj.AverageRating;
                objFromDb.TotalReviews = obj.TotalReviews;

                // If ProductImages is not null, update them
                if (obj.ProductImages != null)
                {
                    objFromDb.ProductImages = obj.ProductImages;
                }

                // If Specifications is not null, update them
                if (obj.Specifications != null)
                {
                    objFromDb.Specifications = obj.Specifications;
                }

                // If Variants is not null, update them
                if (obj.Variants != null)
                {
                    objFromDb.Variants = obj.Variants;
                }

                // If Tags is not null, update them
                if (obj.Tags != null)
                {
                    objFromDb.Tags = obj.Tags;
                }

            }
        }
        public async Task<Product> GetAsync(Expression<Func<Product, bool>> filter)
        {
            return await _dbContext.Products
            .Include(p => p.ProductImages)
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Include(p => p.Specifications)
            .Include(p => p.Variants)
            .Include(p => p.Tags)
            .FirstOrDefaultAsync(filter);
        }
    }
}
