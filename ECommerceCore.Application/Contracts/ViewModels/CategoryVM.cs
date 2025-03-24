using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Application.Contract.ViewModels
{
    public class CategoryVM
    {
        public List<Category>? Categories { get; set; } // List of all categories
        public Category? Category { get; set; } // For create/edit
    }
}
