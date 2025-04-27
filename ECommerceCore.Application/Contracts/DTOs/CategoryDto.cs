using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ECommerceCore.Application.Contracts.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
    }
    public class CreateCategoryRequest
    {
        public CategoryDto CategoryDto { get; set; }
        public string CurrentUser { get; set; }
    }
}
