using ECommerceCore.Application.Contract.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceCore.Web.Controllers
{
    public class WishlistController(IWishlistService wishlistService) : Controller
    {
        private readonly IWishlistService _wishlistService = wishlistService;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishlistItems = await _wishlistService.GetWishlistItemsAsync(currentUserId);

            // Select the products from the wishlist items
            var products = wishlistItems.Select(wi => wi.Product).ToList();

            return View(products);
        }

        [HttpPost]
        [Authorize] // Requires user to be logged in
        public async Task<IActionResult> AddToWishlist(int productId)
        {
            if (productId <= 0)
            {
                return BadRequest("Invalid product ID");
            }

            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID

            if (currentUserId == null) return Unauthorized();

            bool added = await _wishlistService.AddToWishlistAsync(productId, currentUserId);

            if (!added)
            {
                // Handle the case where the item is already in the wishlist
                TempData["Message"] = "Item is already in your wishlist.";
            }
            else
            {
                TempData["Message"] = "Item added to wishlist.";
            }

            return Json(new
            {
                success = added,
                message = added ? "Item added to wishlist." : "Item is already in your wishlist."
            });
        }
        public async Task<IActionResult> RemoveFromWishlist(int productId)
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool removed = await _wishlistService.RemoveFromWishlistAsync(productId, currentUserId);
            return Json(new
            {
                success = removed,
                message = removed ? "Product removed from your wishlist." : "Failed to remove product."
            });
        }
    }
}