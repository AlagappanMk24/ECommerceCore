using System.Security.Claims;

namespace ECommerceCore.Web.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this IHttpContextAccessor contextAccessor)
        {
            var userIdClaim = contextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            return userIdClaim == null ? throw new InvalidOperationException("User ID claim not found in the HTTP context.") : userIdClaim.Value;
        }
        public static List<string> GetUserRoles(this IHttpContextAccessor contextAccessor)
        {
            var roleClaims = contextAccessor.HttpContext?.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            return roleClaims ?? new List<string>();
        }
    }
}