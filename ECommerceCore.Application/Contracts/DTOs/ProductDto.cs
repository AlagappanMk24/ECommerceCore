using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contracts.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string SKU { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsInStock => StockQuantity > 0;

        // Category
        public string CategoryName { get; set; }

        // Brand
        public string BrandName { get; set; }
        public List<ProductImage> ProductImages { get; set; }

        public bool IsFeatured { get; set; }
        public bool IsTrending { get; set; }
        public bool IsNewArrival { get; set; }

        public int Views { get; set; }
        public int SoldCount { get; set; }
        public double AverageRating { get; set; }
    }

}
