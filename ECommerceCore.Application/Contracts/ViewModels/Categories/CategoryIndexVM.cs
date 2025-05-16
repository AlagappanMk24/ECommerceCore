using ECommerceCore.Application.Common.QueryParams;
using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contract.ViewModels.Categories
{
    public class CategoryIndexVM
    {
        public CategoryQueryParameters QueryParameters { get; set; } = new();
        public IEnumerable<Category> ParentCategories { get; set; } = new List<Category>();
    }

    public class CategoryQueryParameters : QueryParameters
    {
        public int? ParentCategoryId { get; set; }
        public bool? IsActive { get; set; }
    }
}