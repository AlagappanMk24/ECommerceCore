using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Domain.Models.Entities;
using ECommerceCore.Infrastructure.Data.Context;

namespace ECommerceCore.Infrastructure.Repositories
{
    public class ContactUsRepository(EcomDbContext dbContext) : Repository<ContactUs>(dbContext), IContactUsRepository
    {
        private readonly EcomDbContext _dbContext = dbContext;
        public async Task AddAsync(ContactUs contactUs)
        {
            await _dbContext.ContactUsSubmissions.AddAsync(contactUs);
        }
    }
}
