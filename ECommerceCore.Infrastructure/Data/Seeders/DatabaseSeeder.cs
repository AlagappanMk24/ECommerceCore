using ECommerceCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Data.Seeders
{
    public static class DatabaseSeeder
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Brands
            modelBuilder.Entity<Brand>().HasData(
                new Brand
                {
                    Id = 1,
                    Name = "Tech Solutions",
                    Slug = "tech-solutions",
                    Description = "Innovative technology solutions for everyday life.",
                    WebsiteUrl = "https://www.techsolutions.com",
                    LogoUrl = "/images/brands/tech-solutions-logo.png",
                    Country = "United States",
                    EstablishedYear = 2005,
                    IsActive = true
                },
                new Brand
                {
                    Id = 2,
                    Name = "Fashion Forward",
                    Slug = "fashion-forward",
                    Description = "Trendy and comfortable clothing for all occasions.",
                    WebsiteUrl = "https://www.fashionforward.com",
                    LogoUrl = "/images/brands/fashion-forward-logo.png",
                    Country = "France",
                    EstablishedYear = 2010,
                    IsActive = true
                },
                new Brand
                {
                    Id = 3,
                    Name = "Green Living",
                    Slug = "green-living",
                    Description = "Eco-friendly products for sustainable living.",
                    WebsiteUrl = "https://www.greenliving.com",
                    LogoUrl = "/images/brands/green-living-logo.png",
                    Country = "Canada",
                    EstablishedYear = 2015,
                    IsActive = true
                },
                new Brand
                {
                    Id = 4,
                    Name = "Global Reads",
                    Slug = "global-reads",
                    Description = "Quality books from authors around the world.",
                    WebsiteUrl = "https://www.globalreads.com",
                    LogoUrl = "/images/brands/global-reads-logo.png",
                    Country = "United Kingdom",
                    EstablishedYear = 1995,
                    IsActive = true
                },
                new Brand
                {
                    Id = 5,
                    Name = "Adventure Gear",
                    Slug = "adventure-gear",
                    Description = "High-quality equipment for outdoor enthusiasts.",
                    WebsiteUrl = "https://www.adventuregear.com",
                    LogoUrl = "/images/brands/adventure-gear-logo.png",
                    Country = "Australia",
                    EstablishedYear = 2008,
                    IsActive = true
                },
                new Brand
                {
                    Id = 6,
                    Name = "Glow & Glam",
                    Slug = "glow-and-glam",
                    Description = "Premium beauty products for radiant skin.",
                    WebsiteUrl = "https://www.glowandglam.com",
                    LogoUrl = "/images/brands/glow-and-glam-logo.png",
                    Country = "South Korea",
                    EstablishedYear = 2012,
                    IsActive = true
                },
                new Brand
                {
                    Id = 7,
                    Name = "Fun Time Toys",
                    Slug = "fun-time-toys",
                    Description = "Creative and educational toys for children of all ages.",
                    WebsiteUrl = "https://www.funtimetoys.com",
                    LogoUrl = "/images/brands/fun-time-toys-logo.png",
                    Country = "Germany",
                    EstablishedYear = 2000,
                    IsActive = true
                }
            );

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
                    Description = "Immerse yourself in stunning visuals with this 55-inch 4K Ultra HD Smart TV. Featuring High Dynamic Range (HDR) for vibrant colors and deep contrast, built-in Wi-Fi, and access to all your favorite streaming apps. Enjoy a cinematic experience in the comfort of your living room.",
                    ShortDescription = "55-inch 4K Smart TV with HDR and built-in streaming apps",
                    Price = 699.99,
                    DiscountPrice = 649.99,
                    IsDiscounted = true,
                    DiscountStartDate = new DateTime(2025, 4, 1),
                    DiscountEndDate = new DateTime(2025, 5, 15),
                    StockQuantity = 50,
                    AllowBackorder = false,
                    SKU = "ELC-SMTV-001",
                    Barcode = "789012345678",
                    CategoryId = 10, // Televisions subcategory
                    BrandId = 1, // Tech Solutions
                    VendorId = "1", // Tech Solutions Inc.
                    WeightInKg = 15.5,
                    WidthInCm = 123.5,
                    HeightInCm = 71.2,
                    LengthInCm = 8.3,
                    IsActive = true,
                    IsFeatured = true,
                    IsNewArrival = false,
                    IsTrending = true,
                    MetaTitle = "55 inch 4K Smart TV | Tech Solutions",
                    MetaDescription = "Shop our 55-inch 4K UHD Smart TV with HDR technology for the ultimate home entertainment experience.",
                    Views = 2500,
                    SoldCount = 120,
                    AverageRating = 4.7,
                    TotalReviews = 85
                },
                new Product
                {
                    Id = 2,
                    Title = "Premium Cotton T-Shirt - Mens (Navy Blue)",
                    Description = "Experience ultimate comfort with our premium 100% combed cotton men's t-shirt. Designed for a classic fit and exceptional softness, this navy blue tee is a versatile wardrobe staple perfect for everyday wear. Available in various sizes.",
                    ShortDescription = "Premium soft cotton t-shirt for men in navy blue",
                    Price = 20.00,
                    DiscountPrice = 16.99,
                    IsDiscounted = true,
                    DiscountStartDate = new DateTime(2025, 4, 10),
                    DiscountEndDate = new DateTime(2025, 4, 30),
                    StockQuantity = 100,
                    AllowBackorder = true,
                    SKU = "APP-MTSRT-002-NVY",
                    Barcode = "456789012345",
                    CategoryId = 11, // Menswear subcategory
                    BrandId = 2, // Fashion Forward,
                    VendorId = "2", // Fashion Forward Ltd. 
                    WeightInKg = 0.2,
                    WidthInCm = 50.0,
                    HeightInCm = 70.0,
                    LengthInCm = 0.5,
                    IsActive = true,
                    IsFeatured = false,
                    IsNewArrival = false,
                    IsTrending = false,
                    MetaTitle = "Men's Premium Cotton T-Shirt | Fashion Forward",
                    MetaDescription = "Classic fit men's navy blue t-shirt made from 100% premium combed cotton for all-day comfort.",
                    Views = 1800,
                    SoldCount = 250,
                    AverageRating = 4.5,
                    TotalReviews = 120
                },
                new Product
                {
                    Id = 3,
                    Title = "Essential Garden Tool Set (3-Piece with Wooden Handles)",
                    Description = "Get your gardening tasks done with ease using our durable 3-piece garden tool set. Includes a sturdy trowel, hand fork, and cultivator, all featuring comfortable wooden handles for a secure grip. Perfect for both novice and experienced gardeners.",
                    ShortDescription = "3-piece garden tool set with wooden handles",
                    Price = 40.00,
                    DiscountPrice = 40.00,
                    IsDiscounted = false,
                    StockQuantity = 30,
                    AllowBackorder = false,
                    SKU = "HGN-TLSET-003-WD",
                    Barcode = "123456789012",
                    CategoryId = 14, // Garden Tools subcategory
                    BrandId = 3, // Green Living
                    VendorId = "3", // Green Living Co.
                    WeightInKg = 0.9,
                    WidthInCm = 12.0,
                    HeightInCm = 30.0,
                    LengthInCm = 5.0,
                    IsActive = true,
                    IsFeatured = false,
                    IsNewArrival = true,
                    IsTrending = false,
                    MetaTitle = "Essential Garden Tool Set | Green Living",
                    MetaDescription = "High-quality 3-piece garden tool set with comfortable wooden handles for all your gardening needs.",
                    Views = 950,
                    SoldCount = 45,
                    AverageRating = 4.8,
                    TotalReviews = 28
                },
                new Product
                {
                    Id = 4,
                    Title = "Ultra-Slim Laptop 15.6\" with SSD",
                    Description = "Experience lightning-fast performance with our ultra-slim 15.6-inch laptop. Featuring a powerful processor, 512GB SSD storage, and 16GB RAM for seamless multitasking. The vibrant Full HD display and long-lasting battery make it perfect for work and entertainment on the go.",
                    ShortDescription = "15.6\" ultra-slim laptop with SSD and powerful performance",
                    Price = 999.99,
                    DiscountPrice = 899.99,
                    IsDiscounted = true,
                    DiscountStartDate = new DateTime(2025, 4, 5),
                    DiscountEndDate = new DateTime(2025, 5, 5),
                    StockQuantity = 35,
                    AllowBackorder = false,
                    SKU = "ELC-LPTOP-004",
                    Barcode = "567890123456",
                    CategoryId = 9, // Laptops subcategory
                    BrandId = 1, // Tech Solutions
                    VendorId = "1",
                    WeightInKg = 1.8,
                    WidthInCm = 35.6,
                    HeightInCm = 1.8,
                    LengthInCm = 24.2,
                    IsActive = true,
                    IsFeatured = true,
                    IsNewArrival = true,
                    IsTrending = true,
                    MetaTitle = "Ultra-Slim 15.6\" Laptop | Tech Solutions",
                    MetaDescription = "Powerful and portable 15.6-inch laptop with SSD storage for fast performance anywhere you go.",
                    Views = 3200,
                    SoldCount = 85,
                    AverageRating = 4.6,
                    TotalReviews = 62
                },
                new Product
                {
                    Id = 5,
                    Title = "Designer Leather Handbag - Women's (Black)",
                    Description = "Add a touch of elegance to any outfit with our designer leather handbag. Crafted from premium genuine leather with a stylish gold-tone hardware and multiple interior compartments for organization. The adjustable shoulder strap and handle offer versatile carrying options.",
                    ShortDescription = "Premium black leather handbag with gold accents",
                    Price = 149.99,
                    DiscountPrice = 129.99,
                    IsDiscounted = true,
                    DiscountStartDate = new DateTime(2025, 4, 15),
                    DiscountEndDate = new DateTime(2025, 5, 15),
                    StockQuantity = 25,
                    AllowBackorder = false,
                    SKU = "APP-HBAG-005-BLK",
                    Barcode = "345678901234",
                    CategoryId = 12, // Womenswear subcategory
                    BrandId = 2, // Fashion Forward
                    VendorId = "2", // Fashion Forward Ltd.
                    WeightInKg = 0.8,
                    WidthInCm = 35.0,
                    HeightInCm = 25.0,
                    LengthInCm = 12.0,
                    IsActive = true,
                    IsFeatured = true,
                    IsNewArrival = false,
                    IsTrending = true,
                    MetaTitle = "Designer Black Leather Handbag | Fashion Forward",
                    MetaDescription = "Elegant black leather handbag with multiple compartments and versatile carrying options.",
                    Views = 1950,
                    SoldCount = 68,
                    AverageRating = 4.8,
                    TotalReviews = 45
                },
                new Product
                {
                    Id = 6,
                    Title = "Premium Digital SLR Camera with 18-55mm Lens",
                    Description = "Capture life's special moments with exceptional clarity using our premium DSLR camera. Features a 24.1 megapixel CMOS sensor, 4K video recording, and includes a versatile 18-55mm lens. Perfect for both photography enthusiasts and those looking to elevate their photography skills.",
                    ShortDescription = "24.1MP DSLR camera with 4K video and 18-55mm lens",
                    Price = 899.99,
                    DiscountPrice = 799.99,
                    IsDiscounted = true,
                    DiscountStartDate = new DateTime(2025, 3, 20),
                    DiscountEndDate = new DateTime(2025, 4, 30),
                    StockQuantity = 20,
                    AllowBackorder = false,
                    SKU = "ELC-CAM-006",
                    Barcode = "234567890123",
                    CategoryId = 1, // Electronics category
                    BrandId = 1, // Tech Solutions
                    VendorId = "1", // Tech Solutions Inc.
                    WeightInKg = 0.7,
                    WidthInCm = 12.9,
                    HeightInCm = 10.0,
                    LengthInCm = 7.8,
                    IsActive = true,
                    IsFeatured = true,
                    IsNewArrival = false,
                    IsTrending = false,
                    MetaTitle = "Premium DSLR Camera with Lens | Tech Solutions",
                    MetaDescription = "Professional-grade DSLR camera with 24.1MP sensor and 4K video capability for stunning photos and videos.",
                    Views = 1680,
                    SoldCount = 42,
                    AverageRating = 4.9,
                    TotalReviews = 36
                },
                new Product
                {
                    Id = 7,
                    Title = "Organic Green Tea Gift Set (Variety Pack)",
                    Description = "Indulge in the refreshing taste of premium organic green tea with our curated gift set. Includes 6 distinct varieties of hand-picked green tea leaves packaged in elegant tins. Perfect for tea enthusiasts or as a thoughtful gift for special occasions.",
                    ShortDescription = "Gift set of 6 premium organic green tea varieties",
                    Price = 45.00,
                    DiscountPrice = 45.00,
                    IsDiscounted = false,
                    StockQuantity = 40,
                    AllowBackorder = true,
                    SKU = "HGN-TEA-007",
                    Barcode = "890123456789",
                    CategoryId = 3, // Home & Garden category
                    BrandId = 3, // Green Living
                    VendorId = "3", // Green Living Co.
                    WeightInKg = 0.5,
                    WidthInCm = 25.0,
                    HeightInCm = 8.0,
                    LengthInCm = 20.0,
                    IsActive = true,
                    IsFeatured = false,
                    IsNewArrival = true,
                    IsTrending = false,
                    MetaTitle = "Organic Green Tea Gift Set | Green Living",
                    MetaDescription = "Premium selection of 6 organic green tea varieties presented in an elegant gift box.",
                    Views = 890,
                    SoldCount = 38,
                    AverageRating = 4.7,
                    TotalReviews = 22
                },
                new Product
                {
                    Id = 8,
                    Title = "Historical Fiction: 'The Forgotten Dynasty'",
                    Description = "Journey back in time with this captivating historical fiction novel that uncovers the story of a lost dynasty. Set in the 16th century, the narrative weaves together adventure, romance, and political intrigue as a young scholar uncovers ancient secrets that could change the course of history.",
                    ShortDescription = "Captivating historical fiction novel set in the 16th century",
                    Price = 18.99,
                    DiscountPrice = 15.99,
                    IsDiscounted = true,
                    DiscountStartDate = new DateTime(2025, 4, 1),
                    DiscountEndDate = new DateTime(2025, 4, 30),
                    StockQuantity = 60,
                    AllowBackorder = true,
                    SKU = "BOK-HFIC-008",
                    Barcode = "901234567890",
                    CategoryId = 15, // Fiction subcategory
                    BrandId = 4, // Global Reads
                    VendorId = "4", // Global Reads 
                    WeightInKg = 0.4,
                    WidthInCm = 15.2,
                    HeightInCm = 2.5,
                    LengthInCm = 22.8,
                    IsActive = true,
                    IsFeatured = false,
                    IsNewArrival = true,
                    IsTrending = true,
                    MetaTitle = "The Forgotten Dynasty | Historical Fiction",
                    MetaDescription = "Immerse yourself in a captivating tale of secrets, adventure, and intrigue set in the 16th century.",
                    Views = 1250,
                    SoldCount = 72,
                    AverageRating = 4.5,
                    TotalReviews = 48
                },
                new Product
                {
                    Id = 9,
                    Title = "All-Weather Hiking Boots (Unisex)",
                    Description = "Conquer any trail with confidence in our all-weather hiking boots. Featuring waterproof construction, superior grip rubber soles, and cushioned insoles for all-day comfort. The breathable membrane keeps feet dry while allowing moisture to escape, making these perfect for year-round outdoor adventures.",
                    ShortDescription = "Waterproof and comfortable hiking boots for all terrains",
                    Price = 129.95,
                    DiscountPrice = 109.95,
                    IsDiscounted = true,
                    DiscountStartDate = new DateTime(2025, 3, 15),
                    DiscountEndDate = new DateTime(2025, 5, 1),
                    StockQuantity = 45,
                    AllowBackorder = false,
                    SKU = "SPT-HBOOT-009",
                    Barcode = "012345678901",
                    CategoryId = 17, // Camping & Hiking subcategory
                    BrandId = 5, // Adventure Gear
                    VendorId = "5", // Adventure Gear Corp. 
                    WeightInKg = 1.2,
                    WidthInCm = 30.0,
                    HeightInCm = 15.0,
                    LengthInCm = 20.0,
                    IsActive = true,
                    IsFeatured = false,
                    IsNewArrival = false,
                    IsTrending = true,
                    MetaTitle = "All-Weather Hiking Boots | Adventure Gear",
                    MetaDescription = "Durable and waterproof hiking boots designed for maximum comfort on any terrain and in any weather.",
                    Views = 2100,
                    SoldCount = 95,
                    AverageRating = 4.8,
                    TotalReviews = 63
                },
                new Product
                {
                    Id = 10,
                    Title = "Anti-Aging Skincare Collection Set",
                    Description = "Turn back the clock with our comprehensive anti-aging skincare collection. This five-piece set includes cleanser, toner, day cream with SPF 30, night serum, and eye cream, all formulated with powerful peptides, antioxidants, and hyaluronic acid to reduce fine lines and restore youthful radiance.",
                    ShortDescription = "5-piece anti-aging skincare set with peptides and antioxidants",
                    Price = 89.99,
                    DiscountPrice = 75.99,
                    IsDiscounted = true,
                    DiscountStartDate = new DateTime(2025, 4, 10),
                    DiscountEndDate = new DateTime(2025, 5, 10),
                    StockQuantity = 30,
                    AllowBackorder = true,
                    SKU = "BPC-AAGE-010",
                    Barcode = "678901234567",
                    CategoryId = 6, // Beauty & Personal Care category
                    BrandId = 6, // Glow & Glam
                    VendorId = "6",
                    WeightInKg = 0.6,
                    WidthInCm = 20.0,
                    HeightInCm = 15.0,
                    LengthInCm = 8.0,
                    IsActive = true,
                    IsFeatured = true,
                    IsNewArrival = true,
                    IsTrending = true,
                    MetaTitle = "Anti-Aging Skincare Collection | Glow & Glam",
                    MetaDescription = "Complete 5-piece anti-aging skincare routine with powerful ingredients for visibly younger-looking skin.",
                    Views = 2800,
                    SoldCount = 110,
                    AverageRating = 4.7,
                    TotalReviews = 82
                },
                new Product
                {
                    Id = 11,
                    Title = "Interactive Learning Robot for Kids",
                    Description = "Spark your child's interest in STEM with our interactive learning robot. Programmable through an easy-to-use app, this friendly robot teaches coding concepts, plays educational games, and responds to voice commands. With multiple sensors and expandable capabilities, it grows with your child's skills.",
                    ShortDescription = "Educational programmable robot that teaches coding to children",
                    Price = 79.99,
                    DiscountPrice = 69.99,
                    IsDiscounted = true,
                    DiscountStartDate = new DateTime(2025, 3, 1),
                    DiscountEndDate = new DateTime(2025, 5, 31),
                    StockQuantity = 25,
                    AllowBackorder = false,
                    SKU = "TOY-ROBOT-011",
                    Barcode = "789012345670",
                    CategoryId = 7, // Toys & Games category
                    BrandId = 7, // Fun Time Toys
                    VendorId = "7",
                    WeightInKg = 0.5,
                    WidthInCm = 15.0,
                    HeightInCm = 22.0,
                    LengthInCm = 12.0,
                    IsActive = true,
                    IsFeatured = true,
                    IsNewArrival = true,
                    IsTrending = true,
                    MetaTitle = "Interactive Learning Robot for Kids | Fun Time Toys",
                    MetaDescription = "Educational robot that makes learning to code fun and engaging for children ages 6-12.",
                    Views = 3500,
                    SoldCount = 135,
                    AverageRating = 4.9,
                    TotalReviews = 98
                },
                new Product
                {
                    Id = 12,
                    Title = "Smartphone Stabilizer Gimbal",
                    Description = "Take your mobile videography to the next level with our 3-axis smartphone stabilizer gimbal. Featuring intelligent tracking, multiple shooting modes, and foldable design for easy portability. The rechargeable battery provides up to 12 hours of operation, perfect for content creators and travelers.",
                    ShortDescription = "3-axis gimbal stabilizer for professional smartphone videos",
                    Price = 85.00,
                    DiscountPrice = 69.99,
                    IsDiscounted = true,
                    DiscountStartDate = new DateTime(2025, 4, 15),
                    DiscountEndDate = new DateTime(2025, 5, 15),
                    StockQuantity = 40,
                    AllowBackorder = false,
                    SKU = "ELC-GIMB-012",
                    Barcode = "123456789013",
                    CategoryId = 8, // Smartphones subcategory
                    BrandId = 1, // Tech Solutions
                    VendorId = "1",
                    WeightInKg = 0.4,
                    WidthInCm = 12.0,
                    HeightInCm = 19.0,
                    LengthInCm = 5.0,
                    IsActive = true,
                    IsFeatured = false,
                    IsNewArrival = true,
                    IsTrending = false,
                    MetaTitle = "Smartphone Stabilizer Gimbal | Tech Solutions",
                    MetaDescription = "Professional 3-axis gimbal stabilizer for smooth, cinematic smartphone videos.",
                    Views = 1680,
                    SoldCount = 58,
                    AverageRating = 4.6,
                    TotalReviews = 42
                },
                 new Product
                 {
                     Id = 13,
                     Title = "Smart Home Starter Kit",
                     Description = "Transform your house into a smart home with our comprehensive starter kit. Includes a smart hub, two smart plugs, two motion sensors, and three smart light bulbs that can all be controlled via app or voice commands. Compatible with major voice assistants for seamless integration with your existing devices.",
                     ShortDescription = "Complete smart home kit with hub, plugs, sensors and bulbs",
                     Price = 179.99,
                     DiscountPrice = 149.99,
                     IsDiscounted = true,
                     DiscountStartDate = new DateTime(2025, 4, 1),
                     DiscountEndDate = new DateTime(2025, 6, 1),
                     StockQuantity = 20,
                     AllowBackorder = true,
                     SKU = "ELC-SMHM-013",
                     Barcode = "234567890124",
                     CategoryId = 1, // Electronics category
                     BrandId = 1, // Tech Solutions
                     VendorId = "1", // Tech Solutions Inc.
                     WeightInKg = 1.2,
                     WidthInCm = 30.0,
                     HeightInCm = 25.0,
                     LengthInCm = 15.0,
                     IsActive = true,
                     IsFeatured = true,
                     IsNewArrival = true,
                     IsTrending = true,
                     MetaTitle = "Smart Home Starter Kit | Tech Solutions",
                     MetaDescription = "Complete solution to begin automating your home with smart devices controllable via app or voice.",
                     Views = 2900,
                     SoldCount = 65,
                     AverageRating = 4.8,
                     TotalReviews = 38
                 }
            );

            // Seed Product Images
            modelBuilder.Entity<ProductImage>().HasData(
                // Images for Smart TV (ProductId = 1)
                new ProductImage { Id = 1, ProductId = 1, ImageUrl = "/images/products/smarttv_main.jpg" },
                new ProductImage { Id = 2, ProductId = 1, ImageUrl = "/images/products/smarttv_side.jpg" },
                new ProductImage { Id = 3, ProductId = 1, ImageUrl = "/images/products/smarttv_ports.jpg" },

                // Images for Cotton T-Shirt (ProductId = 2)
                new ProductImage { Id = 4, ProductId = 2, ImageUrl = "/images/products/tshirt_navy_front.jpg" },
                new ProductImage { Id = 5, ProductId = 2, ImageUrl = "/images/products/tshirt_navy_back.jpg" },

                // Images for Garden Tool Set (ProductId = 3)
                new ProductImage { Id = 6, ProductId = 3, ImageUrl = "/images/products/garden_tool_set.jpg" },
                new ProductImage { Id = 7, ProductId = 3, ImageUrl = "/images/products/gardentool_trowel.jpg" },
                new ProductImage { Id = 8, ProductId = 3, ImageUrl = "/images/products/gardentool_fork.jpg" },

                // Images for Laptop (ProductId = 4)
                new ProductImage { Id = 9, ProductId = 4, ImageUrl = "/images/products/laptop_main.jpg" },
                new ProductImage { Id = 10, ProductId = 4, ImageUrl = "/images/products/laptop_open.jpg" },
                new ProductImage { Id = 11, ProductId = 4, ImageUrl = "/images/products/laptop_side.jpg" },

                // Images for Leather Handbag (ProductId = 5)
                new ProductImage { Id = 12, ProductId = 5, ImageUrl = "/images/products/handbag_black_main.jpg" },
                new ProductImage { Id = 13, ProductId = 5, ImageUrl = "/images/products/handbag_black_open.jpg" },

                // Images for Digital SLR Camera (ProductId = 6)
                new ProductImage { Id = 14, ProductId = 6, ImageUrl = "/images/products/camera_main.jpg" },
                new ProductImage { Id = 15, ProductId = 6, ImageUrl = "/images/products/camera_top.jpg" },
                new ProductImage { Id = 16, ProductId = 6, ImageUrl = "/images/products/camera_lens.jpg" },

                // Images for Green Tea Set (ProductId = 7)
                new ProductImage { Id = 17, ProductId = 7, ImageUrl = "/images/products/tea_set_complete.jpg" },
                new ProductImage { Id = 18, ProductId = 7, ImageUrl = "/images/products/tea_tin_open.jpg" },

                // Images for Book (ProductId = 8)
                new ProductImage { Id = 19, ProductId = 8, ImageUrl = "/images/products/book_cover.jpg" },
                new ProductImage { Id = 20, ProductId = 8, ImageUrl = "/images/products/book_back.jpg" },

                // Images for Hiking Boots (ProductId = 9)
                new ProductImage { Id = 21, ProductId = 9, ImageUrl = "/images/products/hikingboots_pair.jpg" },
                new ProductImage { Id = 22, ProductId = 9, ImageUrl = "/images/products/hikingboots_sole.jpg" },
                new ProductImage { Id = 23, ProductId = 9, ImageUrl = "/images/products/hikingboots_side.jpg" },

                // Images for Skincare Set (ProductId = 10)
                new ProductImage { Id = 24, ProductId = 10, ImageUrl = "/images/products/skincare_set.jpg" },
                new ProductImage { Id = 25, ProductId = 10, ImageUrl = "/images/products/skincare_serum.jpg" },
                new ProductImage { Id = 26, ProductId = 10, ImageUrl = "/images/products/skincare_cream.jpg" },

                // Images for Learning Robot (ProductId = 11)
                new ProductImage { Id = 27, ProductId = 11, ImageUrl = "/images/products/robot_front.jpg" },
                new ProductImage { Id = 28, ProductId = 11, ImageUrl = "/images/products/robot_side.jpg" },

                // Images for Smartphone Gimbal (ProductId = 12)
                new ProductImage { Id = 29, ProductId = 12, ImageUrl = "/images/products/gimbal_main.jpg" },
                new ProductImage { Id = 30, ProductId = 12, ImageUrl = "/images/products/gimbal_folded.jpg" },

                // Images for Smart Home Kit (ProductId = 13)
                new ProductImage { Id = 31, ProductId = 13, ImageUrl = "/images/products/smarthome_kit.jpg" },
                new ProductImage { Id = 32, ProductId = 13, ImageUrl = "/images/products/smarthome_hub.jpg" },
                new ProductImage { Id = 33, ProductId = 13, ImageUrl = "/images/products/smarthome_bulb.jpg" }
           );

            // Seed Product Specifications
            modelBuilder.Entity<ProductSpecification>().HasData(
                // Specifications for Smart TV (ProductId = 1)
                new ProductSpecification { Id = 1, ProductId = 1, Key = "Screen Size", Value = "55 inches" },
                new ProductSpecification { Id = 2, ProductId = 1, Key = "Resolution", Value = "4K Ultra HD (3840 x 2160)" },
                new ProductSpecification { Id = 3, ProductId = 1, Key = "Display Technology", Value = "LED" },
                new ProductSpecification { Id = 4, ProductId = 1, Key = "HDR", Value = "Yes" },
                new ProductSpecification { Id = 5, ProductId = 1, Key = "Smart TV", Value = "Yes" },
                new ProductSpecification { Id = 6, ProductId = 1, Key = "Connectivity", Value = "Wi-Fi, Bluetooth, HDMI, USB" },

                // Specifications for Cotton T-Shirt (ProductId = 2)
                new ProductSpecification { Id = 7, ProductId = 2, Key = "Material", Value = "100% Combed Cotton" },
                new ProductSpecification { Id = 8, ProductId = 2, Key = "Color", Value = "Navy Blue" },
                new ProductSpecification { Id = 9, ProductId = 2, Key = "Care Instructions", Value = "Machine wash cold, tumble dry low" },

                // Specifications for Garden Tool Set (ProductId = 3)
                new ProductSpecification { Id = 10, ProductId = 3, Key = "Material", Value = "Stainless Steel with Wooden Handles" },
                new ProductSpecification { Id = 11, ProductId = 3, Key = "Pieces", Value = "3" },
                new ProductSpecification { Id = 12, ProductId = 3, Key = "Tool Length", Value = "30 cm" },

                // Specifications for Laptop (ProductId = 4)
                new ProductSpecification { Id = 13, ProductId = 4, Key = "Processor", Value = "Intel Core i7" },
                new ProductSpecification { Id = 14, ProductId = 4, Key = "RAM", Value = "16 GB" },
                new ProductSpecification { Id = 15, ProductId = 4, Key = "Storage", Value = "512 GB SSD" },
                new ProductSpecification { Id = 16, ProductId = 4, Key = "Display", Value = "15.6-inch Full HD (1920 x 1080)" },
                new ProductSpecification { Id = 17, ProductId = 4, Key = "Battery Life", Value = "Up to 10 hours" },
                new ProductSpecification { Id = 18, ProductId = 4, Key = "Operating System", Value = "Windows 11" },

                // Specifications for Leather Handbag (ProductId = 5)
                new ProductSpecification { Id = 19, ProductId = 5, Key = "Material", Value = "Genuine Leather" },
                new ProductSpecification { Id = 20, ProductId = 5, Key = "Color", Value = "Black" },
                new ProductSpecification { Id = 21, ProductId = 5, Key = "Dimensions", Value = "35 x 25 x 12 cm" },
                new ProductSpecification { Id = 22, ProductId = 5, Key = "Hardware", Value = "Gold-tone" },

                // Specifications for Digital SLR Camera (ProductId = 6)
                new ProductSpecification { Id = 23, ProductId = 6, Key = "Megapixels", Value = "24.1 MP" },
                new ProductSpecification { Id = 24, ProductId = 6, Key = "Sensor Type", Value = "CMOS" },
                new ProductSpecification { Id = 25, ProductId = 6, Key = "Video Resolution", Value = "4K" },
                new ProductSpecification { Id = 26, ProductId = 6, Key = "Lens", Value = "18-55mm" },
                new ProductSpecification { Id = 27, ProductId = 6, Key = "ISO Range", Value = "100-25600" },

                // Specifications for Green Tea Set (ProductId = 7)
                new ProductSpecification { Id = 28, ProductId = 7, Key = "Varieties", Value = "6" },
                new ProductSpecification { Id = 29, ProductId = 7, Key = "Organic", Value = "Yes" },
                new ProductSpecification { Id = 30, ProductId = 7, Key = "Weight", Value = "300g total (50g each)" },
                new ProductSpecification { Id = 31, ProductId = 7, Key = "Packaging", Value = "Metal tins in gift box" },

                // Specifications for Book (ProductId = 8)
                new ProductSpecification { Id = 32, ProductId = 8, Key = "Format", Value = "Hardcover" },
                new ProductSpecification { Id = 33, ProductId = 8, Key = "Pages", Value = "384" },
                new ProductSpecification { Id = 34, ProductId = 8, Key = "Genre", Value = "Historical Fiction" },
                new ProductSpecification { Id = 35, ProductId = 8, Key = "Language", Value = "English" },

                // Specifications for Hiking Boots (ProductId = 9)
                new ProductSpecification { Id = 36, ProductId = 9, Key = "Material", Value = "Waterproof Leather and Mesh" },
                new ProductSpecification { Id = 37, ProductId = 9, Key = "Sole", Value = "Rubber with Multi-directional Traction" },
                new ProductSpecification { Id = 38, ProductId = 9, Key = "Closure", Value = "Lace-up" },
                new ProductSpecification { Id = 39, ProductId = 9, Key = "Gender", Value = "Unisex" },

                // Specifications for Skincare Set (ProductId = 10)
                new ProductSpecification { Id = 40, ProductId = 10, Key = "Pieces", Value = "5" },
                new ProductSpecification { Id = 41, ProductId = 10, Key = "Skin Type", Value = "All" },
                new ProductSpecification { Id = 42, ProductId = 10, Key = "Key Ingredients", Value = "Peptides, Hyaluronic Acid, Antioxidants" },
                new ProductSpecification { Id = 43, ProductId = 10, Key = "SPF", Value = "30 (Day Cream)" },

                // Specifications for Learning Robot (ProductId = 11)
                new ProductSpecification { Id = 44, ProductId = 11, Key = "Age Range", Value = "6-12 years" },
                new ProductSpecification { Id = 45, ProductId = 11, Key = "Programmable", Value = "Yes" },
                new ProductSpecification { Id = 46, ProductId = 11, Key = "Battery Life", Value = "4 hours" },
                new ProductSpecification { Id = 47, ProductId = 11, Key = "Connectivity", Value = "Bluetooth" },
                new ProductSpecification { Id = 48, ProductId = 11, Key = "App Compatibility", Value = "iOS and Android" },

                // Specifications for Smartphone Gimbal (ProductId = 12)
                new ProductSpecification { Id = 49, ProductId = 12, Key = "Axes", Value = "3-axis" },
                new ProductSpecification { Id = 50, ProductId = 12, Key = "Battery Life", Value = "12 hours" },
                new ProductSpecification { Id = 51, ProductId = 12, Key = "Compatibility", Value = "Most smartphones up to 6.7 inches" },
                new ProductSpecification { Id = 52, ProductId = 12, Key = "Weight", Value = "400g" },

                // Specifications for Smart Home Kit (ProductId = 13)
                new ProductSpecification { Id = 53, ProductId = 13, Key = "Components", Value = "1 Hub, 2 Plugs, 2 Sensors, 3 Bulbs" },
                new ProductSpecification { Id = 54, ProductId = 13, Key = "Connectivity", Value = "Wi-Fi, Bluetooth" },
                new ProductSpecification { Id = 55, ProductId = 13, Key = "Voice Assistant Compatibility", Value = "Alexa, Google Assistant, Siri" },
                new ProductSpecification { Id = 56, ProductId = 13, Key = "App Control", Value = "Yes" }
            );

            // Seed Product Tags
            modelBuilder.Entity<ProductTag>().HasData(
                // Tags for Smart TV (ProductId = 1)
                new ProductTag { Id = 1, ProductId = 1, TagName = "4K" },
                new ProductTag { Id = 2, ProductId = 1, TagName = "Smart TV" },
                new ProductTag { Id = 3, ProductId = 1, TagName = "HDR" },
                new ProductTag { Id = 4, ProductId = 1, TagName = "Home Entertainment" },

                // Tags for Cotton T-Shirt (ProductId = 2)
                new ProductTag { Id = 5, ProductId = 2, TagName = "Men's Fashion" },
                new ProductTag { Id = 6, ProductId = 2, TagName = "Casual Wear" },
                new ProductTag { Id = 7, ProductId = 2, TagName = "Cotton" },

                // Tags for Garden Tool Set (ProductId = 3)
                new ProductTag { Id = 8, ProductId = 3, TagName = "Gardening" },
                new ProductTag { Id = 9, ProductId = 3, TagName = "Tools" },
                new ProductTag { Id = 10, ProductId = 3, TagName = "New Arrival" },

                // Tags for Laptop (ProductId = 4)
                new ProductTag { Id = 11, ProductId = 4, TagName = "Computing" },
                new ProductTag { Id = 12, ProductId = 4, TagName = "SSD" },
                new ProductTag { Id = 13, ProductId = 4, TagName = "Lightweight" },
                new ProductTag { Id = 14, ProductId = 4, TagName = "New Arrival" },

                // Tags for Leather Handbag (ProductId = 5)
                new ProductTag { Id = 15, ProductId = 5, TagName = "Women's Fashion" },
                new ProductTag { Id = 16, ProductId = 5, TagName = "Leather" },
                new ProductTag { Id = 17, ProductId = 5, TagName = "Designer" },
                new ProductTag { Id = 18, ProductId = 5, TagName = "Trending" },

                // Tags for Digital SLR Camera (ProductId = 6)
                new ProductTag { Id = 19, ProductId = 6, TagName = "Photography" },
                new ProductTag { Id = 20, ProductId = 6, TagName = "4K Video" },
                new ProductTag { Id = 21, ProductId = 6, TagName = "Featured" },

                // Tags for Green Tea Set (ProductId = 7)
                new ProductTag { Id = 22, ProductId = 7, TagName = "Organic" },
                new ProductTag { Id = 23, ProductId = 7, TagName = "Gift Set" },
                new ProductTag { Id = 24, ProductId = 7, TagName = "New Arrival" },

                // Tags for Book (ProductId = 8)
                new ProductTag { Id = 25, ProductId = 8, TagName = "Historical Fiction" },
                new ProductTag { Id = 26, ProductId = 8, TagName = "Bestseller" },
                new ProductTag { Id = 27, ProductId = 8, TagName = "New Arrival" },

                // Tags for Hiking Boots (ProductId = 9)
                new ProductTag { Id = 28, ProductId = 9, TagName = "Outdoor" },
                new ProductTag { Id = 29, ProductId = 9, TagName = "Waterproof" },
                new ProductTag { Id = 30, ProductId = 9, TagName = "Unisex" },
                new ProductTag { Id = 31, ProductId = 9, TagName = "Trending" },

                // Tags for Skincare Set (ProductId = 10)
                new ProductTag { Id = 32, ProductId = 10, TagName = "Anti-Aging" },
                new ProductTag { Id = 33, ProductId = 10, TagName = "Beauty" },
                new ProductTag { Id = 34, ProductId = 10, TagName = "New Arrival" },
                new ProductTag { Id = 35, ProductId = 10, TagName = "Trending" },

                // Tags for Learning Robot (ProductId = 11)
                new ProductTag { Id = 36, ProductId = 11, TagName = "Educational" },
                new ProductTag { Id = 37, ProductId = 11, TagName = "STEM" },
                new ProductTag { Id = 38, ProductId = 11, TagName = "Kids" },
                new ProductTag { Id = 39, ProductId = 11, TagName = "New Arrival" },

                // Tags for Smartphone Gimbal (ProductId = 12)
                new ProductTag { Id = 40, ProductId = 12, TagName = "Photography" },
                new ProductTag { Id = 41, ProductId = 12, TagName = "Accessories" },
                new ProductTag { Id = 42, ProductId = 12, TagName = "New Arrival" },

                // Tags for Smart Home Kit (ProductId = 13)
                new ProductTag { Id = 43, ProductId = 13, TagName = "Smart Home" },
                new ProductTag { Id = 44, ProductId = 13, TagName = "IoT" },
                new ProductTag { Id = 45, ProductId = 13, TagName = "New Arrival" },
                new ProductTag { Id = 46, ProductId = 13, TagName = "Trending" }
            );

            // Seed Product Variants
            modelBuilder.Entity<ProductVariant>().HasData(
                // Variants for Cotton T-Shirt (ProductId = 2)
                new ProductVariant { Id = 1, ProductId = 2, VariantName = "Size - S", SKU = "APP-MTSRT-002-NVY-S", Price = 20.00, DiscountPrice = 16.99, StockQuantity = 25 },
                new ProductVariant { Id = 2, ProductId = 2, VariantName = "Size - M", SKU = "APP-MTSRT-002-NVY-M", Price = 20.00, DiscountPrice = 16.99, StockQuantity = 30 },
                new ProductVariant { Id = 3, ProductId = 2, VariantName = "Size - L", SKU = "APP-MTSRT-002-NVY-L", Price = 20.00, DiscountPrice = 16.99, StockQuantity = 25 },
                new ProductVariant { Id = 4, ProductId = 2, VariantName = "Size - XL", SKU = "APP-MTSRT-002-NVY-XL", Price = 22.00, DiscountPrice = 18.99, StockQuantity = 20 },

                // Variants for Leather Handbag (ProductId = 5)
                new ProductVariant { Id = 5, ProductId = 5, VariantName = "Color - Black", SKU = "APP-HBAG-005-BLK", Price = 149.99, DiscountPrice = 129.99, StockQuantity = 15 },
                new ProductVariant { Id = 6, ProductId = 5, VariantName = "Color - Brown", SKU = "APP-HBAG-005-BRN", Price = 149.99, DiscountPrice = 129.99, StockQuantity = 10 },

                // Variants for Hiking Boots (ProductId = 9)
                new ProductVariant { Id = 7, ProductId = 9, VariantName = "Size - US 7 / EU 38", SKU = "SPT-HBOOT-009-07", Price = 129.95, DiscountPrice = 109.95, StockQuantity = 8 },
                new ProductVariant { Id = 8, ProductId = 9, VariantName = "Size - US 8 / EU 39", SKU = "SPT-HBOOT-009-08", Price = 129.95, DiscountPrice = 109.95, StockQuantity = 10 },
                new ProductVariant { Id = 9, ProductId = 9, VariantName = "Size - US 9 / EU 40", SKU = "SPT-HBOOT-009-09", Price = 129.95, DiscountPrice = 109.95, StockQuantity = 12 },
                new ProductVariant { Id = 10, ProductId = 9, VariantName = "Size - US 10 / EU 41", SKU = "SPT-HBOOT-009-10", Price = 129.95, DiscountPrice = 109.95, StockQuantity = 10 },
                new ProductVariant { Id = 11, ProductId = 9, VariantName = "Size - US 11 / EU 42", SKU = "SPT-HBOOT-009-11", Price = 129.95, DiscountPrice = 109.95, StockQuantity = 5 },

                // Variants for Smart TV (ProductId = 1)
                new ProductVariant { Id = 12, ProductId = 1, VariantName = "Size - 55 inch", SKU = "ELC-SMTV-001-55", Price = 699.99, DiscountPrice = 649.99, StockQuantity = 30 },
                new ProductVariant { Id = 13, ProductId = 1, VariantName = "Size - 65 inch", SKU = "ELC-SMTV-001-65", Price = 899.99, DiscountPrice = 849.99, StockQuantity = 20 },

                // Variants for Laptop (ProductId = 4)
                new ProductVariant { Id = 14, ProductId = 4, VariantName = "RAM - 16GB / Storage - 512GB", SKU = "ELC-LPTOP-004-16-512", Price = 999.99, DiscountPrice = 899.99, StockQuantity = 20 },
                new ProductVariant { Id = 15, ProductId = 4, VariantName = "RAM - 32GB / Storage - 1TB", SKU = "ELC-LPTOP-004-32-1TB", Price = 1299.99, DiscountPrice = 1199.99, StockQuantity = 15 },

                // Variants for Anti-Aging Skincare Collection (ProductId = 10)
                new ProductVariant { Id = 16, ProductId = 10, VariantName = "For Normal Skin", SKU = "BPC-AAGE-010-NORM", Price = 89.99, DiscountPrice = 75.99, StockQuantity = 15 },
                new ProductVariant { Id = 17, ProductId = 10, VariantName = "For Dry Skin", SKU = "BPC-AAGE-010-DRY", Price = 89.99, DiscountPrice = 75.99, StockQuantity = 10 },
                new ProductVariant { Id = 18, ProductId = 10, VariantName = "For Sensitive Skin", SKU = "BPC-AAGE-010-SENS", Price = 94.99, DiscountPrice = 79.99, StockQuantity = 5 }
            );

            // Seed Currencies
            modelBuilder.Entity<Currency>().HasData(
                new Currency { Id = 1, Code = "USD", Symbol = "$", Name = "US Dollar" },
                new Currency { Id = 2, Code = "EUR", Symbol = "€", Name = "Euro" },
                new Currency { Id = 3, Code = "GBP", Symbol = "£", Name = "British Pound" },
                new Currency { Id = 4, Code = "CAD", Symbol = "C$", Name = "Canadian Dollar" },
                new Currency { Id = 5, Code = "AUD", Symbol = "A$", Name = "Australian Dollar" },
                new Currency { Id = 6, Code = "JPY", Symbol = "¥", Name = "Japanese Yen" },
                new Currency { Id = 7, Code = "INR", Symbol = "₹", Name = "Indian Rupee" },
                new Currency { Id = 8, Code = "CHF", Symbol = "Fr", Name = "Swiss Franc" }
            );

            // Seed Timezones
            modelBuilder.Entity<Timezone>().HasData(
                new Timezone { Id = 1, Name = "America/New_York", UtcOffset = "-05:00", UtcOffsetString = "EST", Abbreviation = "EST" },
                new Timezone { Id = 2, Name = "Europe/London", UtcOffset = "+00:00", UtcOffsetString = "GMT", Abbreviation = "GMT" },
                new Timezone { Id = 3, Name = "Asia/Tokyo", UtcOffset = "+09:00", UtcOffsetString = "JST", Abbreviation = "JST" },
                new Timezone { Id = 4, Name = "Australia/Sydney", UtcOffset = "+10:00", UtcOffsetString = "AEDT", Abbreviation = "AEDT" },
                new Timezone { Id = 5, Name = "America/Toronto", UtcOffset = "-05:00", UtcOffsetString = "EST", Abbreviation = "EST" },
                new Timezone { Id = 6, Name = "Europe/Paris", UtcOffset = "+01:00", UtcOffsetString = "CET", Abbreviation = "CET" },
                new Timezone { Id = 7, Name = "Asia/Mumbai", UtcOffset = "+05:30", UtcOffsetString = "IST", Abbreviation = "IST" },
                new Timezone { Id = 8, Name = "Europe/Zurich", UtcOffset = "+01:00", UtcOffsetString = "CET", Abbreviation = "CET" }
            );

            // Seed Customers (8 seeds)
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "555-0101",
                    CompanyId = 1, // Tech Solutions Inc.
                },
                new Customer
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Email = "jane.smith@example.com",
                    PhoneNumber = "555-0102",
                    CompanyId = 2, // Fashion Forward Ltd.
                },
                new Customer
                {
                    Id = 3,
                    Name = "Hiroshi Tanaka",
                    Email = "hiroshi.tanaka@example.com",
                    PhoneNumber = "555-0103",
                    CompanyId = 1, // Tech Solutions Inc.
                },
                new Customer
                {
                    Id = 4,
                    Name = "Emma Brown",
                    Email = "emma.brown@example.com",
                    PhoneNumber = "555-0104",
                    CompanyId = 5, // Adventure Gear Corp.
                },
                new Customer
                {
                    Id = 5,
                    Name = "Liam Johnson",
                    Email = "liam.johnson@example.com",
                    PhoneNumber = "555-0105",
                    CompanyId = 6, // Glow & Glam
                },
                new Customer
                {
                    Id = 6,
                    Name = "Sophie Martin",
                    Email = "sophie.martin@example.com",
                    PhoneNumber = "555-0106",
                    CompanyId = 7, // Fun Time Toys
                },
                new Customer
                {
                    Id = 7,
                    Name = "Arjun Patel",
                    Email = "arjun.patel@example.com",
                    PhoneNumber = "555-0107",
                    CompanyId = 4, // Global Reads
                },
                new Customer
                {
                    Id = 8,
                    Name = "Clara Fischer",
                    Email = "clara.fischer@example.com",
                    PhoneNumber = "555-0108",
                    CompanyId = 1, // Tech Solutions Inc.
                }
            );

            // Seed Address for Customers (owned type)
            modelBuilder.Entity<Customer>()
                .OwnsOne(c => c.Address)
                .HasData(
                    new
                    {
                        CustomerId = 1,
                        Address1 = "123 Maple St",
                        Address2 = "Apt 4B",
                        City = "Springfield",
                        State = "IL",
                        Country = "USA",
                        ZipCode = "62701"
                    },
                    new
                    {
                        CustomerId = 2,
                        Address1 = "456 Oak Ave",
                        Address2 = "Suite 201",
                        City = "London",
                        State = "Greater London",
                        Country = "UK",
                        ZipCode = "SW1A 1AA"
                    },
                    new
                    {
                        CustomerId = 3,
                        Address1 = "789 Sakura St",
                        Address2 = "2 Chome-1-1",
                        City = "Tokyo",
                        State = "Tokyo",
                        Country = "Japan",
                        ZipCode = "100-0001"
                    },
                    new
                    {
                        CustomerId = 4,
                        Address1 = "101 Pine Rd",
                        Address2 = "Level 5",
                        City = "Sydney",
                        State = "NSW",
                        Country = "Australia",
                        ZipCode = "2000"
                    },
                    new
                    {
                        CustomerId = 5,
                        Address1 = "202 Birch Ln",
                        Address2 = "Unit 12",
                        City = "Toronto",
                        State = "ON",
                        Country = "Canada",
                        ZipCode = "M5V 2T7"
                    },
                    new
                    {
                        CustomerId = 6,
                        Address1 = "303 Cedar St",
                        Address2 = "Batiment C",
                        City = "Paris",
                        State = "Île-de-France",
                        Country = "France",
                        ZipCode = "75001"
                    },
                    new
                    {
                        CustomerId = 7,
                        Address1 = "404 Elm Dr",
                        Address2 = "Near Main Gate",
                        City = "Mumbai",
                        State = "MH",
                        Country = "India",
                        ZipCode = "400001"
                    },
                    new
                    {
                        CustomerId = 8,
                        Address1 = "505 Spruce Ct",
                        Address2 = "Block A",
                        City = "Zurich",
                        State = "Zurich",
                        Country = "Switzerland",
                        ZipCode = "8001"
                    }
                );

            // Seed Locations (8 seeds)
            modelBuilder.Entity<Location>().HasData(
                new Location
                {
                    Id = 1,
                    CompanyId = 1, // Tech Solutions Inc.
                    Name = "Silicon City Office",
                    CurrencyId = 1, // USD
                    TimezoneId = 1, // America/New_York
                },
                new Location
                {
                    Id = 2,
                    CompanyId = 2, // Fashion Forward Ltd.
                    Name = "Fashionville Store",
                    CurrencyId = 1, // USD
                    TimezoneId = 1, // America/New_York
                },
                new Location
                {
                    Id = 3,
                    CompanyId = 3, // Green Living Co.
                    Name = "Eco City Warehouse",
                    CurrencyId = 1, // USD
                    TimezoneId = 1, // America/New_York
                },
                new Location
                {
                    Id = 4,
                    CompanyId = 4, // Global Reads
                    Name = "London Bookstore",
                    CurrencyId = 3, // GBP
                    TimezoneId = 2, // Europe/London
                },
                new Location
                {
                    Id = 5,
                    CompanyId = 5, // Adventure Gear Corp.
                    Name = "Sydney Outlet",
                    CurrencyId = 5, // AUD
                    TimezoneId = 4, // Australia/Sydney
                },
                new Location
                {
                    Id = 6,
                    CompanyId = 6, // Glow & Glam
                    Name = "Paris Boutique",
                    CurrencyId = 2, // EUR
                    TimezoneId = 6, // Europe/Paris
                },
                new Location
                {
                    Id = 7,
                    CompanyId = 7, // Fun Time Toys
                    Name = "Mumbai Store",
                    CurrencyId = 7, // INR
                    TimezoneId = 7, // Asia/Mumbai
                },
                new Location
                {
                    Id = 8,
                    CompanyId = 1, // Tech Solutions Inc.
                    Name = "Zurich Tech Hub",
                    CurrencyId = 8, // CHF
                    TimezoneId = 8, // Europe/Zurich
                }
            );

            // Seed Address for Locations (owned type)
            modelBuilder.Entity<Location>()
                .OwnsOne(l => l.Address)
                .HasData(
                    new
                    {
                        LocationId = 1,
                        Address1 = "123 Innovation Way",
                        Address2 = "Tech Park, Suite 100",
                        City = "Silicon City",
                        State = "CA",
                        Country = "USA",
                        ZipCode = "94016"
                    },
                    new
                    {
                        LocationId = 2,
                        Address1 = "456 Style Avenue",
                        Address2 = "Fashion Mall, Unit 22",
                        City = "Fashionville",
                        State = "NY",
                        Country = "USA",
                        ZipCode = "10001"
                    },
                    new
                    {
                        LocationId = 3,
                        Address1 = "789 Earth Street",
                        Address2 = "Industrial Zone, Gate 5",
                        City = "Eco City",
                        State = "GA",
                        Country = "USA",
                        ZipCode = "30303"
                    },
                    new
                    {
                        LocationId = 4,
                        Address1 = "101 Literary Lane",
                        Address2 = "Off Charing Cross Rd",
                        City = "London",
                        State = "London",
                        Country = "UK",
                        ZipCode = "WC1B 3PA"
                    },
                    new
                    {
                        LocationId = 5,
                        Address1 = "222 Trail Road",
                        Address2 = "Near Blue Mountains Entry",
                        City = "Sydney",
                        State = "NSW",
                        Country = "Australia",
                        ZipCode = "2000"
                    },
                    new
                    {
                        LocationId = 6,
                        Address1 = "333 Radiant Road",
                        Address2 = "Galerie Vivienne",
                        City = "Paris",
                        State = "Paris",
                        Country = "France",
                        ZipCode = "75002"
                    },
                    new
                    {
                        LocationId = 7,
                        Address1 = "444 Playful Place",
                        Address2 = "Linking Road, Bandra",
                        City = "Mumbai",
                        State = "MH",
                        Country = "India",
                        ZipCode = "400002"
                    },
                    new
                    {
                        LocationId = 8,
                        Address1 = "555 Tech Park",
                        Address2 = "Innovation Center, Floor 3",
                        City = "Zurich",
                        State = "",
                        Country = "Switzerland",
                        ZipCode = "8002"
                    }
                );

            // Seed OrderHeaders (to link with Invoices)
            modelBuilder.Entity<OrderHeader>().HasData(
                new OrderHeader
                {
                    Id = 1,
                    ApplicationUserId = null,
                    CustomerId = 1,
                    OrderDate = new DateTime(2025, 4, 1),
                    ShippingDate = new DateTime(2025, 4, 3),
                    EstimatedDelivery = new DateTime(2025, 4, 5),
                    Subtotal = 599.99m,
                    Tax = 48.00m,
                    Discount = 13.00m,
                    OrderTotal = 649.99m,
                    AmountPaid = 649.99m,
                    AmountDue = 0.00m,
                    ShippingCharges = 15.00m,
                    OrderStatus = "Shipped",
                    PaymentStatus = "Paid",
                    DeliveryStatus = "InTransit",
                    ShippingMethod = "Standard",
                    DeliveryMethod = "Ground",
                    TrackingNumber = "TRK123456",
                    Carrier = "UPS",
                    PaymentDate = new DateTime(2025, 4, 1),
                    PaymentDueDate = new DateOnly(2025, 4, 30),
                    ShippingContactPhone = "555-0101",
                    ShippingContactName = "John Doe",
                    PaymentMethod = "CreditCard",
                    CustomerNotes = "Deliver to front porch",
                },
                new OrderHeader
                {
                    Id = 2,
                    ApplicationUserId = null,
                    CustomerId = 2,
                    OrderDate = new DateTime(2025, 4, 2),
                    ShippingDate = null,
                    EstimatedDelivery = new DateTime(2025, 4, 7),
                    Subtotal = 119.99m,
                    Tax = 9.60m,
                    Discount = 0.00m,
                    OrderTotal = 129.59m,
                    AmountPaid = 0.00m,
                    AmountDue = 129.59m,
                    ShippingCharges = 0.00m,
                    OrderStatus = "Processing",
                    PaymentStatus = "Pending",
                    DeliveryStatus = "Pending",
                    ShippingMethod = "Express",
                    DeliveryMethod = "Air",
                    TrackingNumber = null,
                    Carrier = null,
                    PaymentDate = null,
                    PaymentDueDate = new DateOnly(2025, 5, 2),
                    ShippingContactPhone = "555-0102",
                    ShippingContactName = "Jane Smith",
                    PaymentMethod = "PayPal",
                    CustomerNotes = null,
                },
                new OrderHeader
                {
                    Id = 3,
                    ApplicationUserId = null,
                    CustomerId = 2,
                    OrderDate = new DateTime(2025, 4, 3),
                    ShippingDate = new DateTime(2025, 4, 5),
                    EstimatedDelivery = new DateTime(2025, 4, 7),
                    Subtotal = 735.99m,
                    Tax = 58.88m,
                    Discount = 14.88m,
                    OrderTotal = 799.99m,
                    AmountPaid = 799.99m,
                    AmountDue = 0.00m,
                    ShippingCharges = 20.00m,
                    OrderStatus = "Delivered",
                    PaymentStatus = "Paid",
                    DeliveryStatus = "Delivered",
                    ShippingMethod = "Standard",
                    DeliveryMethod = "Ground",
                    TrackingNumber = "TRK789012",
                    Carrier = "FedEx",
                    PaymentDate = new DateTime(2025, 4, 3),
                    PaymentDueDate = new DateOnly(2025, 5, 3),
                    ShippingContactPhone = "555-0103",
                    ShippingContactName = "Hiroshi Tanaka",
                    PaymentMethod = "CreditCard",
                    CustomerNotes = "Leave at reception",
                },
                new OrderHeader
                {
                    Id = 4,
                    ApplicationUserId = null,
                    CustomerId = 3,
                    OrderDate = new DateTime(2025, 4, 4),
                    ShippingDate = new DateTime(2025, 4, 6),
                    EstimatedDelivery = new DateTime(2025, 4, 8),
                    Subtotal = 100.00m,
                    Tax = 8.00m,
                    Discount = 0.00m,
                    OrderTotal = 108.00m,
                    AmountPaid = 108.00m,
                    AmountDue = 0.00m,
                    ShippingCharges = 0.00m,
                    OrderStatus = "Shipped",
                    PaymentStatus = "Paid",
                    DeliveryStatus = "InTransit",
                    ShippingMethod = "Express",
                    DeliveryMethod = "Air",
                    TrackingNumber = "TRK345678",
                    Carrier = "DHL",
                    PaymentDate = new DateTime(2025, 4, 4),
                    PaymentDueDate = new DateOnly(2025, 5, 4),
                    ShippingContactPhone = "555-0104",
                    ShippingContactName = "Emma Brown",
                    PaymentMethod = "DebitCard",
                    CustomerNotes = null,
                },
                new OrderHeader
                {
                    Id = 5,
                    ApplicationUserId = null,
                    CustomerId = 4,
                    OrderDate = new DateTime(2025, 4, 5),
                    ShippingDate = null,
                    EstimatedDelivery = new DateTime(2025, 4, 10),
                    Subtotal = 70.00m,
                    Tax = 5.60m,
                    Discount = 0.00m,
                    OrderTotal = 75.60m,
                    AmountPaid = 0.00m,
                    AmountDue = 75.60m,
                    ShippingCharges = 0.00m,
                    OrderStatus = "Processing",
                    PaymentStatus = "Pending",
                    DeliveryStatus = "Pending",
                    ShippingMethod = "Standard",
                    DeliveryMethod = "Ground",
                    TrackingNumber = null,
                    Carrier = null,
                    PaymentDate = null,
                    PaymentDueDate = new DateOnly(2025, 5, 5),
                    ShippingContactPhone = "555-0105",
                    ShippingContactName = "Liam Johnson",
                    PaymentMethod = "PayPal",
                    CustomerNotes = "Fragile items",
                },
                new OrderHeader
                {
                    Id = 6,
                    ApplicationUserId = null,
                    CustomerId = 4,
                    OrderDate = new DateTime(2025, 4, 6),
                    ShippingDate = new DateTime(2025, 4, 8),
                    EstimatedDelivery = new DateTime(2025, 4, 10),
                    Subtotal = 64.99m,
                    Tax = 5.20m,
                    Discount = 0.00m,
                    OrderTotal = 70.19m,
                    AmountPaid = 70.19m,
                    AmountDue = 0.00m,
                    ShippingCharges = 0.00m,
                    OrderStatus = "Shipped",
                    PaymentStatus = "Paid",
                    DeliveryStatus = "InTransit",
                    ShippingMethod = "Standard",
                    DeliveryMethod = "Ground",
                    TrackingNumber = "TRK901234",
                    Carrier = "UPS",
                    PaymentDate = new DateTime(2025, 4, 6),
                    PaymentDueDate = new DateOnly(2025, 5, 6),
                    ShippingContactPhone = "555-0106",
                    ShippingContactName = "Sophie Martin",
                    PaymentMethod = "CreditCard",
                    CustomerNotes = null,
                },
                new OrderHeader
                {
                    Id = 7,
                    ApplicationUserId = null,
                    CustomerId = 4,
                    OrderDate = new DateTime(2025, 4, 7),
                    ShippingDate = new DateTime(2025, 4, 9),
                    EstimatedDelivery = new DateTime(2025, 4, 11),
                    Subtotal = 14.99m,
                    Tax = 1.20m,
                    Discount = 0.00m,
                    OrderTotal = 16.19m,
                    AmountPaid = 16.19m,
                    AmountDue = 0.00m,
                    ShippingCharges = 0.00m,
                    OrderStatus = "Delivered",
                    PaymentStatus = "Paid",
                    DeliveryStatus = "Delivered",
                    ShippingMethod = "Express",
                    DeliveryMethod = "Air",
                    TrackingNumber = "TRK567890",
                    Carrier = "FedEx",
                    PaymentDate = new DateTime(2025, 4, 7),
                    PaymentDueDate = new DateOnly(2025, 5, 7),
                    ShippingContactPhone = "555-0107",
                    ShippingContactName = "Arjun Patel",
                    PaymentMethod = "DebitCard",
                    CustomerNotes = "Urgent delivery",
                },
                new OrderHeader
                {
                    Id = 8,
                    ApplicationUserId = null,
                    CustomerId = 5,
                    OrderDate = new DateTime(2025, 4, 8),
                    ShippingDate = null,
                    EstimatedDelivery = new DateTime(2025, 4, 13),
                    Subtotal = 139.99m,
                    Tax = 11.20m,
                    Discount = 0.00m,
                    OrderTotal = 151.19m,
                    AmountPaid = 0.00m,
                    AmountDue = 151.19m,
                    ShippingCharges = 0.00m,
                    OrderStatus = "Processing",
                    PaymentStatus = "Pending",
                    DeliveryStatus = "Pending",
                    ShippingMethod = "Standard",
                    DeliveryMethod = "Ground",
                    TrackingNumber = null,
                    Carrier = null,
                    PaymentDate = null,
                    PaymentDueDate = new DateOnly(2025, 5, 8),
                    ShippingContactPhone = "555-0108",
                    ShippingContactName = "Clara Fischer",
                    PaymentMethod = "CreditCard",
                    CustomerNotes = null,
                },
                  new OrderHeader
                  {
                      Id = 9,
                      ApplicationUserId = null,
                      CustomerId = 6,
                      OrderDate = new DateTime(2025, 4, 8),
                      ShippingDate = null,
                      EstimatedDelivery = new DateTime(2025, 4, 13),
                      Subtotal = 139.99m,
                      Tax = 11.20m,
                      Discount = 0.00m,
                      OrderTotal = 151.19m,
                      AmountPaid = 0.00m,
                      AmountDue = 151.19m,
                      ShippingCharges = 0.00m,
                      OrderStatus = "Processing",
                      PaymentStatus = "Pending",
                      DeliveryStatus = "Pending",
                      ShippingMethod = "Standard",
                      DeliveryMethod = "Ground",
                      TrackingNumber = null,
                      Carrier = null,
                      PaymentDate = null,
                      PaymentDueDate = new DateOnly(2025, 5, 8),
                      ShippingContactPhone = "555-0109",
                      ShippingContactName = "Clara Fischer",
                      PaymentMethod = "PayPal",
                      CustomerNotes = "Gift wrap required",
                  }
            );

            // Seed Shipping Address for OrderHeaders (owned type)
            modelBuilder.Entity<OrderHeader>()
             .OwnsOne(l => l.ShipToAddress)
             .HasData(
                 new
                 {
                     OrderHeaderId = 1,
                     ShippingAddress1 = "123 Maple St",
                     ShippingAddress2 = "Suite 100",
                     ShippingCity = "Springfield",
                     ShippingState = "IL",
                     ShippingCountry = "USA",
                     ShippingZipCode = "94016"
                 },
                 new
                 {
                     OrderHeaderId = 2,
                     ShippingAddress1 = "456 Style Avenue",
                     ShippingAddress2 = "Oak Ave",
                     ShippingCity = "London",
                     ShippingState = "NY",
                     ShippingCountry = "USA",
                     ShippingZipCode = "10001"
                 },
                 new
                 {
                     OrderHeaderId = 3,
                     ShippingAddress1 = "789 Earth Street",
                     ShippingAddress2 = "Industrial Zone Ave ",
                     ShippingCity = "Eco City",
                     ShippingState = "GA",
                     ShippingCountry = "USA",
                     ShippingZipCode = "30303"
                 },
                 new
                 {
                     OrderHeaderId = 4,
                     ShippingAddress1 = "101 Literary Lane",
                     ShippingAddress2 = "Off Charing Cross Rd",
                     ShippingCity = "London",
                     ShippingState = "London",
                     ShippingCountry = "UK",
                     ShippingZipCode = "WC1B 3PA"
                 },
                 new
                 {
                     OrderHeaderId = 5,
                     ShippingAddress1 = "222 Trail Road",
                     ShippingAddress2 = "Near Blue Mountains Entry",
                     ShippingCity = "Sydney",
                     ShippingState = "NSW",
                     ShippingCountry = "Australia",
                     ShippingZipCode = "2000"
                 },
                 new
                 {
                     OrderHeaderId = 6,
                     ShippingAddress1 = "333 Radiant Road",
                     ShippingAddress2 = "Galerie Vivienne",
                     ShippingCity = "Paris",
                     ShippingState = "Paris",
                     ShippingCountry = "France",
                     ShippingZipCode = "75002"
                 },
                 new
                 {
                     OrderHeaderId = 7,
                     ShippingAddress1 = "444 Playful Place",
                     ShippingAddress2 = "Linking Road, Bandra",
                     ShippingCity = "Mumbai",
                     ShippingState = "MH",
                     ShippingCountry = "India",
                     ShippingZipCode = "400002"
                 },
                 new
                 {
                     OrderHeaderId = 8,
                     ShippingAddress1 = "555 Tech Park",
                     ShippingAddress2 = "Innovation Center, Floor 3",
                     ShippingCity = "Zurich",
                     ShippingState = "",
                     ShippingCountry = "Switzerland",
                     ShippingZipCode = "8002"
                 },
                 new
                 {
                     OrderHeaderId = 9,
                     ShippingAddress1 = "777 Skyline Boulevard",
                     ShippingAddress2 = "Sky Tower, Apt 905",
                     ShippingCity = "Toronto",
                     ShippingState = "ON",
                     ShippingCountry = "Canada",
                     ShippingZipCode = "M5V 2T6"
                 }
             );

            // Seed Billing Address for OrderHeaders (owned type)
            modelBuilder.Entity<OrderHeader>()
             .OwnsOne(l => l.BillToAddress)
             .HasData(
                 new
                 {
                     OrderHeaderId = 1,
                     BillingAddress1 = "123 Maple St",
                     BillingAddress2 = "Suite 100",
                     BillingCity = "Springfield",
                     BillingState = "IL",
                     BillingCountry = "USA",
                     BillingZipCode = "94016"
                 },
                 new
                 {
                     OrderHeaderId = 2,
                     BillingAddress1 = "456 Style Avenue",
                     BillingAddress2 = "Oak Ave",
                     BillingCity = "London",
                     BillingState = "NY",
                     BillingCountry = "USA",
                     BillingZipCode = "10001"
                 },
                 new
                 {
                     OrderHeaderId = 3,
                     BillingAddress1 = "789 Earth Street",
                     BillingAddress2 = "Industrial Zone Ave ",
                     BillingCity = "Eco City",
                     BillingState = "GA",
                     BillingCountry = "USA",
                     BillingZipCode = "30303"
                 },
                 new
                 {
                     OrderHeaderId = 4,
                     BillingAddress1 = "101 Literary Lane",
                     BillingAddress2 = "Off Charing Cross Rd",
                     BillingCity = "London",
                     BillingState = "London",
                     BillingCountry = "UK",
                     BillingZipCode = "WC1B 3PA"
                 },
                 new
                 {
                     OrderHeaderId = 5,
                     BillingAddress1 = "222 Trail Road",
                     BillingAddress2 = "Near Blue Mountains Entry",
                     BillingCity = "Sydney",
                     BillingState = "NSW",
                     BillingCountry = "Australia",
                     BillingZipCode = "2000"
                 },
                 new
                 {
                     OrderHeaderId = 6,
                     BillingAddress1 = "333 Radiant Road",
                     BillingAddress2 = "Galerie Vivienne",
                     BillingCity = "Paris",
                     BillingState = "Paris",
                     BillingCountry = "France",
                     BillingZipCode = "75002"
                 },
                 new
                 {
                     OrderHeaderId = 7,
                     BillingAddress1 = "444 Playful Place",
                     BillingAddress2 = "Linking Road, Bandra",
                     BillingCity = "Mumbai",
                     BillingState = "MH",
                     BillingCountry = "India",
                     BillingZipCode = "400002"
                 },
                 new
                 {
                     OrderHeaderId = 8,
                     BillingAddress1 = "555 Tech Park",
                     BillingAddress2 = "Innovation Center, Floor 3",
                     BillingCity = "Zurich",
                     BillingState = "",
                     BillingCountry = "Switzerland",
                     BillingZipCode = "8002"
                 },
                 new
                 {
                     OrderHeaderId = 9,
                     BillingAddress1 = "777 Skyline Boulevard",
                     BillingAddress2 = "Sky Tower, Apt 905",
                     BillingCity = "Toronto",
                     BillingState = "ON",
                     BillingCountry = "Canada",
                     BillingZipCode = "M5V 2T6"
                 }
             );

            // Seed OrderDetails (10 seeds)
            modelBuilder.Entity<OrderDetail>().HasData(
                new OrderDetail
                {
                    Id = 1,
                    OrderHeaderId = 1, // John Doe's order
                    ProductId = 1, // Smart TV 55 inch
                    Count = 1,
                    Price = 649.99, // DiscountPrice from Product
                },
                new OrderDetail
                {
                    Id = 2,
                    OrderHeaderId = 2, // Jane Smith's order
                    ProductId = 5, // Leather Handbag
                    Count = 1,
                    Price = 129.99, // DiscountPrice
                },
                new OrderDetail
                {
                    Id = 3,
                    OrderHeaderId = 3, // Hiroshi Tanaka's order
                    ProductId = 6, // DSLR Camera
                    Count = 1,
                    Price = 799.99, // DiscountPrice
                },
                new OrderDetail
                {
                    Id = 4,
                    OrderHeaderId = 4, // Emma Brown's order
                    ProductId = 9, // Hiking Boots
                    Count = 1,
                    Price = 109.95, // DiscountPrice
                },
                new OrderDetail
                {
                    Id = 5,
                    OrderHeaderId = 5, // Liam Johnson's order
                    ProductId = 10, // Skincare Set
                    Count = 1,
                    Price = 75.99, // DiscountPrice
                },
                new OrderDetail
                {
                    Id = 6,
                    OrderHeaderId = 6, // Sophie Martin's order
                    ProductId = 11, // Learning Robot
                    Count = 1,
                    Price = 69.99, // DiscountPrice
                },
                new OrderDetail
                {
                    Id = 7,
                    OrderHeaderId = 7, // Arjun Patel's order
                    ProductId = 8, // Historical Fiction Book
                    Count = 1,
                    Price = 15.99, // DiscountPrice
                },
                new OrderDetail
                {
                    Id = 8,
                    OrderHeaderId = 8, // Clara Fischer's order
                    ProductId = 13, // Smart Home Kit
                    Count = 1,
                    Price = 149.99, // DiscountPrice

                },
                new OrderDetail
                {
                    Id = 9,
                    OrderHeaderId = 9, // Clara Fischer's order
                    ProductId = 13, // Smart Home Kit
                    Count = 1,
                    Price = 149.99, // DiscountPrice

                },
                new OrderDetail
                {
                    Id = 10,
                    OrderHeaderId = 9, // John Doe's order (additional item)
                    ProductId = 12, // Smartphone Gimbal
                    Count = 1,
                    Price = 69.99, // DiscountPrice
                },
                new OrderDetail
                {
                    Id = 11,
                    OrderHeaderId = 3, // Hiroshi Tanaka's order (additional item)
                    ProductId = 4, // Ultra-Slim Laptop
                    Count = 1,
                    Price = 899.99, // DiscountPrice
                }
            );

            // Seed OrderActivityLogs
            modelBuilder.Entity<OrderActivityLog>().HasData(
                new OrderActivityLog
                {
                    Id = 1,
                    OrderHeaderId = 1,
                    Timestamp = new DateTime(2025, 4, 1, 10, 0, 0),
                    User = "john.doe",
                    ActivityType = ActivityType.OrderCreated,
                    Description = "Order placed by customer",
                    Details = "{\"CustomerId\": 1}"
                },
                new OrderActivityLog
                {
                    Id = 2,
                    OrderHeaderId = 1,
                    Timestamp = new DateTime(2025, 4, 1, 10, 5, 0),
                    User = "john.doe",
                    ActivityType = ActivityType.PaymentProcessed,
                    Description = "Payment completed via CreditCard",
                    Details = "{\"Amount\": 649.99}"
                },
                new OrderActivityLog
                {
                    Id = 3,
                    OrderHeaderId = 1,
                    Timestamp = new DateTime(2025, 4, 3, 9, 0, 0),
                    User = "system",
                    ActivityType = ActivityType.ShippingUpdated,
                    Description = "Order shipped via UPS",
                    Details = "{\"TrackingNumber\": \"TRK123456\"}"
                },
                new OrderActivityLog
                {
                    Id = 4,
                    OrderHeaderId = 2,
                    Timestamp = new DateTime(2025, 4, 2, 11, 0, 0),
                    User = "jane.smith",
                    ActivityType = ActivityType.OrderCreated,
                    Description = "Order placed by customer",
                    Details = "{\"CustomerId\": 2}"
                },
                new OrderActivityLog
                {
                    Id = 5,
                    OrderHeaderId = 3,
                    Timestamp = new DateTime(2025, 4, 3, 12, 0, 0),
                    User = "hiroshi.tanaka",
                    ActivityType = ActivityType.OrderCreated,
                    Description = "Order placed by customer",
                    Details = "{\"CustomerId\": 2}"
                },
                new OrderActivityLog
                {
                    Id = 6,
                    OrderHeaderId = 3,
                    Timestamp = new DateTime(2025, 4, 3, 12, 10, 0),
                    User = "hiroshi.tanaka",
                    ActivityType = ActivityType.PaymentProcessed,
                    Description = "Payment completed via CreditCard",
                    Details = "{\"Amount\": 799.99}"
                },
                new OrderActivityLog
                {
                    Id = 7,
                    OrderHeaderId = 3,
                    Timestamp = new DateTime(2025, 4, 5, 8, 0, 0),
                    User = "system",
                    ActivityType = ActivityType.ShippingUpdated,
                    Description = "Order shipped via FedEx",
                    Details = "{\"TrackingNumber\": \"TRK789012\"}"
                },
                new OrderActivityLog
                {
                    Id = 8,
                    OrderHeaderId = 4,
                    Timestamp = new DateTime(2025, 4, 4, 14, 0, 0),
                    User = "emma.brown",
                    ActivityType = ActivityType.OrderCreated,
                    Description = "Order placed by customer",
                    Details = "{\"CustomerId\": 3}"
                },
                new OrderActivityLog
                {
                    Id = 9,
                    OrderHeaderId = 4,
                    Timestamp = new DateTime(2025, 4, 4, 14, 5, 0),
                    User = "emma.brown",
                    ActivityType = ActivityType.PaymentProcessed,
                    Description = "Payment completed via DebitCard",
                    Details = "{\"Amount\": 109.95}"
                },
                new OrderActivityLog
                {
                    Id = 10,
                    OrderHeaderId = 4,
                    Timestamp = new DateTime(2025, 4, 6, 10, 0, 0),
                    User = "system",
                    ActivityType = ActivityType.ShippingUpdated,
                    Description = "Order shipped via DHL",
                    Details = "{\"TrackingNumber\": \"TRK345678\"}"
                },
                new OrderActivityLog
                {
                    Id = 11,
                    OrderHeaderId = 5,
                    Timestamp = new DateTime(2025, 4, 5, 15, 0, 0),
                    User = "liam.johnson",
                    ActivityType = ActivityType.OrderCreated,
                    Description = "Order placed by customer",
                    Details = "{\"CustomerId\": 4}"
                },
                new OrderActivityLog
                {
                    Id = 12,
                    OrderHeaderId = 6,
                    Timestamp = new DateTime(2025, 4, 6, 16, 0, 0),
                    User = "sophie.martin",
                    ActivityType = ActivityType.OrderCreated,
                    Description = "Order placed by customer",
                    Details = "{\"CustomerId\": 4}"
                },
                new OrderActivityLog
                {
                    Id = 13,
                    OrderHeaderId = 6,
                    Timestamp = new DateTime(2025, 4, 6, 16, 5, 0),
                    User = "sophie.martin",
                    ActivityType = ActivityType.PaymentProcessed,
                    Description = "Payment completed via CreditCard",
                    Details = "{\"Amount\": 69.99}"
                },
                new OrderActivityLog
                {
                    Id = 14,
                    OrderHeaderId = 6,
                    Timestamp = new DateTime(2025, 4, 8, 11, 0, 0),
                    User = "system",
                    ActivityType = ActivityType.ShippingUpdated,
                    Description = "Order shipped via UPS",
                    Details = "{\"TrackingNumber\": \"TRK901234\"}"
                },
                new OrderActivityLog
                {
                    Id = 15,
                    OrderHeaderId = 7,
                    Timestamp = new DateTime(2025, 4, 7, 17, 0, 0),
                    User = "arjun.patel",
                    ActivityType = ActivityType.OrderCreated,
                    Description = "Order placed by customer",
                    Details = "{\"CustomerId\": 4}"
                },
                new OrderActivityLog
                {
                    Id = 16,
                    OrderHeaderId = 7,
                    Timestamp = new DateTime(2025, 4, 7, 17, 5, 0),
                    User = "arjun.patel",
                    ActivityType = ActivityType.PaymentProcessed,
                    Description = "Payment completed via DebitCard",
                    Details = "{\"Amount\": 15.99}"
                },
                new OrderActivityLog
                {
                    Id = 17,
                    OrderHeaderId = 7,
                    Timestamp = new DateTime(2025, 4, 9, 9, 0, 0),
                    User = "system",
                    ActivityType = ActivityType.ShippingUpdated,
                    Description = "Order shipped via FedEx",
                    Details = "{\"TrackingNumber\": \"TRK567890\"}"
                },
                new OrderActivityLog
                {
                    Id = 18,
                    OrderHeaderId = 8,
                    Timestamp = new DateTime(2025, 4, 8, 18, 0, 0),
                    User = "clara.fischer",
                    ActivityType = ActivityType.OrderCreated,
                    Description = "Order placed by customer",
                    Details = "{\"CustomerId\": 5}"
                },
                new OrderActivityLog
                {
                    Id = 19,
                    OrderHeaderId = 9,
                    Timestamp = new DateTime(2025, 4, 8, 19, 0, 0),
                    User = "clara.fischer",
                    ActivityType = ActivityType.OrderCreated,
                    Description = "Order placed by customer",
                    Details = "{\"CustomerId\": 6}"
                }
            );

            // Seed Invoices
            modelBuilder.Entity<Invoice>().HasData(
                new Invoice
                {
                    Id = 1,
                    InvoiceNumber = "INV-2025-001",
                    PONumber = "PO-001",
                    IssueDate = new DateTime(2025, 4, 1),
                    PaymentDue = new DateTime(2025, 4, 30),
                    Status = InvoiceStatus.Paid,
                    InvoiceType = InvoiceType.Standard,
                    Notes = "Thank you for your purchase!",
                    PaymentTerms = "Net 30",
                    PaymentMethod = "Credit Card",
                    CustomerId = 1, // John Doe
                    CompanyId = 1, // Tech Solutions Inc.
                    LocationId = 1, // Silicon City Office
                    OrderId = 1, // Links to OrderHeader
                    Subtotal = 649.99m,
                    Discount = 0m,
                    Tax = 52.00m,
                    ShippingAmount = 20.00m,
                    PaidAmount = 721.99m,
                    TotalAmount = 721.99m,
                    ExternalReference = "REF-001"
                },
                new Invoice
                {
                    Id = 2,
                    InvoiceNumber = "INV-2025-002",
                    PONumber = "PO-002",
                    IssueDate = new DateTime(2025, 4, 2),
                    PaymentDue = new DateTime(2025, 5, 2),
                    Status = InvoiceStatus.Sent,
                    InvoiceType = InvoiceType.Standard,
                    Notes = "Please pay by due date.",
                    PaymentTerms = "Net 30",
                    PaymentMethod = "Bank Transfer",
                    CustomerId = 2, // Jane Smith
                    CompanyId = 2, // Fashion Forward Ltd.
                    LocationId = 2, // Fashionville Store
                    OrderId = 2,
                    Subtotal = 129.99m,
                    Discount = 10.00m,
                    Tax = 10.40m,
                    ShippingAmount = 15.00m,
                    PaidAmount = 0m,
                    TotalAmount = 145.39m,
                    ExternalReference = "REF-002"
                },
                new Invoice
                {
                    Id = 3,
                    InvoiceNumber = "INV-2025-003",
                    PONumber = "PO-003",
                    IssueDate = new DateTime(2025, 4, 3),
                    PaymentDue = new DateTime(2025, 5, 3),
                    Status = InvoiceStatus.Paid,
                    InvoiceType = InvoiceType.Proforma,
                    Notes = "Proforma invoice for approval.",
                    PaymentTerms = "Net 30",
                    PaymentMethod = "Credit Card",
                    CustomerId = 3, // Hiroshi Tanaka
                    CompanyId = 1, // Tech Solutions Inc.
                    LocationId = 1, // Silicon City Office
                    OrderId = 3,
                    Subtotal = 799.99m,
                    Discount = 0m,
                    Tax = 64.00m,
                    ShippingAmount = 25.00m,
                    PaidAmount = 888.99m,
                    TotalAmount = 888.99m,
                    ExternalReference = "REF-003"
                },
                new Invoice
                {
                    Id = 4,
                    InvoiceNumber = "INV-2025-004",
                    PONumber = "PO-004",
                    IssueDate = new DateTime(2025, 4, 4),
                    PaymentDue = new DateTime(2025, 5, 4),
                    Status = InvoiceStatus.PartiallyPaid,
                    InvoiceType = InvoiceType.Standard,
                    Notes = "Partial payment received.",
                    PaymentTerms = "Net 30",
                    PaymentMethod = "Bank Transfer",
                    CustomerId = 4, // Emma Brown
                    CompanyId = 5, // Adventure Gear Corp.
                    LocationId = 5, // Sydney Outlet
                    OrderId = 4,
                    Subtotal = 109.95m,
                    Discount = 0m,
                    Tax = 8.80m,
                    ShippingAmount = 10.00m,
                    PaidAmount = 50.00m,
                    TotalAmount = 128.75m,
                    ExternalReference = "REF-004"
                },
                new Invoice
                {
                    Id = 5,
                    InvoiceNumber = "INV-2025-005",
                    PONumber = "PO-005",
                    IssueDate = new DateTime(2025, 4, 5),
                    PaymentDue = new DateTime(2025, 5, 5),
                    Status = InvoiceStatus.Draft,
                    InvoiceType = InvoiceType.Recurring,
                    Notes = "Recurring invoice for monthly subscription.",
                    PaymentTerms = "Net 30",
                    PaymentMethod = "Direct Debit",
                    CustomerId = 5, // Liam Johnson
                    CompanyId = 6, // Glow & Glam
                    LocationId = 6, // Paris Boutique
                    OrderId = 5,
                    Subtotal = 75.99m,
                    Discount = 0m,
                    Tax = 6.08m,
                    ShippingAmount = 0m,
                    PaidAmount = 0m,
                    TotalAmount = 82.07m,
                    ExternalReference = "REF-005"
                },
                new Invoice
                {
                    Id = 6,
                    InvoiceNumber = "INV-2025-006",
                    PONumber = "PO-006",
                    IssueDate = new DateTime(2025, 4, 6),
                    PaymentDue = new DateTime(2025, 5, 6),
                    Status = InvoiceStatus.Overdue,
                    InvoiceType = InvoiceType.Standard,
                    Notes = "Payment overdue, please settle ASAP.",
                    PaymentTerms = "Net 30",
                    PaymentMethod = "Credit Card",
                    CustomerId = 6, // Sophie Martin
                    CompanyId = 7, // Fun Time Toys
                    LocationId = 7, // Mumbai Store
                    OrderId = 6,
                    Subtotal = 69.99m,
                    Discount = 0m,
                    Tax = 5.60m,
                    ShippingAmount = 8.00m,
                    PaidAmount = 0m,
                    TotalAmount = 83.59m,
                    ExternalReference = "REF-006"
                },
                new Invoice
                {
                    Id = 7,
                    InvoiceNumber = "INV-2025-007",
                    PONumber = "PO-007",
                    IssueDate = new DateTime(2025, 4, 7),
                    PaymentDue = new DateTime(2025, 5, 7),
                    Status = InvoiceStatus.Paid,
                    InvoiceType = InvoiceType.CreditNote,
                    Notes = "Credit note for returned item.",
                    PaymentTerms = "Immediate",
                    PaymentMethod = "Refund",
                    CustomerId = 7, // Arjun Patel
                    CompanyId = 4, // Global Reads
                    LocationId = 4, // London Bookstore
                    OrderId = 7,
                    Subtotal = -15.99m,
                    Discount = 0m,
                    Tax = -1.28m,
                    ShippingAmount = 0m,
                    PaidAmount = -17.27m,
                    TotalAmount = -17.27m,
                    ExternalReference = "REF-007"
                },
                new Invoice
                {
                    Id = 8,
                    InvoiceNumber = "INV-2025-008",
                    PONumber = "PO-008",
                    IssueDate = new DateTime(2025, 4, 8),
                    PaymentDue = new DateTime(2025, 5, 8),
                    Status = InvoiceStatus.Sent,
                    InvoiceType = InvoiceType.Standard,
                    Notes = "Invoice for recent purchase.",
                    PaymentTerms = "Net 30",
                    PaymentMethod = "Bank Transfer",
                    CustomerId = 8, // Clara Fischer
                    CompanyId = 1, // Tech Solutions Inc.
                    LocationId = 8, // Zurich Tech Hub
                    OrderId = 8,
                    Subtotal = 149.99m,
                    Discount = 0m,
                    Tax = 12.00m,
                    ShippingAmount = 15.00m,
                    PaidAmount = 0m,
                    TotalAmount = 176.99m,
                    ExternalReference = "REF-008"
                }
            );

            // Seed InvoiceItems (10 seeds, distributed across Invoices)
            modelBuilder.Entity<InvoiceItem>().HasData(
                new InvoiceItem { Id = 1, InvoiceId = 1, Service = "Smart TV 55 inch", Description = "55-inch 4K Smart TV with HDR", Unit = "Unit", Price = 649.99m, Amount = 649.99m },
                new InvoiceItem { Id = 2, InvoiceId = 2, Service = "Leather Handbag", Description = "Premium black leather handbag", Unit = "Unit", Price = 129.99m, Amount = 129.99m },
                new InvoiceItem { Id = 3, InvoiceId = 3, Service = "DSLR Camera", Description = "24.1MP DSLR camera with 18-55mm lens", Unit = "Unit", Price = 799.99m, Amount = 799.99m },
                new InvoiceItem { Id = 4, InvoiceId = 4, Service = "Hiking Boots", Description = "Waterproof hiking boots size US 9", Unit = "Unit", Price = 109.95m, Amount = 109.95m },
                new InvoiceItem { Id = 5, InvoiceId = 5, Service = "Skincare Set", Description = "Anti-aging skincare collection for normal skin", Unit = "Unit", Price = 75.99m, Amount = 75.99m },
                new InvoiceItem { Id = 6, InvoiceId = 6, Service = "Learning Robot", Description = "Interactive learning robot for kids", Unit = "Unit", Price = 69.99m, Amount = 69.99m },
                new InvoiceItem { Id = 7, InvoiceId = 7, Service = "Book Return", Description = "Credit for returned historical fiction book", Unit = "Unit", Price = -15.99m, Amount = -15.99m },
                new InvoiceItem { Id = 8, InvoiceId = 8, Service = "Smart Home Kit", Description = "Smart home starter kit with hub and bulbs", Unit = "Unit", Price = 149.99m, Amount = 149.99m },
                new InvoiceItem { Id = 9, InvoiceId = 1, Service = "Extended Warranty", Description = "2-year extended warranty for Smart TV", Unit = "Unit", Price = 49.99m, Amount = 49.99m },
                new InvoiceItem { Id = 10, InvoiceId = 3, Service = "Camera Tripod", Description = "Tripod accessory for DSLR camera", Unit = "Unit", Price = 29.99m, Amount = 29.99m }
            );

            // Seed InvoiceAttachments 
            modelBuilder.Entity<InvoiceAttachments>().HasData(
                new InvoiceAttachments { Id = 1, InvoiceId = 1, AttachmentName = "Invoice_INV-2025-001.pdf", AttachmentContent = "/files/invoices/INV-2025-001.pdf" },
                new InvoiceAttachments { Id = 2, InvoiceId = 2, AttachmentName = "Invoice_INV-2025-002.pdf", AttachmentContent = "/files/invoices/INV-2025-002.pdf" },
                new InvoiceAttachments { Id = 3, InvoiceId = 3, AttachmentName = "Invoice_INV-2025-003.pdf", AttachmentContent = "/files/invoices/INV-2025-003.pdf" },
                new InvoiceAttachments { Id = 4, InvoiceId = 4, AttachmentName = "Invoice_INV-2025-004.pdf", AttachmentContent = "/files/invoices/INV-2025-004.pdf" },
                new InvoiceAttachments { Id = 5, InvoiceId = 5, AttachmentName = "Invoice_INV-2025-005.pdf", AttachmentContent = "/files/invoices/INV-2025-005.pdf" },
                new InvoiceAttachments { Id = 6, InvoiceId = 6, AttachmentName = "Invoice_INV-2025-006.pdf", AttachmentContent = "/files/invoices/INV-2025-006.pdf" },
                new InvoiceAttachments { Id = 7, InvoiceId = 7, AttachmentName = "CreditNote_INV-2025-007.pdf", AttachmentContent = "/files/invoices/INV-2025-007.pdf" },
                new InvoiceAttachments { Id = 8, InvoiceId = 8, AttachmentName = "Invoice_INV-2025-008.pdf", AttachmentContent = "/files/invoices/INV-2025-008.pdf" }
            );

            // Seed TaxDetails 
            modelBuilder.Entity<TaxDetail>().HasData(
                new TaxDetail { Id = 1, InvoiceId = 1, TaxType = "VAT", Rate = 8.00m, Amount = 52.00m },
                new TaxDetail { Id = 2, InvoiceId = 2, TaxType = "GST", Rate = 8.00m, Amount = 10.40m, },
                new TaxDetail { Id = 3, InvoiceId = 3, TaxType = "Consumption Tax", Rate = 8.00m, Amount = 64.00m },
                new TaxDetail { Id = 4, InvoiceId = 4, TaxType = "GST", Rate = 8.00m, Amount = 8.80m },
                new TaxDetail { Id = 5, InvoiceId = 5, TaxType = "VAT", Rate = 8.00m, Amount = 6.08m },
                new TaxDetail { Id = 6, InvoiceId = 6, TaxType = "GST", Rate = 8.00m, Amount = 5.60m },
                new TaxDetail { Id = 7, InvoiceId = 7, TaxType = "VAT", Rate = 8.00m, Amount = -1.28m },
                new TaxDetail { Id = 8, InvoiceId = 8, TaxType = "VAT", Rate = 8.00m, Amount = 12.00m }
            );
        }
    }
}
