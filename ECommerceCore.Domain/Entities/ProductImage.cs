using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ECommerceCore.Domain.Entities.Common;
using System.Text.Json.Serialization;

namespace ECommerceCore.Domain.Entities
{
    public class ProductImage : BaseEntity
    {
        [Required]
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [JsonIgnore]
        public Product Product { get; set; }
    }
}