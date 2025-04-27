using ECommerceCore.Application.Common.Results;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Application.Contracts.ViewModels.Users;
using ECommerceCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ECommerceCore.Application.Contract.Service
{
    public interface IUserService
    {
        Task<IEnumerable<Company>> GetAllCompanies();
        Task<ApplicationUser?> GetUserById(string id);
        Task<PaginatedResult<UserDto>> GetUsersPaginatedAsync(UserQueryParameters parameters);
        //Task<IdentityResult> DeleteUser(string id);
        string GetUserId(ClaimsPrincipal user);
        Task<ApplicationUser> GetApplicationUser(string userId);
        Task<bool> UpdateUserAsync(string userId, ApplicationUser updatedUser);
    }
}