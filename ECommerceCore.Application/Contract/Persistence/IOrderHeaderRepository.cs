using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
        Task UpdateStatusAsync(int id, string orderStatus, string? paymentStatus = null);
        Task UpdateStripePaymentIdAsync(int id, string sessionId, string paymentIntentId);
    }
}
