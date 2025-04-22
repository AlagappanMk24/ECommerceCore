using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        void Update(Category obj);
    }
}
