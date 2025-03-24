using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        void Update(Category obj);
    }
}
