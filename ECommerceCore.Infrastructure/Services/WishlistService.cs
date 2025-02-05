using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Domain.Models.Entities;
using ECommerceCore.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Services
{
    public class WishlistService(EcomDbContext context) : IWishlistService
    {
        private readonly EcomDbContext _context = context;

        /// <summary>
        /// Adds a product to the user's wishlist.
        /// </summary>
        /// <param name="productId">The ID of the product to add to the wishlist.</param>
        /// <param name="userId">The ID of the user who wants to add the product to their wishlist.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a boolean value
        /// indicating whether the product was successfully added to the wishlist.
        /// Returns false if the product is already in the wishlist.
        /// </returns>
        public async Task<bool> AddToWishlistAsync(int productId, string userId)
        {
            if (await _context.WishlistItems.AnyAsync(wi => wi.ProductId == productId && wi.UserId == userId))
            {
                return false; // Already in wishlist
            }

            var wishlistItem = new WishlistItem { ProductId = productId, UserId = userId, AddedDate = DateTime.Now };
            _context.WishlistItems.Add(wishlistItem);
            await _context.SaveChangesAsync();
            return true; // Successfully added
        }

        /// <summary>
        /// Retrieves the list of wishlist items for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose wishlist items are to be retrieved.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a list of 
        /// wishlist items associated with the specified user.
        /// </returns>
        public async Task<List<WishlistItem>> GetWishlistItemsAsync(string userId)
        {
            return await _context.WishlistItems
                .Include(wi => wi.Product) // Eager load the Product
                .ThenInclude(pi => pi.ProductImages)
                .Where(wi => wi.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Removes a product from the user's wishlist.
        /// </summary>
        /// <param name="productId">The ID of the product to remove from the wishlist.</param>
        /// <param name="userId">The ID of the user who wants to remove the product from their wishlist.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a boolean value
        /// indicating whether the product was successfully removed from the wishlist.
        /// Returns false if the product was not found in the wishlist.
        /// </returns>
        public async Task<bool> RemoveFromWishlistAsync(int productId, string userId)
        {
            var wishlistItem = await _context.WishlistItems
                .FirstOrDefaultAsync(wi => wi.ProductId == productId && wi.UserId == userId);

            if (wishlistItem == null)
            {
                return false; // Not found
            }

            _context.WishlistItems.Remove(wishlistItem);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
