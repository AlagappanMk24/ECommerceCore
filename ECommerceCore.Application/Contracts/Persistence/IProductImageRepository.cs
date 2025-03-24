using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface IProductImageRepository : IGenericRepository<ProductImage>
    {
        void Update(ProductImage obj);
    }
}
