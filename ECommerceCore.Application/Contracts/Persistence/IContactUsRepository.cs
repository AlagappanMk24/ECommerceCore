using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface IContactUsRepository : IGenericRepository<ContactUs>
    {
        Task AddAsync(ContactUs contactUs);
    }
}
