using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contract.ViewModels
{
    public class CategoryVM
    {
        public List<Category>? Categories { get; set; } // List of all categories
        public Category Category { get; set; } = new Category(); // for create / edit

        // Pagination properties
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }
}
