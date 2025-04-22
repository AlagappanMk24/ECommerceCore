using ECommerceCore.Application.Contract.ViewModels;
using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contract.Service
{
    public interface ICartService
    {
        Task IncrementCartItem(int cartId, bool isAuthenticated, int productId);
        Task DecrementCartItem(int cartId, bool isAuthenticated, int productId);
        Task RemoveCartItem(int cartId, bool isAuthenticated, int productId);
        Task<ShoppingCartVM> GetCartDetails(string userId);
        Task<ShoppingCartVM> GetSummaryDetails(string userId);
        Task<IEnumerable<ShoppingCart>> GetUserCart(string userId);
        double GetPriceBasedOnQuantity(ShoppingCart cart);
    }
}
