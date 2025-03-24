using ECommerceCore.Domain.Models.Entities;
using System.Linq.Expressions;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        void Update(Product obj);
        Task<Product> GetAsync(Expression<Func<Product, bool>> filter);
    }
}
