using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ECommerceCore.Domain.Entities.Common;

namespace ECommerceCore.Domain.Entities.Identity
{
    public class ApplicationUser : AppIdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }

        //[ProtectedPersonalData]
        //[MaxLength(256)]
        //public override string? Email { get; set; } // Use the base implementation

        //[ProtectedPersonalData]
        //[MaxLength(20)]
        //public override string? PhoneNumber { get; set; } // Use the base implementation

        [MaxLength(5)]
        public string? CountryCode { get; set; }

        //Adding Foregin Key relation
        public int? CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        [ValidateNever]
        public Company? Company { get; set; }

        [NotMapped]
        public string Role { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
