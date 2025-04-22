using ECommerceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Data.Seeders
{
    public static class DatabaseSeeder
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                // Top-level categories
                new Category { Id = 1, Name = "Electronics", DisplayOrder = 1, IsActive = true, Description = "Explore the latest gadgets and electronic devices." },
                new Category { Id = 2, Name = "Apparel", DisplayOrder = 2, IsActive = true, Description = "Discover stylish clothing and accessories for all occasions." },
                new Category { Id = 3, Name = "Home & Garden", DisplayOrder = 3, IsActive = true, Description = "Find everything you need for your home and outdoor spaces." },
                new Category { Id = 4, Name = "Books", DisplayOrder = 4, IsActive = true, Description = "Immerse yourself in captivating stories and knowledge." },
                new Category { Id = 5, Name = "Sports & Outdoors", DisplayOrder = 5, IsActive = true, Description = "Gear up for your active lifestyle and outdoor adventures." },
                new Category { Id = 6, Name = "Beauty & Personal Care", DisplayOrder = 6, IsActive = true, Description = "Enhance your natural beauty and well-being." },
                new Category { Id = 7, Name = "Toys & Games", DisplayOrder = 7, IsActive = true, Description = "Unleash fun and creativity for all ages." },

                // Subcategories under Electronics (ParentId = 1)
                new Category { Id = 8, Name = "Smartphones", DisplayOrder = 1, IsActive = true, ParentCategoryId = 1, Description = "The latest smartphones from top brands." },
                new Category { Id = 9, Name = "Laptops", DisplayOrder = 2, IsActive = true, ParentCategoryId = 1, Description = "Powerful laptops for work and play." },
                new Category { Id = 10, Name = "Televisions", DisplayOrder = 3, IsActive = true, ParentCategoryId = 1, Description = "High-definition televisions for home entertainment." },

                // Subcategories under Apparel (ParentId = 2)
                new Category { Id = 11, Name = "Menswear", DisplayOrder = 1, IsActive = true, ParentCategoryId = 2, Description = "Stylish clothing for men." },
                new Category { Id = 12, Name = "Womenswear", DisplayOrder = 2, IsActive = true, ParentCategoryId = 2, Description = "Trendy clothing for women." },

                // Subcategories under Home & Garden (ParentId = 3)
                new Category { Id = 13, Name = "Kitchen Appliances", DisplayOrder = 1, IsActive = true, ParentCategoryId = 3, Description = "Essential appliances for your kitchen." },
                new Category { Id = 14, Name = "Garden Tools", DisplayOrder = 2, IsActive = true, ParentCategoryId = 3, Description = "Tools for maintaining your garden." },

                // Subcategories under Books (ParentId = 4)
                new Category { Id = 15, Name = "Fiction", DisplayOrder = 1, IsActive = true, ParentCategoryId = 4, Description = "Imaginative and engaging fictional works." },
                new Category { Id = 16, Name = "Non-Fiction", DisplayOrder = 2, IsActive = true, ParentCategoryId = 4, Description = "Informative and factual books on various topics." },

                // Subcategories under Sports & Outdoors (ParentId = 5)
                new Category { Id = 17, Name = "Camping & Hiking", DisplayOrder = 1, IsActive = true, ParentCategoryId = 5, Description = "Gear for your outdoor adventures." },
                new Category { Id = 18, Name = "Fitness", DisplayOrder = 2, IsActive = true, ParentCategoryId = 5, Description = "Equipment and accessories for your fitness journey." }
            );

            // Seed Companies
            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, Name = "Tech Solutions Inc.", StreetAddress = "123 Innovation Way", City = "Silicon City", PostalCode = "94016", State = "CA", PhoneNumber = "555-123-4567" },
                new Company { Id = 2, Name = "Fashion Forward Ltd.", StreetAddress = "456 Style Avenue", City = "Fashionville", PostalCode = "10001", State = "NY", PhoneNumber = "212-987-6543" },
                new Company { Id = 3, Name = "Green Living Co.", StreetAddress = "789 Earth Street", City = "Eco City", PostalCode = "30303", State = "GA", PhoneNumber = "404-555-7890" },
                new Company { Id = 4, Name = "Global Reads", StreetAddress = "101 Literary Lane", City = "Booktown", PostalCode = "60602", State = "IL", PhoneNumber = "312-555-1122" },
                new Company { Id = 5, Name = "Adventure Gear Corp.", StreetAddress = "222 Trail Road", City = "Outdoorsville", PostalCode = "80202", State = "CO", PhoneNumber = "720-555-3344" },
                new Company { Id = 6, Name = "Glow & Glam", StreetAddress = "333 Radiant Road", City = "Cosmetic City", PostalCode = "90210", State = "CA", PhoneNumber = "310-555-0011" },
                new Company { Id = 7, Name = "Fun Time Toys", StreetAddress = "444 Playful Place", City = "Toyland", PostalCode = "11201", State = "NY", PhoneNumber = "718-555-9988" }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Smart TV 55 inch 4K UHD with HDR",
                    Author = "Tech Solutions Inc.",
                    Description = "Immerse yourself in stunning visuals with this 55-inch 4K Ultra HD Smart TV. Featuring High Dynamic Range (HDR) for vibrant colors and deep contrast, built-in Wi-Fi, and access to all your favorite streaming apps. Enjoy a cinematic experience in the comfort of your living room.",
                    ISBN = "ELC-SMTV-001",
                    ListPrice = 799.99,
                    Price = 699.99,
                    Price50 = 650.00,
                    Price100 = 600.00,
                    CategoryId = 1,
                    StockQuantity = 50
                },
                new Product
                {
                    Id = 2,
                    Title = "Premium Cotton T-Shirt - Mens (Navy Blue)",
                    Author = "Fashion Forward Ltd.",
                    Description = "Experience ultimate comfort with our premium 100% combed cotton men's t-shirt. Designed for a classic fit and exceptional softness, this navy blue tee is a versatile wardrobe staple perfect for everyday wear. Available in various sizes.",
                    ISBN = "APP-MTSRT-002-NVY",
                    ListPrice = 25.00,
                    Price = 20.00,
                    Price50 = 18.00,
                    Price100 = 15.00,
                    CategoryId = 2,
                    StockQuantity = 100
                },
                new Product
                {
                    Id = 3,
                    Title = "Essential Garden Tool Set (3-Piece with Wooden Handles)",
                    Author = "Green Living Co.",
                    Description = "Get your gardening tasks done with ease using our durable 3-piece garden tool set. Includes a sturdy trowel, hand fork, and cultivator, all featuring comfortable wooden handles for a secure grip. Perfect for both novice and experienced gardeners.",
                    ISBN = "HGN-TLSET-003-WD",
                    ListPrice = 49.95,
                    Price = 40.00,
                    Price50 = 35.00,
                    Price100 = 30.00,
                    CategoryId = 3,
                    StockQuantity = 30
                },
                new Product
                {
                    Id = 4,
                    Title = "The Mystery of the Emerald Tablet (A Detective Jane Doe Novel)",
                    Author = "A.B. Reader",
                    Description = "Dive into the latest thrilling adventure featuring Detective Jane Doe as she unravels the secrets behind an ancient emerald tablet linked to a series of perplexing disappearances. Packed with suspense, twists, and turns that will keep you guessing until the very last page.",
                    ISBN = "BOK-MYST-004-EMT",
                    ListPrice = 15.50,
                    Price = 12.00,
                    Price50 = 10.00,
                    Price100 = 9.00,
                    CategoryId = 4,
                    StockQuantity = 75
                },
                new Product
                {
                    Id = 5,
                    Title = "Adventure Pro 4-Person Waterproof Camping Tent",
                    Author = "Adventure Gear Corp.",
                    Description = "Embark on your next outdoor adventure with the Adventure Pro 4-person camping tent. Constructed with durable, waterproof materials and featuring a spacious interior, ventilation windows, and easy setup. Perfect for family camping, backpacking, and weekend getaways.",
                    ISBN = "SPT-CTNT-005-4P",
                    ListPrice = 199.00,
                    Price = 175.00,
                    Price50 = 160.00,
                    Price100 = 150.00,
                    CategoryId = 5,
                    StockQuantity = 25
                },
                new Product
                {
                    Id = 6,
                    Title = "Noise-Cancelling Wireless Bluetooth Headphones (Black)",
                    Author = "Tech Solutions Inc.",
                    Description = "Escape into your audio world with these premium noise-cancelling wireless Bluetooth headphones. Enjoy crystal-clear sound, deep bass, and up to 30 hours of playtime on a single charge. Features comfortable over-ear cups and intuitive controls. Ideal for travel, work, and relaxation.",
                    ISBN = "ELC-WHP-006-BLK",
                    ListPrice = 149.00,
                    Price = 129.00,
                    Price50 = 115.00,
                    Price100 = 100.00,
                    CategoryId = 1,
                    StockQuantity = 60
                },
                new Product
                {
                    Id = 7,
                    Title = "Performance Running Shoes - Womens (Aqua Blue)",
                    Author = "Fashion Forward Ltd.",
                    Description = "Achieve your personal best with these lightweight and supportive performance running shoes for women. Designed with breathable mesh and responsive cushioning for a comfortable and energized run. Perfect for тренировки on the track or hitting the pavement.",
                    ISBN = "APP-WRSHOE-007-ABL",
                    ListPrice = 89.99,
                    Price = 75.00,
                    Price50 = 70.00,
                    Price100 = 65.00,
                    CategoryId = 2,
                    StockQuantity = 90
                },
                new Product
                {
                    Id = 8,
                    Title = "3-Burner Propane BBQ Grill with Side Burner & Cover",
                    Author = "Green Living Co.",
                    Description = "Become the ultimate grill master with this versatile 3-burner propane BBQ grill. Features a convenient side burner for sauces and sides, a built-in thermometer for precise temperature control, and a durable weather-resistant cover to protect your investment.",
                    ISBN = "HGN-BBQ-008-PC",
                    ListPrice = 299.00,
                    Price = 250.00,
                    Price50 = 225.00,
                    Price100 = 200.00,
                    CategoryId = 3,
                    StockQuantity = 20
                },
                new Product
                {
                    Id = 9,
                    Title = "Timeless Tales of the Cosmos (A Science Fiction Anthology)",
                    Author = "Various Authors",
                    Description = "Embark on interstellar journeys with this captivating collection of classic science fiction short stories from visionary authors. Explore themes of space exploration, artificial intelligence, and the future of humanity in this must-have anthology for sci-fi enthusiasts.",
                    ISBN = "BOK-SCIFI-009-TTC",
                    ListPrice = 18.75,
                    Price = 15.00,
                    Price50 = 13.00,
                    Price100 = 11.00,
                    CategoryId = 4,
                    StockQuantity = 55
                },
                new Product
                {
                    Id = 10,
                    Title = "Summit X Full Suspension Mountain Bike (29 inch Wheels)",
                    Author = "Adventure Gear Corp.",
                    Description = "Conquer any trail with the Summit X full suspension mountain bike. Featuring lightweight aluminum frame, responsive suspension system, and 29-inch wheels for enhanced stability and control. Perfect for experienced riders seeking ultimate off-road performance.",
                    ISBN = "SPT-MTB-010-FS29",
                    ListPrice = 1200.00,
                    Price = 1050.00,
                    Price50 = 950.00,
                    Price100 = 850.00,
                    CategoryId = 5,
                    StockQuantity = 15
                },
                new Product
                {
                    Id = 11,
                    Title = "Hydrating Facial Serum with Hyaluronic Acid",
                    Author = "Glow & Glam",
                    Description = "Replenish and revitalize your skin with our hydrating facial serum. Formulated with pure hyaluronic acid to lock in moisture, reduce the appearance of fine lines, and leave your complexion feeling smooth, plump, and radiant. Suitable for all skin types.",
                    ISBN = "BPC-FSERUM-011",
                    ListPrice = 35.00,
                    Price = 28.00,
                    Price50 = 25.00,
                    Price100 = 22.00,
                    CategoryId = 6,
                    StockQuantity = 80
                },
                new Product
                {
                    Id = 12,
                    Title = "Deluxe Wooden Train Set (Over 100 Pieces)",
                    Author = "Fun Time Toys",
                    Description = "Spark your child's imagination with this deluxe wooden train set. Featuring over 100 pieces including tracks, trains, bridges, figures, and scenery. Crafted from high-quality wood for lasting durability and endless hours of creative play. Perfect for ages 3 and up.",
                    ISBN = "TOY-TRSET-012",
                    ListPrice = 79.99,
                    Price = 65.00,
                    Price50 = 60.00,
                    Price100 = 55.00,
                    CategoryId = 7,
                    StockQuantity = 40
                }
            );

            // Seed Product Images
            modelBuilder.Entity<ProductImage>().HasData(
               // Images for Smart TV (ProductId = 1)
               new ProductImage { Id = 1, ProductId = 1, ImageUrl = "/images/products/smarttv_main.jpg" },
               new ProductImage { Id = 2, ProductId = 1, ImageUrl = "/images/products/smarttv_ports.jpg" },

               // Images for Mens T-Shirt (ProductId = 2)
               new ProductImage { Id = 3, ProductId = 2, ImageUrl = "/images/products/mens_tshirt_navy.jpg" },
               new ProductImage { Id = 4, ProductId = 2, ImageUrl = "/images/products/mens_tshirt_front.jpg" },

               // Images for Garden Tool Set (ProductId = 3)
               new ProductImage { Id = 5, ProductId = 3, ImageUrl = "/images/products/garden_tool_set.jpg" },

               // Images for The Mystery of the Emerald Tablet (ProductId = 4)
               new ProductImage { Id = 6, ProductId = 4, ImageUrl = "/images/products/book_emerald_tablet.jpg" },

               // Images for Camping Tent (ProductId = 5)
               new ProductImage { Id = 7, ProductId = 5, ImageUrl = "/images/products/tent_pitched.jpg" },
               new ProductImage { Id = 8, ProductId = 5, ImageUrl = "/images/products/tent_interior.jpg" },

               // Images for Noise-Cancelling Headphones (ProductId = 6)
               new ProductImage { Id = 9, ProductId = 6, ImageUrl = "/images/products/headphones_black.jpg" },

               // Images for Womens Running Shoes (ProductId = 7)
               new ProductImage { Id = 10, ProductId = 7, ImageUrl = "/images/products/womens_run_shoe_blue.jpg" },

               // Images for BBQ Grill (ProductId = 8)
               new ProductImage { Id = 11, ProductId = 8, ImageUrl = "/images/products/bbq_grill.jpg" },

               // Images for Science Fiction Anthology (ProductId = 9)
               new ProductImage { Id = 12, ProductId = 9, ImageUrl = "/images/products/book_sci_fi_anthology.jpg" },

               // Images for Mountain Bike (ProductId = 10)
               new ProductImage { Id = 13, ProductId = 10, ImageUrl = "/images/products/mountain_bike.jpg" },

               // Images for Facial Serum (ProductId = 11)
               new ProductImage { Id = 14, ProductId = 11, ImageUrl = "/images/products/facial_serum.jpg" },

               // Images for Wooden Train Set (ProductId = 12)
               new ProductImage { Id = 15, ProductId = 12, ImageUrl = "/images/products/train_set_layout.jpg" },
               new ProductImage { Id = 16, ProductId = 12, ImageUrl = "/images/products/train_set_pieces.jpg" }
           );
        }
    }
}
