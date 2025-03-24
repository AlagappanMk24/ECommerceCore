using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Infrastructure.Data.Context;

namespace ECommerceCore.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private EcomDbContext _dbContext;
        public ICategoryRepository Categories { get; private set; }
        public IProductRepository Products { get; private set; }
        public ICompanyRepository Companies { get; private set; }
        public IShoppingCartRepository ShoppingCarts { get; private set; }
        public IUserRepository ApplicationUsers { get; private set; }
        public IOrderHeaderRepository OrderHeaders { get; private set; }
        public IOrderDetailRepository OrderDetails { get; private set; }
        public IProductImageRepository ProductImages { get; private set; }
        public IContactUsRepository ContactUs { get; private set; }
        public UnitOfWork(EcomDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            Categories = new CategoryRepository(_dbContext);
            Products = new ProductRepository(_dbContext);
            Companies = new CompanyRepository(_dbContext);
            ShoppingCarts = new ShoppingCartRepository(_dbContext);
            ApplicationUsers = new UserRepository(_dbContext);
            OrderHeaders = new OrderHeaderRepository(_dbContext);
            OrderDetails = new OrderDetailRepository(_dbContext);
            ProductImages = new ProductImageRepository(_dbContext);
        }
        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        // Optionally implement IDisposable for proper resource management
        public void Dispose()
        {
            _dbContext?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}