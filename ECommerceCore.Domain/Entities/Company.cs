using ECommerceCore.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace ECommerceCore.Domain.Entities
{
    public class Company : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }
        public ICollection<Location>? Locations { get; set; } 
    }
}