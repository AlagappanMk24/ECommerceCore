using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {
        void Update(ProductImage obj);
    }
}
