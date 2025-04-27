using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Infrastructure.Data.Seeders;

namespace ECommerceCore.Infrastructure.Data.Context
{
    public class EcomDbContext(DbContextOptions<EcomDbContext> options) : IdentityDbContext<IdentityUser>(options)
    {
        // DbSet properties
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<InvoiceAttachments> InvoiceAttachments { get; set; }
        public DbSet<TaxDetail> TaxDetails { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Timezone> Timezones { get; set; }
        public DbSet<ContactUs> ContactUsSubmissions { get; set; } 
        public DbSet<AuthState> AuthStates { get; set; }
        public DbSet<AuthToken> AuthTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations from separate configuration classes
            // This single line will pick up all IEntityTypeConfiguration implementations
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EcomDbContext).Assembly);

            // Call the seeder
            DatabaseSeeder.SeedData(modelBuilder);

        }
    }
}