using System.Security.Claims;

namespace ECommerceCore.Web.Extensions
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Retrieves the user's ID from the HTTP context's claims.
        /// </summary>
        /// <param name="contextAccessor">The IHttpContextAccessor instance.</param>
        /// <returns>The user's ID as a string.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the user ID claim is not found in the HTTP context.</exception>
        public static string GetUserId(this IHttpContextAccessor contextAccessor)
        {
            var userIdClaim = contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            return userIdClaim == null ? throw new InvalidOperationException("User ID claim not found in the HTTP context.") : userIdClaim.Value;
        }

        /// <summary>
        /// Retrieves the user's roles from the HTTP context's claims.
        /// </summary>
        /// <param name="contextAccessor">The IHttpContextAccessor instance.</param>
        /// <returns>A list of user roles as strings.</returns>
        public static List<string> GetUserRoles(this IHttpContextAccessor contextAccessor)
        {
            var roleClaims = contextAccessor.HttpContext?.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            return roleClaims ?? new List<string>();
        }
    }
}