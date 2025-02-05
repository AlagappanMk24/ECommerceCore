using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Infrastructure.Data.Context;

namespace ECommerceCore.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private EcomDbContext _dbContext;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IUserRepository ApplicationUser { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IProductImageRepository ProductImage { get; private set; }
        public IContactUsRepository ContactUs { get; private set; }
        public UnitOfWork(EcomDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            Category = new CategoryRepository(_dbContext);
            Product = new ProductRepository(_dbContext);
            Company = new CompanyRepository(_dbContext);
            ShoppingCart = new ShoppingCartRepository(_dbContext);
            ApplicationUser = new UserRepository(_dbContext);
            OrderHeader = new OrderHeaderRepository(_dbContext);
            OrderDetail = new OrderDetailRepository(_dbContext);
            ProductImage = new ProductImageRepository(_dbContext);
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