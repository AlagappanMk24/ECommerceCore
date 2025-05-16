using ECommerceCore.Application.Common.Results;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Application.Contracts.ViewModels.Users;
using ECommerceCore.Domain.Entities.Identity;
using System.Security.Claims;

namespace ECommerceCore.Application.Contract.Service
{
    public interface IUserService
    {
        Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync();
        Task<PaginatedResult<UserDto>> GetUsersPaginatedAsync(UserQueryParameters parameters);
        Task<UserDto> GetUserByIdAsync(string id);
        Task<OperationResult<string>> CreateUserAsync(UserDto userDto, string currentUserId);
        Task<OperationResult<string>> UpdateUserAsync(UserDto userDto, string currentUserId);
        Task<OperationResult<string>> DeleteUserAsync(string userId, string currentUserId);
        string GetUserId(ClaimsPrincipal user);
        Task<ApplicationUser> GetApplicationUser(string userId);
        Task<UserUpsertVM> CreateUserUpsertVMAsync(UserDto userDto);
        Task<OperationResult<string>> ReleaseEmailAsync(string userId);
        Task CleanupSoftDeletedUsersAsync();
    }
}