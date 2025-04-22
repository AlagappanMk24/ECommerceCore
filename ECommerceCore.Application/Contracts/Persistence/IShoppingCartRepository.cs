using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface IShoppingCartRepository : IGenericRepository<ShoppingCart>
    {
        void Update(ShoppingCart obj);
    }
}
