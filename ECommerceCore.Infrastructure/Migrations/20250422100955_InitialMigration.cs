using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerceCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthStates",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EmailOTP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SmsOTP = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PasswordVerified = table.Column<bool>(type: "bit", nullable: false),
                    EmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthTokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactUsSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUsSubmissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Price50 = table.Column<double>(type: "float", nullable: false),
                    Price100 = table.Column<double>(type: "float", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShippingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderTotal = table.Column<double>(type: "float", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Carrier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentIntenId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderHeaders_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WishlistItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishlistItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WishlistItems_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WishlistItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderHeaderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
                        column: x => x.OrderHeaderId,
                        principalTable: "OrderHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Deleted", "Description", "DisplayOrder", "IsActive", "Name", "ParentCategoryId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(8904), false, "Explore the latest gadgets and electronic devices.", 1, true, "Electronics", null, null, null },
                    { 2, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(8907), false, "Discover stylish clothing and accessories for all occasions.", 2, true, "Apparel", null, null, null },
                    { 3, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(8909), false, "Find everything you need for your home and outdoor spaces.", 3, true, "Home & Garden", null, null, null },
                    { 4, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(8910), false, "Immerse yourself in captivating stories and knowledge.", 4, true, "Books", null, null, null },
                    { 5, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(8911), false, "Gear up for your active lifestyle and outdoor adventures.", 5, true, "Sports & Outdoors", null, null, null },
                    { 6, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(8912), false, "Enhance your natural beauty and well-being.", 6, true, "Beauty & Personal Care", null, null, null },
                    { 7, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(8916), false, "Unleash fun and creativity for all ages.", 7, true, "Toys & Games", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "City", "CreatedBy", "CreatedDate", "Deleted", "Name", "PhoneNumber", "PostalCode", "State", "StreetAddress", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Silicon City", null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9099), false, "Tech Solutions Inc.", "555-123-4567", "94016", "CA", "123 Innovation Way", null, null },
                    { 2, "Fashionville", null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9102), false, "Fashion Forward Ltd.", "212-987-6543", "10001", "NY", "456 Style Avenue", null, null },
                    { 3, "Eco City", null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9104), false, "Green Living Co.", "404-555-7890", "30303", "GA", "789 Earth Street", null, null },
                    { 4, "Booktown", null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9106), false, "Global Reads", "312-555-1122", "60602", "IL", "101 Literary Lane", null, null },
                    { 5, "Outdoorsville", null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9107), false, "Adventure Gear Corp.", "720-555-3344", "80202", "CO", "222 Trail Road", null, null },
                    { 6, "Cosmetic City", null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9108), false, "Glow & Glam", "310-555-0011", "90210", "CA", "333 Radiant Road", null, null },
                    { 7, "Toyland", null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9110), false, "Fun Time Toys", "718-555-9988", "11201", "NY", "444 Playful Place", null, null }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Deleted", "Description", "DisplayOrder", "IsActive", "Name", "ParentCategoryId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 8, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(8918), false, "The latest smartphones from top brands.", 1, true, "Smartphones", 1, null, null },
                    { 9, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(8920), false, "Powerful laptops for work and play.", 2, true, "Laptops", 1, null, null },
                    { 10, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(8922), false, "High-definition televisions for home entertainment.", 3, true, "Televisions", 1, null, null },
                    { 11, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(8924), false, "Stylish clothing for men.", 1, true, "Menswear", 2, null, null },
                    { 12, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(8925), false, "Trendy clothing for women.", 2, true, "Womenswear", 2, null, null },
                    { 13, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(8928), false, "Essential appliances for your kitchen.", 1, true, "Kitchen Appliances", 3, null, null },
                    { 14, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(8931), false, "Tools for maintaining your garden.", 2, true, "Garden Tools", 3, null, null },
                    { 15, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(8932), false, "Imaginative and engaging fictional works.", 1, true, "Fiction", 4, null, null },
                    { 16, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(8934), false, "Informative and factual books on various topics.", 2, true, "Non-Fiction", 4, null, null },
                    { 17, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(8935), false, "Gear for your outdoor adventures.", 1, true, "Camping & Hiking", 5, null, null },
                    { 18, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(8936), false, "Equipment and accessories for your fitness journey.", 2, true, "Fitness", 5, null, null }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "CreatedBy", "CreatedDate", "Deleted", "Description", "ISBN", "ListPrice", "Price", "Price100", "Price50", "StockQuantity", "Title", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Tech Solutions Inc.", 1, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9143), false, "Immerse yourself in stunning visuals with this 55-inch 4K Ultra HD Smart TV. Featuring High Dynamic Range (HDR) for vibrant colors and deep contrast, built-in Wi-Fi, and access to all your favorite streaming apps. Enjoy a cinematic experience in the comfort of your living room.", "ELC-SMTV-001", 799.99000000000001, 699.99000000000001, 600.0, 650.0, 50, "Smart TV 55 inch 4K UHD with HDR", null, null },
                    { 2, "Fashion Forward Ltd.", 2, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9148), false, "Experience ultimate comfort with our premium 100% combed cotton men's t-shirt. Designed for a classic fit and exceptional softness, this navy blue tee is a versatile wardrobe staple perfect for everyday wear. Available in various sizes.", "APP-MTSRT-002-NVY", 25.0, 20.0, 15.0, 18.0, 100, "Premium Cotton T-Shirt - Mens (Navy Blue)", null, null },
                    { 3, "Green Living Co.", 3, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9150), false, "Get your gardening tasks done with ease using our durable 3-piece garden tool set. Includes a sturdy trowel, hand fork, and cultivator, all featuring comfortable wooden handles for a secure grip. Perfect for both novice and experienced gardeners.", "HGN-TLSET-003-WD", 49.950000000000003, 40.0, 30.0, 35.0, 30, "Essential Garden Tool Set (3-Piece with Wooden Handles)", null, null },
                    { 4, "A.B. Reader", 4, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9152), false, "Dive into the latest thrilling adventure featuring Detective Jane Doe as she unravels the secrets behind an ancient emerald tablet linked to a series of perplexing disappearances. Packed with suspense, twists, and turns that will keep you guessing until the very last page.", "BOK-MYST-004-EMT", 15.5, 12.0, 9.0, 10.0, 75, "The Mystery of the Emerald Tablet (A Detective Jane Doe Novel)", null, null },
                    { 5, "Adventure Gear Corp.", 5, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9153), false, "Embark on your next outdoor adventure with the Adventure Pro 4-person camping tent. Constructed with durable, waterproof materials and featuring a spacious interior, ventilation windows, and easy setup. Perfect for family camping, backpacking, and weekend getaways.", "SPT-CTNT-005-4P", 199.0, 175.0, 150.0, 160.0, 25, "Adventure Pro 4-Person Waterproof Camping Tent", null, null },
                    { 6, "Tech Solutions Inc.", 1, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9155), false, "Escape into your audio world with these premium noise-cancelling wireless Bluetooth headphones. Enjoy crystal-clear sound, deep bass, and up to 30 hours of playtime on a single charge. Features comfortable over-ear cups and intuitive controls. Ideal for travel, work, and relaxation.", "ELC-WHP-006-BLK", 149.0, 129.0, 100.0, 115.0, 60, "Noise-Cancelling Wireless Bluetooth Headphones (Black)", null, null },
                    { 7, "Fashion Forward Ltd.", 2, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9157), false, "Achieve your personal best with these lightweight and supportive performance running shoes for women. Designed with breathable mesh and responsive cushioning for a comfortable and energized run. Perfect for тренировки on the track or hitting the pavement.", "APP-WRSHOE-007-ABL", 89.989999999999995, 75.0, 65.0, 70.0, 90, "Performance Running Shoes - Womens (Aqua Blue)", null, null },
                    { 8, "Green Living Co.", 3, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9158), false, "Become the ultimate grill master with this versatile 3-burner propane BBQ grill. Features a convenient side burner for sauces and sides, a built-in thermometer for precise temperature control, and a durable weather-resistant cover to protect your investment.", "HGN-BBQ-008-PC", 299.0, 250.0, 200.0, 225.0, 20, "3-Burner Propane BBQ Grill with Side Burner & Cover", null, null },
                    { 9, "Various Authors", 4, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9160), false, "Embark on interstellar journeys with this captivating collection of classic science fiction short stories from visionary authors. Explore themes of space exploration, artificial intelligence, and the future of humanity in this must-have anthology for sci-fi enthusiasts.", "BOK-SCIFI-009-TTC", 18.75, 15.0, 11.0, 13.0, 55, "Timeless Tales of the Cosmos (A Science Fiction Anthology)", null, null },
                    { 10, "Adventure Gear Corp.", 5, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9162), false, "Conquer any trail with the Summit X full suspension mountain bike. Featuring lightweight aluminum frame, responsive suspension system, and 29-inch wheels for enhanced stability and control. Perfect for experienced riders seeking ultimate off-road performance.", "SPT-MTB-010-FS29", 1200.0, 1050.0, 850.0, 950.0, 15, "Summit X Full Suspension Mountain Bike (29 inch Wheels)", null, null },
                    { 11, "Glow & Glam", 6, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9163), false, "Replenish and revitalize your skin with our hydrating facial serum. Formulated with pure hyaluronic acid to lock in moisture, reduce the appearance of fine lines, and leave your complexion feeling smooth, plump, and radiant. Suitable for all skin types.", "BPC-FSERUM-011", 35.0, 28.0, 22.0, 25.0, 80, "Hydrating Facial Serum with Hyaluronic Acid", null, null },
                    { 12, "Fun Time Toys", 7, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9165), false, "Spark your child's imagination with this deluxe wooden train set. Featuring over 100 pieces including tracks, trains, bridges, figures, and scenery. Crafted from high-quality wood for lasting durability and endless hours of creative play. Perfect for ages 3 and up.", "TOY-TRSET-012", 79.989999999999995, 65.0, 55.0, 60.0, 40, "Deluxe Wooden Train Set (Over 100 Pieces)", null, null }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Deleted", "ImageUrl", "ProductId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9196), false, "/images/products/smarttv_main.jpg", 1, null, null },
                    { 2, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9198), false, "/images/products/smarttv_ports.jpg", 1, null, null },
                    { 3, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9199), false, "/images/products/mens_tshirt_navy.jpg", 2, null, null },
                    { 4, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9200), false, "/images/products/mens_tshirt_front.jpg", 2, null, null },
                    { 5, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9201), false, "/images/products/garden_tool_set.jpg", 3, null, null },
                    { 6, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9201), false, "/images/products/book_emerald_tablet.jpg", 4, null, null },
                    { 7, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9202), false, "/images/products/tent_pitched.jpg", 5, null, null },
                    { 8, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9203), false, "/images/products/tent_interior.jpg", 5, null, null },
                    { 9, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9204), false, "/images/products/headphones_black.jpg", 6, null, null },
                    { 10, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9354), false, "/images/products/womens_run_shoe_blue.jpg", 7, null, null },
                    { 11, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9356), false, "/images/products/bbq_grill.jpg", 8, null, null },
                    { 12, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9356), false, "/images/products/book_sci_fi_anthology.jpg", 9, null, null },
                    { 13, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9357), false, "/images/products/mountain_bike.jpg", 10, null, null },
                    { 14, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9358), false, "/images/products/facial_serum.jpg", 11, null, null },
                    { 15, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9359), false, "/images/products/train_set_layout.jpg", 12, null, null },
                    { 16, null, new DateTime(2025, 4, 22, 10, 9, 54, 406, DateTimeKind.Utc).AddTicks(9359), false, "/images/products/train_set_pieces.jpg", 12, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuthStates_ExpiresAt",
                table: "AuthStates",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuthStates_UserId",
                table: "AuthStates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthTokens_Token",
                table: "AuthTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthTokens_UserId",
                table: "AuthTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderHeaderId",
                table: "OrderDetails",
                column: "OrderHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_ApplicationUserId",
                table: "OrderHeaders",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_ApplicationUserId",
                table: "ShoppingCarts",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_ProductId",
                table: "ShoppingCarts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_ProductId",
                table: "WishlistItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_UserId",
                table: "WishlistItems",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuthStates");

            migrationBuilder.DropTable(
                name: "AuthTokens");

            migrationBuilder.DropTable(
                name: "ContactUsSubmissions");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "WishlistItems");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "OrderHeaders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
