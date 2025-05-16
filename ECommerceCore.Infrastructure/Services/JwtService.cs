using ECommerceCore.Application.Contracts.Services;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Domain.Entities.Identity;
using ECommerceCore.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerceCore.Infrastructure.Services
{
    public class JwtService(IConfiguration configuration, EcomDbContext context, ILogger<JwtService> logger) : IJwtService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly EcomDbContext _context = context;
        private readonly ILogger<JwtService> _logger = logger;
        public string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[]
            //    {
            //        new Claim(ClaimTypes.NameIdentifier, user.Id),
            //        new Claim(ClaimTypes.Email, user.Email ?? user.UserName),
            //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            //    }),
            //    Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["JwtSettings:ExpireHours"])),
            //    SigningCredentials = new SigningCredentials(
            //        new SymmetricSecurityKey(key),
            //        SecurityAlgorithms.HmacSha256Signature)
            //};

            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //return tokenHandler.WriteToken(token);
        }
        public async Task StoreTokenAsync(string userId, string token)
        {
            try
            {
                var authToken = new AuthToken
                {
                    UserId = userId,
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["JwtSettings:ExpireHours"]))
                };

                await _context.AuthTokens.AddAsync(authToken);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Stored JWT token for user {UserId}", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error storing JWT token for user {UserId}", userId);
                throw;
            }
        }
        public async Task<bool> ValidateTokenAsync(string token)
        {
            await CleanupExpiredTokensAsync();

            return await _context.AuthTokens
                .AnyAsync(t => t.Token == token && !t.IsRevoked && t.ExpiresAt > DateTime.UtcNow);
        }
        public async Task CleanupExpiredTokensAsync()
        {
            try
            {
                var expiredTokens = await _context.AuthTokens
                    .Where(t => t.ExpiresAt < DateTime.UtcNow)
                    .ToListAsync();

                _context.AuthTokens.RemoveRange(expiredTokens);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cleaned up {Count} expired tokens", expiredTokens.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cleaning up expired tokens");
                throw;
            }
        }
        public async Task RevokeTokenAsync(string userId)
        {
            try
            {
                var tokens = await _context.AuthTokens
                    .Where(t => t.UserId == userId && !t.IsRevoked)
                    .ToListAsync();

                foreach (var token in tokens)
                {
                    token.IsRevoked = true;
                    token.RevokedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Revoked {Count} tokens for user {UserId}", tokens.Count, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking tokens for user {UserId}", userId);
                throw;
            }
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