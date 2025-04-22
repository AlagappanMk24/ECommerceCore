using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ECommerceCore.Domain.Entities.Common;

namespace ECommerceCore.Domain.Entities
{
    public class OrderHeader : BaseEntity
    {
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public double OrderTotal { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateOnly PaymentDueDate { get; set; } //if we are using .net 8 then only use DateOnly otherwise use DateTime
        public string? SessionId { get; set; }
        public string? PaymentIntenId { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Name { get; set; }

        [ValidateNever]
        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
