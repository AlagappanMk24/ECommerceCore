using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category obj);
    }
}
