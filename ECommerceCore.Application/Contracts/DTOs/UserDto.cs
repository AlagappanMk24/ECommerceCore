namespace ECommerceCore.Application.Contracts.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string Role { get; set; }
        public string Address { get; set; }
    }
}
