using ECommerceCore.Domain.Entities;
using System.Security.Claims;

namespace ECommerceCore.Application.Contract.Service
{
    public interface IUserService
    {
        string GetUserId(ClaimsPrincipal user);
        Task<ApplicationUser> GetApplicationUser(string userId);
        Task<bool> UpdateUserAsync(string userId, ApplicationUser updatedUser);
    }
}