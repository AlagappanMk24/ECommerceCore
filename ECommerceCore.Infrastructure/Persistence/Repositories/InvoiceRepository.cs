using ECommerceCore.Application.Contracts.Persistence;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Infrastructure.Data.Context;

namespace ECommerceCore.Infrastructure.Persistence.Repositories
{
    public class InvoiceRepository(EcomDbContext dbContext) : GenericRepository<Invoice>(dbContext), IInvoiceRepository
    {
        private EcomDbContext _dbContext = dbContext;
    }
}
