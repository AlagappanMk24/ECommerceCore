using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Domain.Models.Entities;
using ECommerceCore.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Repositories
{
    public class OrderHeaderRepository(EcomDbContext dbContext) : GenericRepository<OrderHeader>(dbContext), IOrderHeaderRepository
    {
        private EcomDbContext _dbContext = dbContext;
        public void Update(OrderHeader obj)
        {
            _dbContext.OrderHeaders.Update(obj);
        }
        public async Task UpdateStatusAsync(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = await _dbContext.OrderHeaders.FirstOrDefaultAsync(x => x.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }
        public async Task UpdateStripePaymentIdAsync(int id, string sessionId, string paymentIntentId)
        {
            var orderFromDb = await _dbContext.OrderHeaders.FirstOrDefaultAsync(x => x.Id == id);
            if (!string.IsNullOrEmpty(sessionId))
            {
                orderFromDb.SessionId = sessionId;
            }
            if (!string.IsNullOrEmpty(paymentIntentId))
            {
                orderFromDb.PaymentIntenId = paymentIntentId;
                orderFromDb.PaymentDate = DateTime.Now;
            }
        }
    }
}
