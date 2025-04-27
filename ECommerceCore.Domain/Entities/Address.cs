using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Domain.Entities
{
    [Owned]
    public class Address
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }

    [Owned]
    public class ShippingAddress
    {
        public string ShippingAddress1 { get; set; }
        public string? ShippingAddress2 { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingCountry { get; set; }
        public string ShippingZipCode { get; set; }
    }
}