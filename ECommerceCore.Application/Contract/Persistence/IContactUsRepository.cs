using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Application.Contract.Persistence
{
    public interface IContactUsRepository : IRepository<ContactUs>
    {
        Task AddAsync(ContactUs contactUs);
    }
}
