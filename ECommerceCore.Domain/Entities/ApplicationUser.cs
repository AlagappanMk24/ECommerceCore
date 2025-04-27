using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Identity;
using ECommerceCore.Domain.ValueObjects;
using ECommerceCore.Domain.Exceptions;

namespace ECommerceCore.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }

        private string? _email;
        private string? _phoneNumber;
        private string? _countryCode;

        [ProtectedPersonalData]
        [MaxLength(256)]
        public override string? Email
        {
            get => _email;
            set
            {
                try
                {
                    _email = value != null ? new Email(value).Value : null;
                }
                catch (InvalidEmailException)
                {
                    // If validation fails, keep the raw value
                    // The ModelState error will prevent saving invalid emails
                    _email = value;
                }
            }
        }

        [ProtectedPersonalData]
        [MaxLength(20)]
        public override string? PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                try
                {
                    if (value != null && !string.IsNullOrEmpty(_countryCode))
                    {
                        _phoneNumber = new PhoneNumber(value, _countryCode).Value;
                    }
                    else if (value != null)
                    {
                        // Fallback to auto-detection if country code isn't set
                        _phoneNumber = value;
                    }
                    else
                    {
                        _phoneNumber = null;
                    }
                }
                catch (InvalidPhoneException)
                {
                    // If validation fails, keep the raw value
                    // The ModelState error will prevent saving invalid numbers
                    _phoneNumber = value;
                }
            }
        }

        [NotMapped]
        [ValidateNever]
        public string? CountryCode
        {
            get => _countryCode;
            set => _countryCode = value;
        }

        //Adding Foregin Key relation
        public int? CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        [ValidateNever]
        public Company? Company { get; set; }

        [NotMapped]
        public string Role { get; set; }
    }
}
