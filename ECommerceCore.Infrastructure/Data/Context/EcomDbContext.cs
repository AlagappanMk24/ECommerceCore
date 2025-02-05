using ECommerceCore.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Data.Context
{
    public class EcomDbContext : IdentityDbContext<IdentityUser>
    {
        public EcomDbContext(DbContextOptions<EcomDbContext> options) : base(options)
        {

        }
        //To Add Dbset
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<ContactUs> ContactUsSubmissions { get; set; } 

        //To do seeding or to add data in db
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }
                );
            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, Name = "Tech Solution", StreetAddress = "123 Tech st", City = "Tech City", PostalCode = "12121", State = "IL", PhoneNumber = "7887482473" },
                new Company { Id = 2, Name = "Vivid Books", StreetAddress = "999 vid st", City = "Vid City", PostalCode = "66666", State = "IL", PhoneNumber = "7887482470" },
                new Company { Id = 3, Name = "Readers Club", StreetAddress = "999 Main st", City = "Lala land", PostalCode = "99999", State = "NY", PhoneNumber = "7887482472" }
                );
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Fortune of Time",
                    Author = "Billy Spark",
                    Description = "A thrilling race against time! When an ancient artifact surfaces, a group of daring adventurers must decipher its secrets before it falls into the wrong hands. Explosions, chases, and close calls abound in this action-packed adventure.",
                    ISBN = "SWD9999001",
                    ListPrice = 99,
                    Price = 90,
                    Price50 = 85,
                    Price100 = 80,
                    CategoryId = 1

                },
                new Product
                {
                    Id = 2,
                    Title = "Dark Skies",
                    Author = "Nancy Hoover",
                    Description = "In the year 2342, humanity has colonized the outer reaches of the solar system. But a mysterious force is picking off outposts one by one. A lone pilot must uncover the truth behind the 'Dark Skies' before it's too late for humanity.",
                    ISBN = "CAW777777701",
                    ListPrice = 40,
                    Price = 30,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 2

                },
                new Product
                {
                    Id = 3,
                    Title = "Vanish in the Sunset",
                    Author = "Julian Button",
                    Description = "The year is 1888. A renowned historian vanishes without a trace during an expedition to the American West. His journal, filled with cryptic clues about a hidden Native American tribe, is the only lead. Follow the trail of mystery and discover the secrets lost to time.",
                    ISBN = "RITO5555501",
                    ListPrice = 55,
                    Price = 50,
                    Price50 = 40,
                    Price100 = 35,
                    CategoryId = 3

                },
                new Product
                {
                    Id = 4,
                    Title = "Cotton Candy",
                    Author = "Abby Muscles",
                    Description = "A heartwarming science fiction story about a young girl who discovers a small, fluffy alien creature in her backyard. This gentle tale explores themes of friendship, acceptance, and the wonders of the universe.",
                    ISBN = "WS3333333301",
                    ListPrice = 70,
                    Price = 65,
                    Price50 = 60,
                    Price100 = 55,
                    CategoryId = 2

                },
                new Product
                {
                    Id = 5,
                    Title = "Rock in the Ocean",
                    Author = "Ron Parker",
                    Description = "A high-octane action thriller set on a remote island fortress. A team of elite mercenaries is hired to infiltrate the heavily guarded complex and retrieve a valuable asset. Prepare for intense combat, daring escapes, and unexpected twists.",
                    ISBN = "SOTJ1111111101",
                    ListPrice = 30,
                    Price = 27,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 1

                },
                new Product
                {
                    Id = 6,
                    Title = "Leaves and Wonders",
                    Author = "Laura Phantom",
                    Description = "Step back in time to the ancient forests of Europe. This historical fiction novel follows the lives of a Celtic tribe as they face Roman invasion and the changing tides of history. Explore the rich culture, myths, and struggles of a forgotten era.",
                    ISBN = "FOT000000001",
                    ListPrice = 25,
                    Price = 23,
                    Price50 = 22,
                    Price100 = 20,
                    CategoryId = 3
                }
            );
        }
    }
}