using ECommerceCore.Application.Common.QueryParams;
using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contracts.ViewModels.Products
{
    public class ProductIndexVM
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public ProductQueryParameters QueryParameters { get; set; }
    }

    public class ProductQueryParameters : QueryParameters
    {
        public int? CategoryId { get; set; }
        public string? StockStatus { get; set; } // "in-stock", "low-stock", "out-of-stock"
    }
}