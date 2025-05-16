using System.Security.Claims;

namespace ECommerceCore.Web.Extensions
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Retrieves the user's ID from the HTTP context's claims.
        /// </summary>
        /// <param name="contextAccessor">The IHttpContextAccessor instance.</param>
        /// <returns>The user's ID as a string, or null if not found.</returns>
        public static string? GetCurrentUserId(this IHttpContextAccessor contextAccessor)
        {
            var userId = contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId; // Return null if not found; let the caller handle it
        }

        /// <summary>
        /// Retrieves the user's roles from the HTTP context's claims.
        /// </summary>
        /// <param name="contextAccessor">The IHttpContextAccessor instance.</param>
        /// <returns>A list of user roles as strings.</returns>
        public static List<string> GetUserRoles(this IHttpContextAccessor contextAccessor)
        {
            var roleClaims = contextAccessor.HttpContext?.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            return roleClaims ?? new List<string>();
        }
    }
}