using ECommerceCore.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace ECommerceCore.Domain.Entities
{
    public class Category : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public int? ParentCategoryId { get; set; }

        // Navigation property for the parent category
        public Category? ParentCategory { get; set; }
    }
}