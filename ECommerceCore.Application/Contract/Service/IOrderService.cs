using ECommerceCore.Application.Contract.ViewModels;
using ECommerceCore.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ECommerceCore.Application.Contract.Service
{
    public interface IOrderService
    {
        Task<OrderVM> GetOrderDetailsAsync(int orderId);
        Task UpdateOrderDetailsAsync(OrderHeader orderHeader);
        Task UpdateOrderStatusAsync(int orderId, string status);
        Task ShipOrderAsync(int orderId, string carrier, string trackingNumber);
        Task CancelOrderAsync(int orderId);
        Task<IEnumerable<OrderHeader>> GetOrdersByStatusAsync(ClaimsPrincipal user, string status);
        OrderHeader InitializeOrderHeader(string userId);
        void CalculateOrderTotal(ShoppingCartVM cartVM);
        void SetOrderStatus(ApplicationUser user, OrderHeader orderHeader);
        Task CreateOrder(ShoppingCartVM cartVM);
        Task UpdateStripePaymentDetails(int orderId, string sessionId, string paymentIntentId);
        Task HandleOrderConfirmation(int id, HttpContext httpContext);
    }
}
