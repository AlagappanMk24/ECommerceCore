using ECommerceCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Stripe;

namespace ECommerceCore.Application.Contracts.Services
{
    public interface IJwtService
    {
        string GenerateSecretKey(int length = 32);
        string GenerateJwtToken(IdentityUser user);
        Task StoreTokenAsync(string userId, string token);
        Task<bool> ValidateTokenAsync(string token);
        Task CleanupExpiredTokensAsync();
        Task RevokeTokenAsync(string userId);
    }
}
