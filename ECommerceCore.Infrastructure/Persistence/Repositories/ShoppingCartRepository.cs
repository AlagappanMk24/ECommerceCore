using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Infrastructure.Data.Context;

namespace ECommerceCore.Infrastructure.Persistence.Repositories
{
    public class ShoppingCartRepository(EcomDbContext dbContext) : GenericRepository<ShoppingCart>(dbContext), IShoppingCartRepository
    {
        private EcomDbContext _dbContext = dbContext;

        public void Update(ShoppingCart obj)
        {
            _dbContext.ShoppingCarts.Update(obj);
        }
    }
}
