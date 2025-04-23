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
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstablishedYear = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brand", x => x.Id);
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
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    DiscountPrice = table.Column<double>(type: "float", nullable: false),
                    IsDiscounted = table.Column<bool>(type: "bit", nullable: false),
                    DiscountStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiscountEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    AllowBackorder = table.Column<bool>(type: "bit", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    WeightInKg = table.Column<double>(type: "float", nullable: false),
                    WidthInCm = table.Column<double>(type: "float", nullable: false),
                    HeightInCm = table.Column<double>(type: "float", nullable: false),
                    LengthInCm = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsNewArrival = table.Column<bool>(type: "bit", nullable: false),
                    IsTrending = table.Column<bool>(type: "bit", nullable: false),
                    MetaTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Views = table.Column<int>(type: "int", nullable: false),
                    SoldCount = table.Column<int>(type: "int", nullable: false),
                    AverageRating = table.Column<double>(type: "float", nullable: false),
                    TotalReviews = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_Products_Brand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "ProductSpecification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSpecification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSpecification_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    TagName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTag_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    VariantName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SKU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    DiscountPrice = table.Column<double>(type: "float", nullable: true),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVariant_Products_ProductId",
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
                table: "Brand",
                columns: new[] { "Id", "Country", "CreatedBy", "CreatedDate", "Deleted", "Description", "EstablishedYear", "IsActive", "LogoUrl", "Name", "Slug", "UpdatedBy", "UpdatedDate", "WebsiteUrl" },
                values: new object[,]
                {
                    { 1, "United States", null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(4939), false, "Innovative technology solutions for everyday life.", 2005, true, "/images/brands/tech-solutions-logo.png", "Tech Solutions", "tech-solutions", null, null, "https://www.techsolutions.com" },
                    { 2, "France", null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(4946), false, "Trendy and comfortable clothing for all occasions.", 2010, true, "/images/brands/fashion-forward-logo.png", "Fashion Forward", "fashion-forward", null, null, "https://www.fashionforward.com" },
                    { 3, "Canada", null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(4949), false, "Eco-friendly products for sustainable living.", 2015, true, "/images/brands/green-living-logo.png", "Green Living", "green-living", null, null, "https://www.greenliving.com" },
                    { 4, "United Kingdom", null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(4952), false, "Quality books from authors around the world.", 1995, true, "/images/brands/global-reads-logo.png", "Global Reads", "global-reads", null, null, "https://www.globalreads.com" },
                    { 5, "Australia", null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(4954), false, "High-quality equipment for outdoor enthusiasts.", 2008, true, "/images/brands/adventure-gear-logo.png", "Adventure Gear", "adventure-gear", null, null, "https://www.adventuregear.com" },
                    { 6, "South Korea", null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(4956), false, "Premium beauty products for radiant skin.", 2012, true, "/images/brands/glow-and-glam-logo.png", "Glow & Glam", "glow-and-glam", null, null, "https://www.glowandglam.com" },
                    { 7, "Germany", null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(4958), false, "Creative and educational toys for children of all ages.", 2000, true, "/images/brands/fun-time-toys-logo.png", "Fun Time Toys", "fun-time-toys", null, null, "https://www.funtimetoys.com" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Deleted", "Description", "DisplayOrder", "IsActive", "Name", "ParentCategoryId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5157), false, "Explore the latest gadgets and electronic devices.", 1, true, "Electronics", null, null, null },
                    { 2, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5161), false, "Discover stylish clothing and accessories for all occasions.", 2, true, "Apparel", null, null, null },
                    { 3, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5163), false, "Find everything you need for your home and outdoor spaces.", 3, true, "Home & Garden", null, null, null },
                    { 4, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5164), false, "Immerse yourself in captivating stories and knowledge.", 4, true, "Books", null, null, null },
                    { 5, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5166), false, "Gear up for your active lifestyle and outdoor adventures.", 5, true, "Sports & Outdoors", null, null, null },
                    { 6, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5168), false, "Enhance your natural beauty and well-being.", 6, true, "Beauty & Personal Care", null, null, null },
                    { 7, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5170), false, "Unleash fun and creativity for all ages.", 7, true, "Toys & Games", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "City", "CreatedBy", "CreatedDate", "Deleted", "Name", "PhoneNumber", "PostalCode", "State", "StreetAddress", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Silicon City", null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5243), false, "Tech Solutions Inc.", "555-123-4567", "94016", "CA", "123 Innovation Way", null, null },
                    { 2, "Fashionville", null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5248), false, "Fashion Forward Ltd.", "212-987-6543", "10001", "NY", "456 Style Avenue", null, null },
                    { 3, "Eco City", null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5251), false, "Green Living Co.", "404-555-7890", "30303", "GA", "789 Earth Street", null, null },
                    { 4, "Booktown", null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5252), false, "Global Reads", "312-555-1122", "60602", "IL", "101 Literary Lane", null, null },
                    { 5, "Outdoorsville", null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5254), false, "Adventure Gear Corp.", "720-555-3344", "80202", "CO", "222 Trail Road", null, null },
                    { 6, "Cosmetic City", null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5256), false, "Glow & Glam", "310-555-0011", "90210", "CA", "333 Radiant Road", null, null },
                    { 7, "Toyland", null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5258), false, "Fun Time Toys", "718-555-9988", "11201", "NY", "444 Playful Place", null, null }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Deleted", "Description", "DisplayOrder", "IsActive", "Name", "ParentCategoryId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 8, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5171), false, "The latest smartphones from top brands.", 1, true, "Smartphones", 1, null, null },
                    { 9, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5173), false, "Powerful laptops for work and play.", 2, true, "Laptops", 1, null, null },
                    { 10, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5175), false, "High-definition televisions for home entertainment.", 3, true, "Televisions", 1, null, null },
                    { 11, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5177), false, "Stylish clothing for men.", 1, true, "Menswear", 2, null, null },
                    { 12, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5179), false, "Trendy clothing for women.", 2, true, "Womenswear", 2, null, null },
                    { 13, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5180), false, "Essential appliances for your kitchen.", 1, true, "Kitchen Appliances", 3, null, null },
                    { 14, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5182), false, "Tools for maintaining your garden.", 2, true, "Garden Tools", 3, null, null },
                    { 15, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5184), false, "Imaginative and engaging fictional works.", 1, true, "Fiction", 4, null, null },
                    { 16, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5185), false, "Informative and factual books on various topics.", 2, true, "Non-Fiction", 4, null, null },
                    { 17, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5187), false, "Gear for your outdoor adventures.", 1, true, "Camping & Hiking", 5, null, null },
                    { 18, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5189), false, "Equipment and accessories for your fitness journey.", 2, true, "Fitness", 5, null, null }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AllowBackorder", "AverageRating", "Barcode", "BrandId", "CategoryId", "CreatedBy", "CreatedDate", "Deleted", "Description", "DiscountEndDate", "DiscountPrice", "DiscountStartDate", "HeightInCm", "IsActive", "IsDiscounted", "IsFeatured", "IsNewArrival", "IsTrending", "LengthInCm", "MetaDescription", "MetaTitle", "Price", "SKU", "ShortDescription", "SoldCount", "StockQuantity", "Title", "TotalReviews", "UpdatedBy", "UpdatedDate", "Views", "WeightInKg", "WidthInCm" },
                values: new object[,]
                {
                    { 6, false, 4.9000000000000004, "234567890123", 1, 1, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5354), false, "Capture life's special moments with exceptional clarity using our premium DSLR camera. Features a 24.1 megapixel CMOS sensor, 4K video recording, and includes a versatile 18-55mm lens. Perfect for both photography enthusiasts and those looking to elevate their photography skills.", new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 799.99000000000001, new DateTime(2025, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 10.0, true, true, true, false, false, 7.7999999999999998, "Professional-grade DSLR camera with 24.1MP sensor and 4K video capability for stunning photos and videos.", "Premium DSLR Camera with Lens | Tech Solutions", 899.99000000000001, "ELC-CAM-006", "24.1MP DSLR camera with 4K video and 18-55mm lens", 42, 20, "Premium Digital SLR Camera with 18-55mm Lens", 36, null, null, 1680, 0.69999999999999996, 12.9 },
                    { 7, true, 4.7000000000000002, "890123456789", 3, 3, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5360), false, "Indulge in the refreshing taste of premium organic green tea with our curated gift set. Includes 6 distinct varieties of hand-picked green tea leaves packaged in elegant tins. Perfect for tea enthusiasts or as a thoughtful gift for special occasions.", null, 45.0, null, 8.0, true, false, false, true, false, 20.0, "Premium selection of 6 organic green tea varieties presented in an elegant gift box.", "Organic Green Tea Gift Set | Green Living", 45.0, "HGN-TEA-007", "Gift set of 6 premium organic green tea varieties", 38, 40, "Organic Green Tea Gift Set (Variety Pack)", 22, null, null, 890, 0.5, 25.0 },
                    { 10, true, 4.7000000000000002, "678901234567", 6, 6, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5377), false, "Turn back the clock with our comprehensive anti-aging skincare collection. This five-piece set includes cleanser, toner, day cream with SPF 30, night serum, and eye cream, all formulated with powerful peptides, antioxidants, and hyaluronic acid to reduce fine lines and restore youthful radiance.", new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 75.989999999999995, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 15.0, true, true, true, true, true, 8.0, "Complete 5-piece anti-aging skincare routine with powerful ingredients for visibly younger-looking skin.", "Anti-Aging Skincare Collection | Glow & Glam", 89.989999999999995, "BPC-AAGE-010", "5-piece anti-aging skincare set with peptides and antioxidants", 110, 30, "Anti-Aging Skincare Collection Set", 82, null, null, 2800, 0.59999999999999998, 20.0 },
                    { 11, false, 4.9000000000000004, "789012345670", 7, 7, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5384), false, "Spark your child's interest in STEM with our interactive learning robot. Programmable through an easy-to-use app, this friendly robot teaches coding concepts, plays educational games, and responds to voice commands. With multiple sensors and expandable capabilities, it grows with your child's skills.", new DateTime(2025, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 69.989999999999995, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 22.0, true, true, true, true, true, 12.0, "Educational robot that makes learning to code fun and engaging for children ages 6-12.", "Interactive Learning Robot for Kids | Fun Time Toys", 79.989999999999995, "TOY-ROBOT-011", "Educational programmable robot that teaches coding to children", 135, 25, "Interactive Learning Robot for Kids", 98, null, null, 3500, 0.5, 15.0 },
                    { 13, true, 4.7999999999999998, "234567890124", 1, 1, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5395), false, "Transform your house into a smart home with our comprehensive starter kit. Includes a smart hub, two smart plugs, two motion sensors, and three smart light bulbs that can all be controlled via app or voice commands. Compatible with major voice assistants for seamless integration with your existing devices.", new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 149.99000000000001, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 25.0, true, true, true, true, true, 15.0, "Complete solution to begin automating your home with smart devices controllable via app or voice.", "Smart Home Starter Kit | Tech Solutions", 179.99000000000001, "ELC-SMHM-013", "Complete smart home kit with hub, plugs, sensors and bulbs", 65, 20, "Smart Home Starter Kit", 38, null, null, 2900, 1.2, 30.0 }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Deleted", "ImageUrl", "ProductId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 14, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5641), false, "/images/products/camera_main.jpg", 6, null, null },
                    { 15, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5642), false, "/images/products/camera_top.jpg", 6, null, null },
                    { 16, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5644), false, "/images/products/camera_lens.jpg", 6, null, null },
                    { 17, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5645), false, "/images/products/tea_set_complete.jpg", 7, null, null },
                    { 18, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5646), false, "/images/products/tea_tin_open.jpg", 7, null, null },
                    { 24, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5652), false, "/images/products/skincare_set.jpg", 10, null, null },
                    { 25, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5653), false, "/images/products/skincare_serum.jpg", 10, null, null },
                    { 26, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5655), false, "/images/products/skincare_cream.jpg", 10, null, null },
                    { 27, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5656), false, "/images/products/robot_front.jpg", 11, null, null },
                    { 28, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5657), false, "/images/products/robot_side.jpg", 11, null, null },
                    { 31, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5661), false, "/images/products/smarthome_kit.jpg", 13, null, null },
                    { 32, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5662), false, "/images/products/smarthome_hub.jpg", 13, null, null },
                    { 33, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5663), false, "/images/products/smarthome_bulb.jpg", 13, null, null }
                });

            migrationBuilder.InsertData(
                table: "ProductSpecification",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Deleted", "Key", "ProductId", "UpdatedBy", "UpdatedDate", "Value" },
                values: new object[,]
                {
                    { 23, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5769), false, "Megapixels", 6, null, null, "24.1 MP" },
                    { 24, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5770), false, "Sensor Type", 6, null, null, "CMOS" },
                    { 25, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5772), false, "Video Resolution", 6, null, null, "4K" },
                    { 26, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5773), false, "Lens", 6, null, null, "18-55mm" },
                    { 27, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5774), false, "ISO Range", 6, null, null, "100-25600" },
                    { 28, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5775), false, "Varieties", 7, null, null, "6" },
                    { 29, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5776), false, "Organic", 7, null, null, "Yes" },
                    { 30, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5778), false, "Weight", 7, null, null, "300g total (50g each)" },
                    { 31, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5779), false, "Packaging", 7, null, null, "Metal tins in gift box" },
                    { 40, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5792), false, "Pieces", 10, null, null, "5" },
                    { 41, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5794), false, "Skin Type", 10, null, null, "All" },
                    { 42, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5795), false, "Key Ingredients", 10, null, null, "Peptides, Hyaluronic Acid, Antioxidants" },
                    { 43, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5800), false, "SPF", 10, null, null, "30 (Day Cream)" },
                    { 44, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5801), false, "Age Range", 11, null, null, "6-12 years" },
                    { 45, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5802), false, "Programmable", 11, null, null, "Yes" },
                    { 46, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5804), false, "Battery Life", 11, null, null, "4 hours" },
                    { 47, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5805), false, "Connectivity", 11, null, null, "Bluetooth" },
                    { 48, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5806), false, "App Compatibility", 11, null, null, "iOS and Android" },
                    { 53, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5812), false, "Components", 13, null, null, "1 Hub, 2 Plugs, 2 Sensors, 3 Bulbs" },
                    { 54, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5814), false, "Connectivity", 13, null, null, "Wi-Fi, Bluetooth" },
                    { 55, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5815), false, "Voice Assistant Compatibility", 13, null, null, "Alexa, Google Assistant, Siri" },
                    { 56, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5816), false, "App Control", 13, null, null, "Yes" }
                });

            migrationBuilder.InsertData(
                table: "ProductTag",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Deleted", "ProductId", "TagName", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 19, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5926), false, 6, "Photography", null, null },
                    { 20, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5927), false, 6, "4K Video", null, null },
                    { 21, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5928), false, 6, "Featured", null, null },
                    { 22, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5929), false, 7, "Organic", null, null },
                    { 23, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5931), false, 7, "Gift Set", null, null },
                    { 24, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5932), false, 7, "New Arrival", null, null },
                    { 32, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5940), false, 10, "Anti-Aging", null, null },
                    { 33, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5941), false, 10, "Beauty", null, null },
                    { 34, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5942), false, 10, "New Arrival", null, null },
                    { 35, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5943), false, 10, "Trending", null, null },
                    { 36, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5945), false, 11, "Educational", null, null },
                    { 37, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5946), false, 11, "STEM", null, null },
                    { 38, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5947), false, 11, "Kids", null, null },
                    { 39, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5948), false, 11, "New Arrival", null, null },
                    { 43, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5953), false, 13, "Smart Home", null, null },
                    { 44, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5954), false, 13, "IoT", null, null },
                    { 45, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5956), false, 13, "New Arrival", null, null },
                    { 46, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5957), false, 13, "Trending", null, null }
                });

            migrationBuilder.InsertData(
                table: "ProductVariant",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Deleted", "DiscountPrice", "Price", "ProductId", "SKU", "StockQuantity", "UpdatedBy", "UpdatedDate", "VariantName" },
                values: new object[,]
                {
                    { 16, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(6066), false, 75.989999999999995, 89.989999999999995, 10, "BPC-AAGE-010-NORM", 15, null, null, "For Normal Skin" },
                    { 17, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(6068), false, 75.989999999999995, 89.989999999999995, 10, "BPC-AAGE-010-DRY", 10, null, null, "For Dry Skin" },
                    { 18, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(6070), false, 79.989999999999995, 94.989999999999995, 10, "BPC-AAGE-010-SENS", 5, null, null, "For Sensitive Skin" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AllowBackorder", "AverageRating", "Barcode", "BrandId", "CategoryId", "CreatedBy", "CreatedDate", "Deleted", "Description", "DiscountEndDate", "DiscountPrice", "DiscountStartDate", "HeightInCm", "IsActive", "IsDiscounted", "IsFeatured", "IsNewArrival", "IsTrending", "LengthInCm", "MetaDescription", "MetaTitle", "Price", "SKU", "ShortDescription", "SoldCount", "StockQuantity", "Title", "TotalReviews", "UpdatedBy", "UpdatedDate", "Views", "WeightInKg", "WidthInCm" },
                values: new object[,]
                {
                    { 1, false, 4.7000000000000002, "789012345678", 1, 10, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5308), false, "Immerse yourself in stunning visuals with this 55-inch 4K Ultra HD Smart TV. Featuring High Dynamic Range (HDR) for vibrant colors and deep contrast, built-in Wi-Fi, and access to all your favorite streaming apps. Enjoy a cinematic experience in the comfort of your living room.", new DateTime(2025, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 649.99000000000001, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 71.200000000000003, true, true, true, false, true, 8.3000000000000007, "Shop our 55-inch 4K UHD Smart TV with HDR technology for the ultimate home entertainment experience.", "55 inch 4K Smart TV | Tech Solutions", 699.99000000000001, "ELC-SMTV-001", "55-inch 4K Smart TV with HDR and built-in streaming apps", 120, 50, "Smart TV 55 inch 4K UHD with HDR", 85, null, null, 2500, 15.5, 123.5 },
                    { 2, true, 4.5, "456789012345", 2, 11, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5330), false, "Experience ultimate comfort with our premium 100% combed cotton men's t-shirt. Designed for a classic fit and exceptional softness, this navy blue tee is a versatile wardrobe staple perfect for everyday wear. Available in various sizes.", new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 16.989999999999998, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 70.0, true, true, false, false, false, 0.5, "Classic fit men's navy blue t-shirt made from 100% premium combed cotton for all-day comfort.", "Men's Premium Cotton T-Shirt | Fashion Forward", 20.0, "APP-MTSRT-002-NVY", "Premium soft cotton t-shirt for men in navy blue", 250, 100, "Premium Cotton T-Shirt - Mens (Navy Blue)", 120, null, null, 1800, 0.20000000000000001, 50.0 },
                    { 3, false, 4.7999999999999998, "123456789012", 3, 14, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5337), false, "Get your gardening tasks done with ease using our durable 3-piece garden tool set. Includes a sturdy trowel, hand fork, and cultivator, all featuring comfortable wooden handles for a secure grip. Perfect for both novice and experienced gardeners.", null, 40.0, null, 30.0, true, false, false, true, false, 5.0, "High-quality 3-piece garden tool set with comfortable wooden handles for all your gardening needs.", "Essential Garden Tool Set | Green Living", 40.0, "HGN-TLSET-003-WD", "3-piece garden tool set with wooden handles", 45, 30, "Essential Garden Tool Set (3-Piece with Wooden Handles)", 28, null, null, 950, 0.90000000000000002, 12.0 },
                    { 4, false, 4.5999999999999996, "567890123456", 1, 9, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5342), false, "Experience lightning-fast performance with our ultra-slim 15.6-inch laptop. Featuring a powerful processor, 512GB SSD storage, and 16GB RAM for seamless multitasking. The vibrant Full HD display and long-lasting battery make it perfect for work and entertainment on the go.", new DateTime(2025, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 899.99000000000001, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1.8, true, true, true, true, true, 24.199999999999999, "Powerful and portable 15.6-inch laptop with SSD storage for fast performance anywhere you go.", "Ultra-Slim 15.6\" Laptop | Tech Solutions", 999.99000000000001, "ELC-LPTOP-004", "15.6\" ultra-slim laptop with SSD and powerful performance", 85, 35, "Ultra-Slim Laptop 15.6\" with SSD", 62, null, null, 3200, 1.8, 35.600000000000001 },
                    { 5, false, 4.7999999999999998, "345678901234", 2, 12, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5348), false, "Add a touch of elegance to any outfit with our designer leather handbag. Crafted from premium genuine leather with a stylish gold-tone hardware and multiple interior compartments for organization. The adjustable shoulder strap and handle offer versatile carrying options.", new DateTime(2025, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 129.99000000000001, new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 25.0, true, true, true, false, true, 12.0, "Elegant black leather handbag with multiple compartments and versatile carrying options.", "Designer Black Leather Handbag | Fashion Forward", 149.99000000000001, "APP-HBAG-005-BLK", "Premium black leather handbag with gold accents", 68, 25, "Designer Leather Handbag - Women's (Black)", 45, null, null, 1950, 0.80000000000000004, 35.0 },
                    { 8, true, 4.5, "901234567890", 4, 15, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5366), false, "Journey back in time with this captivating historical fiction novel that uncovers the story of a lost dynasty. Set in the 16th century, the narrative weaves together adventure, romance, and political intrigue as a young scholar uncovers ancient secrets that could change the course of history.", new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 15.99, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2.5, true, true, false, true, true, 22.800000000000001, "Immerse yourself in a captivating tale of secrets, adventure, and intrigue set in the 16th century.", "The Forgotten Dynasty | Historical Fiction", 18.989999999999998, "BOK-HFIC-008", "Captivating historical fiction novel set in the 16th century", 72, 60, "Historical Fiction: 'The Forgotten Dynasty'", 48, null, null, 1250, 0.40000000000000002, 15.199999999999999 },
                    { 9, false, 4.7999999999999998, "012345678901", 5, 17, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5372), false, "Conquer any trail with confidence in our all-weather hiking boots. Featuring waterproof construction, superior grip rubber soles, and cushioned insoles for all-day comfort. The breathable membrane keeps feet dry while allowing moisture to escape, making these perfect for year-round outdoor adventures.", new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 109.95, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 15.0, true, true, false, false, true, 20.0, "Durable and waterproof hiking boots designed for maximum comfort on any terrain and in any weather.", "All-Weather Hiking Boots | Adventure Gear", 129.94999999999999, "SPT-HBOOT-009", "Waterproof and comfortable hiking boots for all terrains", 95, 45, "All-Weather Hiking Boots (Unisex)", 63, null, null, 2100, 1.2, 30.0 },
                    { 12, false, 4.5999999999999996, "123456789013", 1, 8, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5389), false, "Take your mobile videography to the next level with our 3-axis smartphone stabilizer gimbal. Featuring intelligent tracking, multiple shooting modes, and foldable design for easy portability. The rechargeable battery provides up to 12 hours of operation, perfect for content creators and travelers.", new DateTime(2025, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 69.989999999999995, new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 19.0, true, true, false, true, false, 5.0, "Professional 3-axis gimbal stabilizer for smooth, cinematic smartphone videos.", "Smartphone Stabilizer Gimbal | Tech Solutions", 85.0, "ELC-GIMB-012", "3-axis gimbal stabilizer for professional smartphone videos", 58, 40, "Smartphone Stabilizer Gimbal", 42, null, null, 1680, 0.40000000000000002, 12.0 }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Deleted", "ImageUrl", "ProductId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5625), false, "/images/products/smarttv_main.jpg", 1, null, null },
                    { 2, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5628), false, "/images/products/smarttv_side.jpg", 1, null, null },
                    { 3, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5629), false, "/images/products/smarttv_ports.jpg", 1, null, null },
                    { 4, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5630), false, "/images/products/tshirt_navy_front.jpg", 2, null, null },
                    { 5, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5632), false, "/images/products/tshirt_navy_back.jpg", 2, null, null },
                    { 6, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5633), false, "/images/products/garden_tool_set.jpg", 3, null, null },
                    { 7, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5634), false, "/images/products/gardentool_trowel.jpg", 3, null, null },
                    { 8, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5635), false, "/images/products/gardentool_fork.jpg", 3, null, null },
                    { 9, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5636), false, "/images/products/laptop_main.jpg", 4, null, null },
                    { 10, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5637), false, "/images/products/laptop_open.jpg", 4, null, null },
                    { 11, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5638), false, "/images/products/laptop_side.jpg", 4, null, null },
                    { 12, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5639), false, "/images/products/handbag_black_main.jpg", 5, null, null },
                    { 13, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5640), false, "/images/products/handbag_black_open.jpg", 5, null, null },
                    { 19, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5647), false, "/images/products/book_cover.jpg", 8, null, null },
                    { 20, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5648), false, "/images/products/book_back.jpg", 8, null, null },
                    { 21, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5649), false, "/images/products/hikingboots_pair.jpg", 9, null, null },
                    { 22, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5650), false, "/images/products/hikingboots_sole.jpg", 9, null, null },
                    { 23, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5651), false, "/images/products/hikingboots_side.jpg", 9, null, null },
                    { 29, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5658), false, "/images/products/gimbal_main.jpg", 12, null, null },
                    { 30, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5660), false, "/images/products/gimbal_folded.jpg", 12, null, null }
                });

            migrationBuilder.InsertData(
                table: "ProductSpecification",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Deleted", "Key", "ProductId", "UpdatedBy", "UpdatedDate", "Value" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5740), false, "Screen Size", 1, null, null, "55 inches" },
                    { 2, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5742), false, "Resolution", 1, null, null, "4K Ultra HD (3840 x 2160)" },
                    { 3, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5743), false, "Display Technology", 1, null, null, "LED" },
                    { 4, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5744), false, "HDR", 1, null, null, "Yes" },
                    { 5, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5746), false, "Smart TV", 1, null, null, "Yes" },
                    { 6, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5747), false, "Connectivity", 1, null, null, "Wi-Fi, Bluetooth, HDMI, USB" },
                    { 7, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5748), false, "Material", 2, null, null, "100% Combed Cotton" },
                    { 8, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5750), false, "Color", 2, null, null, "Navy Blue" },
                    { 9, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5751), false, "Care Instructions", 2, null, null, "Machine wash cold, tumble dry low" },
                    { 10, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5752), false, "Material", 3, null, null, "Stainless Steel with Wooden Handles" },
                    { 11, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5753), false, "Pieces", 3, null, null, "3" },
                    { 12, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5755), false, "Tool Length", 3, null, null, "30 cm" },
                    { 13, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5756), false, "Processor", 4, null, null, "Intel Core i7" },
                    { 14, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5757), false, "RAM", 4, null, null, "16 GB" },
                    { 15, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5758), false, "Storage", 4, null, null, "512 GB SSD" },
                    { 16, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5760), false, "Display", 4, null, null, "15.6-inch Full HD (1920 x 1080)" },
                    { 17, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5761), false, "Battery Life", 4, null, null, "Up to 10 hours" },
                    { 18, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5763), false, "Operating System", 4, null, null, "Windows 11" },
                    { 19, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5764), false, "Material", 5, null, null, "Genuine Leather" },
                    { 20, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5765), false, "Color", 5, null, null, "Black" },
                    { 21, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5767), false, "Dimensions", 5, null, null, "35 x 25 x 12 cm" },
                    { 22, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5768), false, "Hardware", 5, null, null, "Gold-tone" },
                    { 32, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5780), false, "Format", 8, null, null, "Hardcover" },
                    { 33, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5781), false, "Pages", 8, null, null, "384" },
                    { 34, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5782), false, "Genre", 8, null, null, "Historical Fiction" },
                    { 35, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5784), false, "Language", 8, null, null, "English" },
                    { 36, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5785), false, "Material", 9, null, null, "Waterproof Leather and Mesh" },
                    { 37, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5787), false, "Sole", 9, null, null, "Rubber with Multi-directional Traction" },
                    { 38, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5788), false, "Closure", 9, null, null, "Lace-up" },
                    { 39, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5790), false, "Gender", 9, null, null, "Unisex" },
                    { 49, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5807), false, "Axes", 12, null, null, "3-axis" },
                    { 50, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5808), false, "Battery Life", 12, null, null, "12 hours" },
                    { 51, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5810), false, "Compatibility", 12, null, null, "Most smartphones up to 6.7 inches" },
                    { 52, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5811), false, "Weight", 12, null, null, "400g" }
                });

            migrationBuilder.InsertData(
                table: "ProductTag",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Deleted", "ProductId", "TagName", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5901), false, 1, "4K", null, null },
                    { 2, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5904), false, 1, "Smart TV", null, null },
                    { 3, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5905), false, 1, "HDR", null, null },
                    { 4, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5907), false, 1, "Home Entertainment", null, null },
                    { 5, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5908), false, 2, "Men's Fashion", null, null },
                    { 6, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5909), false, 2, "Casual Wear", null, null },
                    { 7, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5910), false, 2, "Cotton", null, null },
                    { 8, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5912), false, 3, "Gardening", null, null },
                    { 9, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5914), false, 3, "Tools", null, null },
                    { 10, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5915), false, 3, "New Arrival", null, null },
                    { 11, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5916), false, 4, "Computing", null, null },
                    { 12, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5919), false, 4, "SSD", null, null },
                    { 13, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5920), false, 4, "Lightweight", null, null },
                    { 14, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5921), false, 4, "New Arrival", null, null },
                    { 15, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5922), false, 5, "Women's Fashion", null, null },
                    { 16, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5923), false, 5, "Leather", null, null },
                    { 17, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5924), false, 5, "Designer", null, null },
                    { 18, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5925), false, 5, "Trending", null, null },
                    { 25, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5933), false, 8, "Historical Fiction", null, null },
                    { 26, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5934), false, 8, "Bestseller", null, null },
                    { 27, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5935), false, 8, "New Arrival", null, null },
                    { 28, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5936), false, 9, "Outdoor", null, null },
                    { 29, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5937), false, 9, "Waterproof", null, null },
                    { 30, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5938), false, 9, "Unisex", null, null },
                    { 31, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5939), false, 9, "Trending", null, null },
                    { 40, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5949), false, 12, "Photography", null, null },
                    { 41, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5951), false, 12, "Accessories", null, null },
                    { 42, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(5952), false, 12, "New Arrival", null, null }
                });

            migrationBuilder.InsertData(
                table: "ProductVariant",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Deleted", "DiscountPrice", "Price", "ProductId", "SKU", "StockQuantity", "UpdatedBy", "UpdatedDate", "VariantName" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(6033), false, 16.989999999999998, 20.0, 2, "APP-MTSRT-002-NVY-S", 25, null, null, "Size - S" },
                    { 2, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(6040), false, 16.989999999999998, 20.0, 2, "APP-MTSRT-002-NVY-M", 30, null, null, "Size - M" },
                    { 3, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(6042), false, 16.989999999999998, 20.0, 2, "APP-MTSRT-002-NVY-L", 25, null, null, "Size - L" },
                    { 4, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(6044), false, 18.989999999999998, 22.0, 2, "APP-MTSRT-002-NVY-XL", 20, null, null, "Size - XL" },
                    { 5, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(6046), false, 129.99000000000001, 149.99000000000001, 5, "APP-HBAG-005-BLK", 15, null, null, "Color - Black" },
                    { 6, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(6047), false, 129.99000000000001, 149.99000000000001, 5, "APP-HBAG-005-BRN", 10, null, null, "Color - Brown" },
                    { 7, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(6049), false, 109.95, 129.94999999999999, 9, "SPT-HBOOT-009-07", 8, null, null, "Size - US 7 / EU 38" },
                    { 8, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(6051), false, 109.95, 129.94999999999999, 9, "SPT-HBOOT-009-08", 10, null, null, "Size - US 8 / EU 39" },
                    { 9, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(6053), false, 109.95, 129.94999999999999, 9, "SPT-HBOOT-009-09", 12, null, null, "Size - US 9 / EU 40" },
                    { 10, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(6055), false, 109.95, 129.94999999999999, 9, "SPT-HBOOT-009-10", 10, null, null, "Size - US 10 / EU 41" },
                    { 11, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(6056), false, 109.95, 129.94999999999999, 9, "SPT-HBOOT-009-11", 5, null, null, "Size - US 11 / EU 42" },
                    { 12, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(6058), false, 649.99000000000001, 699.99000000000001, 1, "ELC-SMTV-001-55", 30, null, null, "Size - 55 inch" },
                    { 13, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(6061), false, 849.99000000000001, 899.99000000000001, 1, "ELC-SMTV-001-65", 20, null, null, "Size - 65 inch" },
                    { 14, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(6063), false, 899.99000000000001, 999.99000000000001, 4, "ELC-LPTOP-004-16-512", 20, null, null, "RAM - 16GB / Storage - 512GB" },
                    { 15, null, new DateTime(2025, 4, 23, 14, 47, 21, 28, DateTimeKind.Utc).AddTicks(6065), false, 1199.99, 1299.99, 4, "ELC-LPTOP-004-32-1TB", 15, null, null, "RAM - 32GB / Storage - 1TB" }
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
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSpecification_ProductId",
                table: "ProductSpecification",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTag_ProductId",
                table: "ProductTag",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariant_ProductId",
                table: "ProductVariant",
                column: "ProductId");

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
                name: "ProductSpecification");

            migrationBuilder.DropTable(
                name: "ProductTag");

            migrationBuilder.DropTable(
                name: "ProductVariant");

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
                name: "Brand");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
