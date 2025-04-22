using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ECommerceCore.Application.Contracts.DTOs
{
    public class CategoryDto
    {
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public string Name { get; set; } = string.Empty;

        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display order must be between 1-100")]
        public int DisplayOrder { get; set; }
    }
    public class CreateCategoryRequest
    {
        public CategoryDto CategoryDto { get; set; }
        public string CurrentUser { get; set; }
    }
}
