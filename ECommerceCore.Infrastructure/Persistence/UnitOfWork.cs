using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contracts.Persistence;
using ECommerceCore.Infrastructure.Data.Context;
using ECommerceCore.Infrastructure.Persistence.Repositories;

namespace ECommerceCore.Infrastructure.Persistence
{
    public class UnitOfWork(EcomDbContext dbContext) : IUnitOfWork
    {
        private EcomDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        public ICategoryRepository Categories => new CategoryRepository(_dbContext);
        public IBrandRepository Brands => new BrandRepository(_dbContext);
        public IProductRepository Products => new ProductRepository(_dbContext);
        public ICompanyRepository Companies => new CompanyRepository(_dbContext);
        public IShoppingCartRepository ShoppingCarts => new ShoppingCartRepository(_dbContext);
        public IUserRepository ApplicationUsers => new UserRepository(_dbContext);
        public IOrderHeaderRepository OrderHeaders => new OrderHeaderRepository(_dbContext);
        public IOrderDetailRepository OrderDetails => new OrderDetailRepository(_dbContext);
        public IProductImageRepository ProductImages => new ProductImageRepository(_dbContext);
        public IInvoiceRepository Invoices => new InvoiceRepository(_dbContext);
        public ICustomerRepository Customers => new CustomerRepository(_dbContext);
        public IContactUsRepository ContactUs => new ContactUsRepository(_dbContext);
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        // Implement IDisposable for proper resource management
        public void Dispose()
        {
            _dbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}