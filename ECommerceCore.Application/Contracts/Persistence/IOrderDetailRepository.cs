using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
    {
        void Update(OrderDetail obj);
    }
}
