using ECommerceCore.Application.Common.QueryParams;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contracts.ViewModels.Users
{
    public class UserIndexVM
    {
        public UserQueryParameters QueryParameters { get; set; }
        public List<CompanyDto> Companies { get; set; }
        public List<string> Roles { get; set; }
    }

    public class UserQueryParameters : QueryParameters
    {
        public int? CompanyId { get; set; }
        public string? Role { get; set; }
    }
}
