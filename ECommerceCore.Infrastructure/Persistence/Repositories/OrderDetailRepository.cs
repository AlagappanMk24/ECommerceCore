using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Infrastructure.Data.Context;

namespace ECommerceCore.Infrastructure.Persistence.Repositories
{
    public class OrderDetailRepository(EcomDbContext dbContext) : GenericRepository<OrderDetail>(dbContext), IOrderDetailRepository
    {
        private EcomDbContext _dbContext = dbContext;

        public void Update(OrderDetail obj)
        {
            _dbContext.OrderDetails.Update(obj);
        }
    }
}