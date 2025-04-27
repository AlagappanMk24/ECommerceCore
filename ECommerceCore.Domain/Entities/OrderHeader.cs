using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ECommerceCore.Domain.Entities.Common;

namespace ECommerceCore.Domain.Entities
{
    public class OrderHeader : BaseEntity
    {
        public string? ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        [ValidateNever]
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public double OrderTotal { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateOnly PaymentDueDate { get; set; } 
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
        public ShippingAddress ShipToAddress { get; set; }
        public string ShippingContactName { get; set; }
        public string ShippingContactPhone { get; set; }

        [ValidateNever]
        public ICollection<OrderDetail>? OrderDetails { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }
    }
}
