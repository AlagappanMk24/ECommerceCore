using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ECommerceCore.Infrastructure.Services
{
    public class UserService(IUnitOfWork unitOfWork) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
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
                existingUser.StreetAddress = updatedUser.StreetAddress;
                existingUser.City = updatedUser.City;
                existingUser.State = updatedUser.State;
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
