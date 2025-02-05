using System.ComponentModel.DataAnnotations;

namespace ECommerceCore.Domain.Models.Entities
{
    public class WishlistItem : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public DateTime AddedDate { get; set; }

        // Navigation Properties
        public Product? Product { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
