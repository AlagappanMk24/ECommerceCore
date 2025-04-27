using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contract.ViewModels
{
    public class CategoryIndexVM
    {
        public CategoryQueryParameters QueryParameters { get; set; } = new();
        public IEnumerable<Category> ParentCategories { get; set; } = new List<Category>();
    }

    public class CategoryQueryParameters 
    {
        public int? ParentCategoryId { get; set; }
        public bool? IsActive { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; }
        public string? SearchTerm { get; set; }
    }
}