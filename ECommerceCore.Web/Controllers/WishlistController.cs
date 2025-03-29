using ECommerceCore.Application.Contract.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceCore.Web.Controllers
{
    public class WishlistController(IWishlistService wishlistService) : Controller
    {
        private readonly IWishlistService _wishlistService = wishlistService;

        /// <summary>
        /// Retrieves and displays the wishlist items for the currently logged-in user.
        /// </summary>
        /// <returns>An IActionResult representing the wishlist view with a list of products.</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wishlistItems = await _wishlistService.GetWishlistItemsAsync(currentUserId);

            // Select the products from the wishlist items
            var products = wishlistItems.Select(wi => wi.Product).ToList();

            return View(products);
        }

        /// <summary>
        /// Adds a product to the wishlist of the currently logged-in user.
        /// </summary>
        /// <param name="productId">The ID of the product to add to the wishlist.</param>
        /// <returns>A JSON IActionResult indicating the success or failure of the operation.</returns>
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

        /// <summary>
        /// Removes a product from the wishlist of the currently logged-in user.
        /// </summary>
        /// <param name="productId">The ID of the product to remove from the wishlist.</param>
        /// <returns>A JSON IActionResult indicating the success or failure of the removal operation.</returns>
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