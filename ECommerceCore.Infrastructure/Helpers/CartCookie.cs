using ECommerceCore.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ECommerceCore.Infrastructure.Utilities
{
    public static class CartCookie
    {
        private const string CartCookieName = "TempCart"; // Name of the cookie

        /// <summary>
        /// Helper method to retrieve the cart for anonymous users from cookies.
        /// </summary>
        public static List<ShoppingCart> GetCartFromCookie(HttpContext httpContext, ILogger logger)
        {
            try
            {
                var cartJson = httpContext?.Request.Cookies[CartCookieName];

                if (string.IsNullOrEmpty(cartJson))
                {
                    return new List<ShoppingCart>();
                }

                return JsonSerializer.Deserialize<List<ShoppingCart>>(cartJson) ?? new List<ShoppingCart>();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving the cart from cookies.");
                return new List<ShoppingCart>();
            }
        }

        /// <summary>
        /// Stores the cart in a cookie.
        /// </summary>
        public static void SetCartToCookie(HttpContext httpContext, List<ShoppingCart> cart, ILogger logger)
        {
            try
            {
                if (httpContext == null)
                {
                    logger.LogWarning("HttpContext is null. Unable to set cart cookies.");
                    return;
                }

                string cartJson = JsonSerializer.Serialize(cart);

                // Save the full cart as JSON
                httpContext.Response.Cookies.Append(CartCookieName, cartJson, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true
                });

                // Save total quantity separately for quick access
                int totalCartQuantity = cart.Sum(item => item.Count);
                httpContext.Response.Cookies.Append("CartCount", totalCartQuantity.ToString(), new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                    HttpOnly = true,
                    Secure = true,
                    IsEssential = true
                });

                logger.LogInformation("Cart successfully saved to cookies.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while saving the cart to cookies.");
            }
        }
    }
}
