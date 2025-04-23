using ECommerceCore.Application.Common.QueryParameters;
using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contracts.ViewModels
{
    public class ProductIndexVM
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public ProductQueryParameters QueryParameters { get; set; }
    }
}
