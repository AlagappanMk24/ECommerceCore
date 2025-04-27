using ECommerceCore.Application.Common.Results;
using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Application.Contracts.ViewModels.Users;
using ECommerceCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace ECommerceCore.Infrastructure.Services
{
    public class UserService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ILogger<UserService> logger) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<UserService> _logger = logger;

        public async Task<IEnumerable<Company>> GetAllCompanies()
        {
            return await _unitOfWork.Companies.GetAllAsync();
        }
        public async Task<ApplicationUser?> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
        public async Task<PaginatedResult<UserDto>> GetUsersPaginatedAsync(UserQueryParameters parameters)
        {
            try
            {
                // Base query - includes users from UserManager
                var usersQuery = _userManager.Users
                    .Include(u => u.Company)
                    .AsQueryable();

                // Apply search filter
                if (!string.IsNullOrEmpty(parameters.SearchTerm))
                {
                    string searchTerm = parameters.SearchTerm.ToLower().Trim();
                    usersQuery = usersQuery.Where(u =>
                        u.Name.ToLower().Contains(searchTerm) ||
                        u.Email.ToLower().Contains(searchTerm) ||
                        u.PhoneNumber.Contains(searchTerm)
                    );
                }

                // Apply company filter
                if (parameters.CompanyId.HasValue)
                {
                    usersQuery = usersQuery.Where(u => u.CompanyId == parameters.CompanyId.Value);
                }

                // Apply role filter
                if (!string.IsNullOrEmpty(parameters.Role))
                {
                    // This requires handling differently since roles aren't directly in the User table
                    // We'll get all users in the role first
                    var usersInRole = await _userManager.GetUsersInRoleAsync(parameters.Role);
                    var userIdsInRole = usersInRole.Select(u => u.Id);
                    usersQuery = usersQuery.Where(u => userIdsInRole.Contains(u.Id));
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(parameters.SortColumn))
                {
                    usersQuery = parameters.SortColumn.ToLower() switch
                    {
                        "name" => parameters.SortDirection == "asc" ?
                            usersQuery.OrderBy(u => u.Name) : usersQuery.OrderByDescending(u => u.Name),
                        "email" => parameters.SortDirection == "asc" ?
                            usersQuery.OrderBy(u => u.Email) : usersQuery.OrderByDescending(u => u.Email),
                        "company" => parameters.SortDirection == "asc" ?
                            usersQuery.OrderBy(u => u.Company.Name) : usersQuery.OrderByDescending(u => u.Company.Name),
                        _ => usersQuery.OrderBy(u => u.Name)
                    };
                }
                else
                {
                    // Default sort
                    usersQuery = usersQuery.OrderBy(u => u.Name);
                }

                // Get total count before pagination
                var totalCount = await usersQuery.CountAsync();

                // Apply pagination
                var users = await usersQuery
                    .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                    .Take(parameters.PageSize)
                    .ToListAsync();

                // Create DTOs with roles
                var userDtos = new List<UserDto>();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    userDtos.Add(new UserDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        CompanyId = user.CompanyId,
                        CompanyName = user.Company?.Name,
                        Role = roles.FirstOrDefault() ?? string.Empty,
                        Address = string.IsNullOrEmpty(user.Address1) ? string.Empty :
                            $"{user.Address1}, {user.City}, {user.State} {user.PostalCode}"
                    });
                }

                return new PaginatedResult<UserDto>
                {
                    Items = userDtos,
                    TotalCount = totalCount,
                    PageNumber = parameters.PageNumber,
                    PageSize = parameters.PageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUsersPaginatedAsync");
                throw;
            }
        }
        public string GetUserId(ClaimsPrincipal user)
        {
            var claimsIdentity = (ClaimsIdentity)user.Identity;
            return claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        public async Task<ApplicationUser> GetApplicationUser(string userId)
        {
            return await _unitOfWork.ApplicationUsers.GetAsync(u => u.Id == userId);
        }
        public async Task<bool> UpdateUserAsync(string userId, ApplicationUser updatedUser)
        {
            try
            {
                // Fetch the existing user
                var existingUser = await GetApplicationUser(userId);
                if (existingUser == null)
                {
                    return false;
                }

                // Update the user properties from the provided model
                existingUser.Name = updatedUser.Name;
                existingUser.Email = updatedUser.Email;
                existingUser.PhoneNumber = updatedUser.PhoneNumber;
                existingUser.Address1 = updatedUser.Address1;
                existingUser.Address2 = updatedUser.Address2;
                existingUser.City = updatedUser.City;
                existingUser.State = updatedUser.State;
                existingUser.CountryCode = updatedUser.CountryCode;
                existingUser.PostalCode = updatedUser.PostalCode;

                // Save changes to the database
                bool isUpdated = await _unitOfWork.ApplicationUsers.UpdateUserAsync(existingUser);
                return isUpdated;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

///<summary>
/// The `UserService` class provides utility functions for managing and retrieving user-related data.
/// 
/// Key Functions:
/// 1. `GetUserId` - Extracts the user's unique identifier from the provided `ClaimsPrincipal` object.
///    - Parameters: 
///      - `user`: The authenticated user's claims.
///    - Returns: The user's ID as a string.
/// 
/// 2. `GetApplicationUser` - Retrieves an application user by their ID from the database.
///    - Parameters:
///      - `userId`: The unique ID of the user.
///    - Returns: An `ApplicationUser` object representing the user.
/// 
/// Dependencies:
/// - `IUnitOfWork`: Provides access to the `ApplicationUser` repository.
/// 
/// These methods simplify the process of identifying and accessing user details in a secure and efficient manner.
///</summary>
