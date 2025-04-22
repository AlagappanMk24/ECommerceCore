using ECommerceCore.Application.Contracts.Services;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Services
{
    public class AuthStateService(EcomDbContext context) : IAuthStateService
    {
        private readonly EcomDbContext _context = context;

        public async Task<AuthState> CreateAuthStateAsync(string userId)
        {
            var authState = new AuthState
            {
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };

            await _context.AuthStates.AddAsync(authState);
            await _context.SaveChangesAsync();

            return authState;
        }

        public async Task<AuthState> GetAuthStateAsync(string authStateId)
        {
            await CleanupExpiredStatesAsync();
            return await _context.AuthStates.FindAsync(authStateId);
        }

        public async Task UpdateAuthStateAsync(AuthState authState)
        {
            _context.AuthStates.Update(authState);
            await _context.SaveChangesAsync();
        }

        public async Task CleanupExpiredStatesAsync()
        {
            var expiredStates = await _context.AuthStates
         .Where(x => x.ExpiresAt < DateTime.UtcNow && x.UserId == null)
         .ToListAsync();

            _context.AuthStates.RemoveRange(expiredStates);
            await _context.SaveChangesAsync();
        }
    }
}
