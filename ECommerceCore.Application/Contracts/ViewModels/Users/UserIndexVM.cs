using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contracts.ViewModels.Users
{
    public class UserIndexVM
    {
        public UserQueryParameters QueryParameters { get; set; }
        public List<Company> Companies { get; set; }
    }

    public class UserQueryParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public int? CompanyId { get; set; }
        public string? Role { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; }
    }
}
