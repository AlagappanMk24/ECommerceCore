using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ECommerceCore.Domain.Models.Entities
{
    public class Category : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display order must be between 1-100")]
        public int DisplayOrder { get; set; }
    }
}
