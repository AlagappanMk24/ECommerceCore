using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ECommerceCore.Domain.Entities.Common;

namespace ECommerceCore.Domain.Entities
{
    public class ProductSpecification : BaseEntity
    {
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

        [Required]
        public string Key { get; set; } // e.g., "RAM", "Color"

        [Required]
        public string Value { get; set; } // e.g., "8GB", "Black"
    }
}
