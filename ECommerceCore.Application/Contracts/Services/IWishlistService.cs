using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contract.Service
{
    public interface IWishlistService
    {
        Task<bool> AddToWishlistAsync(int productId, string userId);
        Task<bool> RemoveFromWishlistAsync(int productId, string userId);
        Task<List<WishlistItem>> GetWishlistItemsAsync(string userId);
    }
}
