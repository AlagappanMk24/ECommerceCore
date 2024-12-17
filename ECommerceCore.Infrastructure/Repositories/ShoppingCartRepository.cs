using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Domain.Models.Entities;
using ECommerceCore.Infrastructure.Data.Context;

namespace ECommerceCore.Infrastructure.Repositories
{
    public class ShoppingCartRepository(EcomDbContext dbContext) : Repository<ShoppingCart>(dbContext), IShoppingCartRepository
    {
        private EcomDbContext _dbContext = dbContext;

        public void Update(ShoppingCart obj)
        {
            _dbContext.ShoppingCarts.Update(obj);
        }
    }
}
