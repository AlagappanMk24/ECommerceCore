using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);
    }
}
