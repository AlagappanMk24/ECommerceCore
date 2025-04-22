using ECommerceCore.Application.Contracts.Services;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerceCore.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly EcomDbContext _context;

        public JwtService(IConfiguration configuration, EcomDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public string GenerateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email ?? user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["JwtSettings:ExpireHours"])),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<AuthToken> StoreTokenAsync(string userId, string token)
        {
            var authToken = new AuthToken
            {
                UserId = userId,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"]))
            };

            await _context.AuthTokens.AddAsync(authToken);
            await _context.SaveChangesAsync();

            return authToken;
        }
        public async Task<bool> ValidateTokenAsync(string token)
        {
            await CleanupExpiredTokensAsync();

            return await _context.AuthTokens
                .AnyAsync(t => t.Token == token && !t.IsRevoked && t.ExpiresAt > DateTime.UtcNow);
        }
        public async Task CleanupExpiredTokensAsync()
        {
            var expiredTokens = _context.AuthTokens
                .Where(t => t.ExpiresAt < DateTime.UtcNow);

            _context.AuthTokens.RemoveRange(expiredTokens);
            await _context.SaveChangesAsync();
        }
        public async Task RevokeTokenAsync(string userId)
        {
            var userTokens = _context.AuthTokens.Where(t => t.UserId == userId && !t.IsRevoked);
            foreach (var token in userTokens)
            {
                token.IsRevoked = true; // Mark as revoked instead of deleting          
            }
            await _context.SaveChangesAsync();
        }
        public string GenerateSecretKey(int length = 32)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var byteArray = new byte[length];
                rng.GetBytes(byteArray);
                return Convert.ToBase64String(byteArray);
            }
        }
    }
}