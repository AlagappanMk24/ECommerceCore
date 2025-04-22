using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contracts.Services
{
    public interface IAuthStateService
    {
        Task<AuthState> CreateAuthStateAsync(string userId);
        Task<AuthState> GetAuthStateAsync(string authStateId);
        Task UpdateAuthStateAsync(AuthState authState);
        Task CleanupExpiredStatesAsync();
    }
}
