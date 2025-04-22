using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Infrastructure.Data.Context;

namespace ECommerceCore.Infrastructure.Persistence.Repositories
{
    public class ContactUsRepository(EcomDbContext dbContext) : GenericRepository<ContactUs>(dbContext), IContactUsRepository
    {
        private readonly EcomDbContext _dbContext = dbContext;
        public async Task AddAsync(ContactUs contactUs)
        {
            await _dbContext.ContactUsSubmissions.AddAsync(contactUs);
        }
    }
}