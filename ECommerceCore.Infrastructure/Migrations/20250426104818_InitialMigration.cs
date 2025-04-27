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
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
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
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
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
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
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
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUsSubmissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Timezones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UtcOffset = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UtcOffsetString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timezones", x => x.Id);
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
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
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
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    TimezoneId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Locations_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Locations_Timezones_TimezoneId",
                        column: x => x.TimezoneId,
                        principalTable: "Timezones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
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
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
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
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
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
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
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
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
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
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
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
                name: "OrderHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShippingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TrackingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Carrier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentIntentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingAddress1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingCity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingContactName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_OrderHeaders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PONumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentDue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    InvoiceType = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentTerms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecurringInvoiceId = table.Column<int>(type: "int", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExternalReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId1 = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_Companies_CompanyId1",
                        column: x => x.CompanyId1,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invoices_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_OrderHeaders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "OrderHeaders",
                        principalColumn: "Id");
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
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "InvoiceAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttachmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceAttachments_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Service = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxDetails_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Brand",
                columns: new[] { "Id", "Country", "CreatedBy", "CreatedDate", "Description", "EstablishedYear", "IsActive", "IsDeleted", "LogoUrl", "Name", "Slug", "UpdatedBy", "UpdatedDate", "WebsiteUrl" },
                values: new object[,]
                {
                    { 1, "United States", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8156), "Innovative technology solutions for everyday life.", 2005, true, false, "/images/brands/tech-solutions-logo.png", "Tech Solutions", "tech-solutions", null, null, "https://www.techsolutions.com" },
                    { 2, "France", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8164), "Trendy and comfortable clothing for all occasions.", 2010, true, false, "/images/brands/fashion-forward-logo.png", "Fashion Forward", "fashion-forward", null, null, "https://www.fashionforward.com" },
                    { 3, "Canada", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8166), "Eco-friendly products for sustainable living.", 2015, true, false, "/images/brands/green-living-logo.png", "Green Living", "green-living", null, null, "https://www.greenliving.com" },
                    { 4, "United Kingdom", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8167), "Quality books from authors around the world.", 1995, true, false, "/images/brands/global-reads-logo.png", "Global Reads", "global-reads", null, null, "https://www.globalreads.com" },
                    { 5, "Australia", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8169), "High-quality equipment for outdoor enthusiasts.", 2008, true, false, "/images/brands/adventure-gear-logo.png", "Adventure Gear", "adventure-gear", null, null, "https://www.adventuregear.com" },
                    { 6, "South Korea", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8171), "Premium beauty products for radiant skin.", 2012, true, false, "/images/brands/glow-and-glam-logo.png", "Glow & Glam", "glow-and-glam", null, null, "https://www.glowandglam.com" },
                    { 7, "Germany", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8173), "Creative and educational toys for children of all ages.", 2000, true, false, "/images/brands/fun-time-toys-logo.png", "Fun Time Toys", "fun-time-toys", null, null, "https://www.funtimetoys.com" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "DisplayOrder", "IsActive", "IsDeleted", "Name", "ParentCategoryId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8361), "Explore the latest gadgets and electronic devices.", 1, true, false, "Electronics", null, null, null },
                    { 2, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8364), "Discover stylish clothing and accessories for all occasions.", 2, true, false, "Apparel", null, null, null },
                    { 3, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8366), "Find everything you need for your home and outdoor spaces.", 3, true, false, "Home & Garden", null, null, null },
                    { 4, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8367), "Immerse yourself in captivating stories and knowledge.", 4, true, false, "Books", null, null, null },
                    { 5, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8369), "Gear up for your active lifestyle and outdoor adventures.", 5, true, false, "Sports & Outdoors", null, null, null },
                    { 6, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8370), "Enhance your natural beauty and well-being.", 6, true, false, "Beauty & Personal Care", null, null, null },
                    { 7, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8371), "Unleash fun and creativity for all ages.", 7, true, false, "Toys & Games", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "City", "CreatedBy", "CreatedDate", "IsDeleted", "Name", "PhoneNumber", "PostalCode", "State", "StreetAddress", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Silicon City", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8505), false, "Tech Solutions Inc.", "555-123-4567", "94016", "CA", "123 Innovation Way", null, null },
                    { 2, "Fashionville", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8509), false, "Fashion Forward Ltd.", "212-987-6543", "10001", "NY", "456 Style Avenue", null, null },
                    { 3, "Eco City", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8511), false, "Green Living Co.", "404-555-7890", "30303", "GA", "789 Earth Street", null, null },
                    { 4, "Booktown", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8512), false, "Global Reads", "312-555-1122", "60602", "IL", "101 Literary Lane", null, null },
                    { 5, "Outdoorsville", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8514), false, "Adventure Gear Corp.", "720-555-3344", "80202", "CO", "222 Trail Road", null, null },
                    { 6, "Cosmetic City", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8515), false, "Glow & Glam", "310-555-0011", "90210", "CA", "333 Radiant Road", null, null },
                    { 7, "Toyland", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8516), false, "Fun Time Toys", "718-555-9988", "11201", "NY", "444 Playful Place", null, null }
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Code", "CreatedBy", "CreatedDate", "IsDeleted", "Name", "Symbol", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "USD", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9283), false, "US Dollar", "$", null, null },
                    { 2, "EUR", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9285), false, "Euro", "€", null, null },
                    { 3, "GBP", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9286), false, "British Pound", "£", null, null },
                    { 4, "CAD", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9287), false, "Canadian Dollar", "C$", null, null },
                    { 5, "AUD", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9288), false, "Australian Dollar", "A$", null, null },
                    { 6, "JPY", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9289), false, "Japanese Yen", "¥", null, null },
                    { 7, "INR", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9290), false, "Indian Rupee", "₹", null, null },
                    { 8, "CHF", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9291), false, "Swiss Franc", "Fr", null, null }
                });

            migrationBuilder.InsertData(
                table: "Timezones",
                columns: new[] { "Id", "Abbreviation", "CreatedBy", "CreatedDate", "IsDeleted", "Name", "UpdatedBy", "UpdatedDate", "UtcOffset", "UtcOffsetString" },
                values: new object[,]
                {
                    { 1, "EST", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9327), false, "America/New_York", null, null, "-05:00", "EST" },
                    { 2, "GMT", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9330), false, "Europe/London", null, null, "+00:00", "GMT" },
                    { 3, "JST", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9331), false, "Asia/Tokyo", null, null, "+09:00", "JST" },
                    { 4, "AEDT", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9332), false, "Australia/Sydney", null, null, "+10:00", "AEDT" },
                    { 5, "EST", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9334), false, "America/Toronto", null, null, "-05:00", "EST" },
                    { 6, "CET", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9426), false, "Europe/Paris", null, null, "+01:00", "CET" },
                    { 7, "IST", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9428), false, "Asia/Mumbai", null, null, "+05:30", "IST" },
                    { 8, "CET", null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9429), false, "Europe/Zurich", null, null, "+01:00", "CET" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "DisplayOrder", "IsActive", "IsDeleted", "Name", "ParentCategoryId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 8, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8372), "The latest smartphones from top brands.", 1, true, false, "Smartphones", 1, null, null },
                    { 9, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8373), "Powerful laptops for work and play.", 2, true, false, "Laptops", 1, null, null },
                    { 10, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8375), "High-definition televisions for home entertainment.", 3, true, false, "Televisions", 1, null, null },
                    { 11, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8376), "Stylish clothing for men.", 1, true, false, "Menswear", 2, null, null },
                    { 12, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8377), "Trendy clothing for women.", 2, true, false, "Womenswear", 2, null, null },
                    { 13, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8445), "Essential appliances for your kitchen.", 1, true, false, "Kitchen Appliances", 3, null, null },
                    { 14, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8447), "Tools for maintaining your garden.", 2, true, false, "Garden Tools", 3, null, null },
                    { 15, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8448), "Imaginative and engaging fictional works.", 1, true, false, "Fiction", 4, null, null },
                    { 16, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8449), "Informative and factual books on various topics.", 2, true, false, "Non-Fiction", 4, null, null },
                    { 17, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8450), "Gear for your outdoor adventures.", 1, true, false, "Camping & Hiking", 5, null, null },
                    { 18, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8452), "Equipment and accessories for your fitness journey.", 2, true, false, "Fitness", 5, null, null }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "CompanyId", "CreatedBy", "CreatedDate", "Email", "IsDeleted", "Name", "PhoneNumber", "UpdatedBy", "UpdatedDate", "Address1", "Address2", "City", "Country", "State", "ZipCode" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9468), "john.doe@example.com", false, "John Doe", "555-0101", null, null, "123 Maple St", "Apt 4B", "Springfield", "USA", "IL", "62701" },
                    { 2, 2, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9476), "jane.smith@example.com", false, "Jane Smith", "555-0102", null, null, "456 Oak Ave", "Suite 201", "London", "UK", "Greater London", "SW1A 1AA" },
                    { 3, 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9477), "hiroshi.tanaka@example.com", false, "Hiroshi Tanaka", "555-0103", null, null, "789 Sakura St", "2 Chome-1-1", "Tokyo", "Japan", "Tokyo", "100-0001" },
                    { 4, 5, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9478), "emma.brown@example.com", false, "Emma Brown", "555-0104", null, null, "101 Pine Rd", "Level 5", "Sydney", "Australia", "NSW", "2000" },
                    { 5, 6, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9480), "liam.johnson@example.com", false, "Liam Johnson", "555-0105", null, null, "202 Birch Ln", "Unit 12", "Toronto", "Canada", "ON", "M5V 2T7" },
                    { 6, 7, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9481), "sophie.martin@example.com", false, "Sophie Martin", "555-0106", null, null, "303 Cedar St", "Batiment C", "Paris", "France", "Île-de-France", "75001" },
                    { 7, 4, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9482), "arjun.patel@example.com", false, "Arjun Patel", "555-0107", null, null, "404 Elm Dr", "Near Main Gate", "Mumbai", "India", "MH", "400001" },
                    { 8, 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9483), "clara.fischer@example.com", false, "Clara Fischer", "555-0108", null, null, "505 Spruce Ct", "Block A", "Zurich", "Switzerland", "Zurich", "8001" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "CompanyId", "CreatedBy", "CreatedDate", "CurrencyId", "IsDeleted", "Name", "TimezoneId", "UpdatedBy", "UpdatedDate", "Address1", "Address2", "City", "Country", "State", "ZipCode" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9833), 1, false, "Silicon City Office", 1, null, null, "123 Innovation Way", "Tech Park, Suite 100", "Silicon City", "USA", "CA", "94016" },
                    { 2, 2, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9837), 1, false, "Fashionville Store", 1, null, null, "456 Style Avenue", "Fashion Mall, Unit 22", "Fashionville", "USA", "NY", "10001" },
                    { 3, 3, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9838), 1, false, "Eco City Warehouse", 1, null, null, "789 Earth Street", "Industrial Zone, Gate 5", "Eco City", "USA", "GA", "30303" },
                    { 4, 4, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9840), 3, false, "London Bookstore", 2, null, null, "101 Literary Lane", "Off Charing Cross Rd", "London", "UK", "London", "WC1B 3PA" },
                    { 5, 5, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9841), 5, false, "Sydney Outlet", 4, null, null, "222 Trail Road", "Near Blue Mountains Entry", "Sydney", "Australia", "NSW", "2000" },
                    { 6, 6, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9842), 2, false, "Paris Boutique", 6, null, null, "333 Radiant Road", "Galerie Vivienne", "Paris", "France", "Paris", "75002" },
                    { 7, 7, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9843), 7, false, "Mumbai Store", 7, null, null, "444 Playful Place", "Linking Road, Bandra", "Mumbai", "India", "MH", "400002" },
                    { 8, 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9845), 8, false, "Zurich Tech Hub", 8, null, null, "555 Tech Park", "Innovation Center, Floor 3", "Zurich", "Switzerland", "", "8002" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AllowBackorder", "AverageRating", "Barcode", "BrandId", "CategoryId", "CreatedBy", "CreatedDate", "Description", "DiscountEndDate", "DiscountPrice", "DiscountStartDate", "HeightInCm", "IsActive", "IsDeleted", "IsDiscounted", "IsFeatured", "IsNewArrival", "IsTrending", "LengthInCm", "MetaDescription", "MetaTitle", "Price", "SKU", "ShortDescription", "SoldCount", "StockQuantity", "Title", "TotalReviews", "UpdatedBy", "UpdatedDate", "Views", "WeightInKg", "WidthInCm" },
                values: new object[,]
                {
                    { 6, false, 4.9000000000000004, "234567890123", 1, 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8589), "Capture life's special moments with exceptional clarity using our premium DSLR camera. Features a 24.1 megapixel CMOS sensor, 4K video recording, and includes a versatile 18-55mm lens. Perfect for both photography enthusiasts and those looking to elevate their photography skills.", new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 799.99000000000001, new DateTime(2025, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 10.0, true, false, true, true, false, false, 7.7999999999999998, "Professional-grade DSLR camera with 24.1MP sensor and 4K video capability for stunning photos and videos.", "Premium DSLR Camera with Lens | Tech Solutions", 899.99000000000001, "ELC-CAM-006", "24.1MP DSLR camera with 4K video and 18-55mm lens", 42, 20, "Premium Digital SLR Camera with 18-55mm Lens", 36, null, null, 1680, 0.69999999999999996, 12.9 },
                    { 7, true, 4.7000000000000002, "890123456789", 3, 3, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8593), "Indulge in the refreshing taste of premium organic green tea with our curated gift set. Includes 6 distinct varieties of hand-picked green tea leaves packaged in elegant tins. Perfect for tea enthusiasts or as a thoughtful gift for special occasions.", null, 45.0, null, 8.0, true, false, false, false, true, false, 20.0, "Premium selection of 6 organic green tea varieties presented in an elegant gift box.", "Organic Green Tea Gift Set | Green Living", 45.0, "HGN-TEA-007", "Gift set of 6 premium organic green tea varieties", 38, 40, "Organic Green Tea Gift Set (Variety Pack)", 22, null, null, 890, 0.5, 25.0 },
                    { 10, true, 4.7000000000000002, "678901234567", 6, 6, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8606), "Turn back the clock with our comprehensive anti-aging skincare collection. This five-piece set includes cleanser, toner, day cream with SPF 30, night serum, and eye cream, all formulated with powerful peptides, antioxidants, and hyaluronic acid to reduce fine lines and restore youthful radiance.", new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 75.989999999999995, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 15.0, true, false, true, true, true, true, 8.0, "Complete 5-piece anti-aging skincare routine with powerful ingredients for visibly younger-looking skin.", "Anti-Aging Skincare Collection | Glow & Glam", 89.989999999999995, "BPC-AAGE-010", "5-piece anti-aging skincare set with peptides and antioxidants", 110, 30, "Anti-Aging Skincare Collection Set", 82, null, null, 2800, 0.59999999999999998, 20.0 },
                    { 11, false, 4.9000000000000004, "789012345670", 7, 7, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8610), "Spark your child's interest in STEM with our interactive learning robot. Programmable through an easy-to-use app, this friendly robot teaches coding concepts, plays educational games, and responds to voice commands. With multiple sensors and expandable capabilities, it grows with your child's skills.", new DateTime(2025, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 69.989999999999995, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 22.0, true, false, true, true, true, true, 12.0, "Educational robot that makes learning to code fun and engaging for children ages 6-12.", "Interactive Learning Robot for Kids | Fun Time Toys", 79.989999999999995, "TOY-ROBOT-011", "Educational programmable robot that teaches coding to children", 135, 25, "Interactive Learning Robot for Kids", 98, null, null, 3500, 0.5, 15.0 },
                    { 13, true, 4.7999999999999998, "234567890124", 1, 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8618), "Transform your house into a smart home with our comprehensive starter kit. Includes a smart hub, two smart plugs, two motion sensors, and three smart light bulbs that can all be controlled via app or voice commands. Compatible with major voice assistants for seamless integration with your existing devices.", new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 149.99000000000001, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 25.0, true, false, true, true, true, true, 15.0, "Complete solution to begin automating your home with smart devices controllable via app or voice.", "Smart Home Starter Kit | Tech Solutions", 179.99000000000001, "ELC-SMHM-013", "Complete smart home kit with hub, plugs, sensors and bulbs", 65, 20, "Smart Home Starter Kit", 38, null, null, 2900, 1.2, 30.0 }
                });

            migrationBuilder.InsertData(
                table: "OrderHeaders",
                columns: new[] { "Id", "ApplicationUserId", "Carrier", "CreatedBy", "CreatedDate", "CustomerId", "IsDeleted", "OrderDate", "OrderStatus", "OrderTotal", "PaymentDate", "PaymentDueDate", "PaymentIntentId", "PaymentStatus", "SessionId", "ShippingContactName", "ShippingContactPhone", "ShippingDate", "TrackingNumber", "UpdatedBy", "UpdatedDate", "ShippingAddress1", "ShippingAddress2", "ShippingCity", "ShippingCountry", "ShippingState", "ShippingZipCode" },
                values: new object[,]
                {
                    { 1, null, "UPS", null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(119), 1, false, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shipped", 649.99m, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 4, 30), null, "Paid", null, "John Doe", "555-0101", new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "TRK123456", null, null, "123 Maple St", "Suite 100", "Springfield", "USA", "IL", "94016" },
                    { 2, null, null, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(132), 2, false, new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Processing", 129.99m, new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 5, 2), null, "Pending", null, "Jane Smith", "555-0102", new DateTime(2025, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "456 Style Avenue", "Oak Ave", "London", "USA", "NY", "10001" },
                    { 3, null, "FedEx", null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(136), 2, false, new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Delivered", 799.99m, new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 5, 3), null, "Paid", null, "Hiroshi Tanaka", "555-0103", new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "TRK789012", null, null, "789 Earth Street", "Industrial Zone Ave ", "Eco City", "USA", "GA", "30303" },
                    { 4, null, "DHL", null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(139), 3, false, new DateTime(2025, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shipped", 109.95m, new DateTime(2025, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 5, 4), null, "Paid", null, "Emma Brown", "555-0104", new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "TRK345678", null, null, "101 Literary Lane", "Off Charing Cross Rd", "London", "UK", "London", "WC1B 3PA" },
                    { 5, null, null, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(142), 4, false, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Processing", 75.99m, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 5, 5), null, "Pending", null, "Liam Johnson", "555-0105", new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "222 Trail Road", "Near Blue Mountains Entry", "Sydney", "Australia", "NSW", "2000" },
                    { 6, null, "UPS", null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(145), 4, false, new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shipped", 69.99m, new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 5, 6), null, "Paid", null, "Sophie Martin", "555-0106", new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "TRK901234", null, null, "333 Radiant Road", "Galerie Vivienne", "Paris", "France", "Paris", "75002" },
                    { 7, null, "FedEx", null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(147), 4, false, new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Delivered", 15.99m, new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 5, 7), null, "Paid", null, "Arjun Patel", "555-0107", new DateTime(2025, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "TRK567890", null, null, "444 Playful Place", "Linking Road, Bandra", "Mumbai", "India", "MH", "400002" },
                    { 8, null, null, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(151), 5, false, new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Processing", 149.99m, new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 5, 8), null, "Pending", null, "Clara Fischer", "555-0108", new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "555 Tech Park", "Innovation Center, Floor 3", "Zurich", "Switzerland", "", "8002" },
                    { 9, null, null, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(154), 6, false, new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Processing", 149.99m, new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2025, 5, 8), null, "Pending", null, "Clara Fischer", "525-0108", new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "777 Skyline Boulevard", "Sky Tower, Apt 905", "Toronto", "Canada", "ON", "M5V 2T6" }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "ImageUrl", "IsDeleted", "ProductId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 14, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8741), "/images/products/camera_main.jpg", false, 6, null, null },
                    { 15, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8742), "/images/products/camera_top.jpg", false, 6, null, null },
                    { 16, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8743), "/images/products/camera_lens.jpg", false, 6, null, null },
                    { 17, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8743), "/images/products/tea_set_complete.jpg", false, 7, null, null },
                    { 18, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8744), "/images/products/tea_tin_open.jpg", false, 7, null, null },
                    { 24, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8751), "/images/products/skincare_set.jpg", false, 10, null, null },
                    { 25, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8752), "/images/products/skincare_serum.jpg", false, 10, null, null },
                    { 26, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8753), "/images/products/skincare_cream.jpg", false, 10, null, null },
                    { 27, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8754), "/images/products/robot_front.jpg", false, 11, null, null },
                    { 28, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8755), "/images/products/robot_side.jpg", false, 11, null, null },
                    { 31, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8757), "/images/products/smarthome_kit.jpg", false, 13, null, null },
                    { 32, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8758), "/images/products/smarthome_hub.jpg", false, 13, null, null },
                    { 33, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8759), "/images/products/smarthome_bulb.jpg", false, 13, null, null }
                });

            migrationBuilder.InsertData(
                table: "ProductSpecification",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsDeleted", "Key", "ProductId", "UpdatedBy", "UpdatedDate", "Value" },
                values: new object[,]
                {
                    { 23, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8847), false, "Megapixels", 6, null, null, "24.1 MP" },
                    { 24, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8848), false, "Sensor Type", 6, null, null, "CMOS" },
                    { 25, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8849), false, "Video Resolution", 6, null, null, "4K" },
                    { 26, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8850), false, "Lens", 6, null, null, "18-55mm" },
                    { 27, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8851), false, "ISO Range", 6, null, null, "100-25600" },
                    { 28, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8852), false, "Varieties", 7, null, null, "6" },
                    { 29, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8853), false, "Organic", 7, null, null, "Yes" },
                    { 30, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8854), false, "Weight", 7, null, null, "300g total (50g each)" },
                    { 31, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8892), false, "Packaging", 7, null, null, "Metal tins in gift box" },
                    { 40, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8903), false, "Pieces", 10, null, null, "5" },
                    { 41, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8904), false, "Skin Type", 10, null, null, "All" },
                    { 42, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8905), false, "Key Ingredients", 10, null, null, "Peptides, Hyaluronic Acid, Antioxidants" },
                    { 43, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8906), false, "SPF", 10, null, null, "30 (Day Cream)" },
                    { 44, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8907), false, "Age Range", 11, null, null, "6-12 years" },
                    { 45, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8908), false, "Programmable", 11, null, null, "Yes" },
                    { 46, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8909), false, "Battery Life", 11, null, null, "4 hours" },
                    { 47, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8910), false, "Connectivity", 11, null, null, "Bluetooth" },
                    { 48, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8911), false, "App Compatibility", 11, null, null, "iOS and Android" },
                    { 53, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8917), false, "Components", 13, null, null, "1 Hub, 2 Plugs, 2 Sensors, 3 Bulbs" },
                    { 54, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8918), false, "Connectivity", 13, null, null, "Wi-Fi, Bluetooth" },
                    { 55, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8919), false, "Voice Assistant Compatibility", 13, null, null, "Alexa, Google Assistant, Siri" },
                    { 56, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8920), false, "App Control", 13, null, null, "Yes" }
                });

            migrationBuilder.InsertData(
                table: "ProductTag",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsDeleted", "ProductId", "TagName", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 19, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9003), false, 6, "Photography", null, null },
                    { 20, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9004), false, 6, "4K Video", null, null },
                    { 21, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9005), false, 6, "Featured", null, null },
                    { 22, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9006), false, 7, "Organic", null, null },
                    { 23, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9007), false, 7, "Gift Set", null, null },
                    { 24, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9008), false, 7, "New Arrival", null, null },
                    { 32, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9014), false, 10, "Anti-Aging", null, null },
                    { 33, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9120), false, 10, "Beauty", null, null },
                    { 34, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9122), false, 10, "New Arrival", null, null },
                    { 35, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9122), false, 10, "Trending", null, null },
                    { 36, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9123), false, 11, "Educational", null, null },
                    { 37, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9124), false, 11, "STEM", null, null },
                    { 38, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9127), false, 11, "Kids", null, null },
                    { 39, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9128), false, 11, "New Arrival", null, null },
                    { 43, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9131), false, 13, "Smart Home", null, null },
                    { 44, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9132), false, 13, "IoT", null, null },
                    { 45, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9133), false, 13, "New Arrival", null, null },
                    { 46, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9134), false, 13, "Trending", null, null }
                });

            migrationBuilder.InsertData(
                table: "ProductVariant",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DiscountPrice", "IsDeleted", "Price", "ProductId", "SKU", "StockQuantity", "UpdatedBy", "UpdatedDate", "VariantName" },
                values: new object[,]
                {
                    { 16, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9233), 75.989999999999995, false, 89.989999999999995, 10, "BPC-AAGE-010-NORM", 15, null, null, "For Normal Skin" },
                    { 17, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9234), 75.989999999999995, false, 89.989999999999995, 10, "BPC-AAGE-010-DRY", 10, null, null, "For Dry Skin" },
                    { 18, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9236), 79.989999999999995, false, 94.989999999999995, 10, "BPC-AAGE-010-SENS", 5, null, null, "For Sensitive Skin" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "AllowBackorder", "AverageRating", "Barcode", "BrandId", "CategoryId", "CreatedBy", "CreatedDate", "Description", "DiscountEndDate", "DiscountPrice", "DiscountStartDate", "HeightInCm", "IsActive", "IsDeleted", "IsDiscounted", "IsFeatured", "IsNewArrival", "IsTrending", "LengthInCm", "MetaDescription", "MetaTitle", "Price", "SKU", "ShortDescription", "SoldCount", "StockQuantity", "Title", "TotalReviews", "UpdatedBy", "UpdatedDate", "Views", "WeightInKg", "WidthInCm" },
                values: new object[,]
                {
                    { 1, false, 4.7000000000000002, "789012345678", 1, 10, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8554), "Immerse yourself in stunning visuals with this 55-inch 4K Ultra HD Smart TV. Featuring High Dynamic Range (HDR) for vibrant colors and deep contrast, built-in Wi-Fi, and access to all your favorite streaming apps. Enjoy a cinematic experience in the comfort of your living room.", new DateTime(2025, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 649.99000000000001, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 71.200000000000003, true, false, true, true, false, true, 8.3000000000000007, "Shop our 55-inch 4K UHD Smart TV with HDR technology for the ultimate home entertainment experience.", "55 inch 4K Smart TV | Tech Solutions", 699.99000000000001, "ELC-SMTV-001", "55-inch 4K Smart TV with HDR and built-in streaming apps", 120, 50, "Smart TV 55 inch 4K UHD with HDR", 85, null, null, 2500, 15.5, 123.5 },
                    { 2, true, 4.5, "456789012345", 2, 11, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8571), "Experience ultimate comfort with our premium 100% combed cotton men's t-shirt. Designed for a classic fit and exceptional softness, this navy blue tee is a versatile wardrobe staple perfect for everyday wear. Available in various sizes.", new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 16.989999999999998, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 70.0, true, false, true, false, false, false, 0.5, "Classic fit men's navy blue t-shirt made from 100% premium combed cotton for all-day comfort.", "Men's Premium Cotton T-Shirt | Fashion Forward", 20.0, "APP-MTSRT-002-NVY", "Premium soft cotton t-shirt for men in navy blue", 250, 100, "Premium Cotton T-Shirt - Mens (Navy Blue)", 120, null, null, 1800, 0.20000000000000001, 50.0 },
                    { 3, false, 4.7999999999999998, "123456789012", 3, 14, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8577), "Get your gardening tasks done with ease using our durable 3-piece garden tool set. Includes a sturdy trowel, hand fork, and cultivator, all featuring comfortable wooden handles for a secure grip. Perfect for both novice and experienced gardeners.", null, 40.0, null, 30.0, true, false, false, false, true, false, 5.0, "High-quality 3-piece garden tool set with comfortable wooden handles for all your gardening needs.", "Essential Garden Tool Set | Green Living", 40.0, "HGN-TLSET-003-WD", "3-piece garden tool set with wooden handles", 45, 30, "Essential Garden Tool Set (3-Piece with Wooden Handles)", 28, null, null, 950, 0.90000000000000002, 12.0 },
                    { 4, false, 4.5999999999999996, "567890123456", 1, 9, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8580), "Experience lightning-fast performance with our ultra-slim 15.6-inch laptop. Featuring a powerful processor, 512GB SSD storage, and 16GB RAM for seamless multitasking. The vibrant Full HD display and long-lasting battery make it perfect for work and entertainment on the go.", new DateTime(2025, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 899.99000000000001, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1.8, true, false, true, true, true, true, 24.199999999999999, "Powerful and portable 15.6-inch laptop with SSD storage for fast performance anywhere you go.", "Ultra-Slim 15.6\" Laptop | Tech Solutions", 999.99000000000001, "ELC-LPTOP-004", "15.6\" ultra-slim laptop with SSD and powerful performance", 85, 35, "Ultra-Slim Laptop 15.6\" with SSD", 62, null, null, 3200, 1.8, 35.600000000000001 },
                    { 5, false, 4.7999999999999998, "345678901234", 2, 12, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8585), "Add a touch of elegance to any outfit with our designer leather handbag. Crafted from premium genuine leather with a stylish gold-tone hardware and multiple interior compartments for organization. The adjustable shoulder strap and handle offer versatile carrying options.", new DateTime(2025, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 129.99000000000001, new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 25.0, true, false, true, true, false, true, 12.0, "Elegant black leather handbag with multiple compartments and versatile carrying options.", "Designer Black Leather Handbag | Fashion Forward", 149.99000000000001, "APP-HBAG-005-BLK", "Premium black leather handbag with gold accents", 68, 25, "Designer Leather Handbag - Women's (Black)", 45, null, null, 1950, 0.80000000000000004, 35.0 },
                    { 8, true, 4.5, "901234567890", 4, 15, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8597), "Journey back in time with this captivating historical fiction novel that uncovers the story of a lost dynasty. Set in the 16th century, the narrative weaves together adventure, romance, and political intrigue as a young scholar uncovers ancient secrets that could change the course of history.", new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 15.99, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2.5, true, false, true, false, true, true, 22.800000000000001, "Immerse yourself in a captivating tale of secrets, adventure, and intrigue set in the 16th century.", "The Forgotten Dynasty | Historical Fiction", 18.989999999999998, "BOK-HFIC-008", "Captivating historical fiction novel set in the 16th century", 72, 60, "Historical Fiction: 'The Forgotten Dynasty'", 48, null, null, 1250, 0.40000000000000002, 15.199999999999999 },
                    { 9, false, 4.7999999999999998, "012345678901", 5, 17, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8602), "Conquer any trail with confidence in our all-weather hiking boots. Featuring waterproof construction, superior grip rubber soles, and cushioned insoles for all-day comfort. The breathable membrane keeps feet dry while allowing moisture to escape, making these perfect for year-round outdoor adventures.", new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 109.95, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 15.0, true, false, true, false, false, true, 20.0, "Durable and waterproof hiking boots designed for maximum comfort on any terrain and in any weather.", "All-Weather Hiking Boots | Adventure Gear", 129.94999999999999, "SPT-HBOOT-009", "Waterproof and comfortable hiking boots for all terrains", 95, 45, "All-Weather Hiking Boots (Unisex)", 63, null, null, 2100, 1.2, 30.0 },
                    { 12, false, 4.5999999999999996, "123456789013", 1, 8, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8614), "Take your mobile videography to the next level with our 3-axis smartphone stabilizer gimbal. Featuring intelligent tracking, multiple shooting modes, and foldable design for easy portability. The rechargeable battery provides up to 12 hours of operation, perfect for content creators and travelers.", new DateTime(2025, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 69.989999999999995, new DateTime(2025, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 19.0, true, false, true, false, true, false, 5.0, "Professional 3-axis gimbal stabilizer for smooth, cinematic smartphone videos.", "Smartphone Stabilizer Gimbal | Tech Solutions", 85.0, "ELC-GIMB-012", "3-axis gimbal stabilizer for professional smartphone videos", 58, 40, "Smartphone Stabilizer Gimbal", 42, null, null, 1680, 0.40000000000000002, 12.0 }
                });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "Id", "CompanyId", "CompanyId1", "CreatedBy", "CreatedDate", "CustomerId", "Discount", "ExternalReference", "InvoiceNumber", "InvoiceType", "IsDeleted", "IssueDate", "LocationId", "Notes", "OrderId", "PONumber", "PaidAmount", "PaymentDue", "PaymentMethod", "PaymentTerms", "RecurringInvoiceId", "ShippingAmount", "Status", "Subtotal", "Tax", "TotalAmount", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, null, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(466), 1, 0m, "REF-001", "INV-2025-001", 0, false, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Thank you for your purchase!", 1, "PO-001", 721.99m, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card", "Net 30", null, 20.00m, 3, 649.99m, 52.00m, 721.99m, null, null },
                    { 2, 2, null, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(482), 2, 10.00m, "REF-002", "INV-2025-002", 0, false, new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Please pay by due date.", 2, "PO-002", 0m, new DateTime(2025, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bank Transfer", "Net 30", null, 15.00m, 1, 129.99m, 10.40m, 145.39m, null, null },
                    { 3, 1, null, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(559), 3, 0m, "REF-003", "INV-2025-003", 2, false, new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Proforma invoice for approval.", 3, "PO-003", 888.99m, new DateTime(2025, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card", "Net 30", null, 25.00m, 3, 799.99m, 64.00m, 888.99m, null, null },
                    { 4, 5, null, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(565), 4, 0m, "REF-004", "INV-2025-004", 0, false, new DateTime(2025, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Partial payment received.", 4, "PO-004", 50.00m, new DateTime(2025, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bank Transfer", "Net 30", null, 10.00m, 2, 109.95m, 8.80m, 128.75m, null, null },
                    { 5, 6, null, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(570), 5, 0m, "REF-005", "INV-2025-005", 1, false, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, "Recurring invoice for monthly subscription.", 5, "PO-005", 0m, new DateTime(2025, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Direct Debit", "Net 30", null, 0m, 0, 75.99m, 6.08m, 82.07m, null, null },
                    { 6, 7, null, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(574), 6, 0m, "REF-006", "INV-2025-006", 0, false, new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "Payment overdue, please settle ASAP.", 6, "PO-006", 0m, new DateTime(2025, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Credit Card", "Net 30", null, 8.00m, 4, 69.99m, 5.60m, 83.59m, null, null },
                    { 7, 4, null, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(578), 7, 0m, "REF-007", "INV-2025-007", 3, false, new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Credit note for returned item.", 7, "PO-007", -17.27m, new DateTime(2025, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Refund", "Immediate", null, 0m, 3, -15.99m, -1.28m, -17.27m, null, null },
                    { 8, 1, null, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(582), 8, 0m, "REF-008", "INV-2025-008", 0, false, new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, "Invoice for recent purchase.", 8, "PO-008", 0m, new DateTime(2025, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bank Transfer", "Net 30", null, 15.00m, 1, 149.99m, 12.00m, 176.99m, null, null }
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "Id", "Count", "CreatedBy", "CreatedDate", "IsDeleted", "OrderHeaderId", "Price", "ProductId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(402), false, 1, 649.99000000000001, 1, null, null },
                    { 2, 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(405), false, 2, 129.99000000000001, 5, null, null },
                    { 3, 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(407), false, 3, 799.99000000000001, 6, null, null },
                    { 4, 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(408), false, 4, 109.95, 9, null, null },
                    { 5, 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(409), false, 5, 75.989999999999995, 10, null, null },
                    { 6, 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(411), false, 6, 69.989999999999995, 11, null, null },
                    { 7, 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(412), false, 7, 15.99, 8, null, null },
                    { 8, 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(413), false, 8, 149.99000000000001, 13, null, null },
                    { 9, 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(414), false, 9, 149.99000000000001, 13, null, null },
                    { 10, 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(415), false, 9, 69.989999999999995, 12, null, null },
                    { 11, 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(416), false, 3, 899.99000000000001, 4, null, null }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "ImageUrl", "IsDeleted", "ProductId", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8663), "/images/products/smarttv_main.jpg", false, 1, null, null },
                    { 2, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8666), "/images/products/smarttv_side.jpg", false, 1, null, null },
                    { 3, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8667), "/images/products/smarttv_ports.jpg", false, 1, null, null },
                    { 4, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8668), "/images/products/tshirt_navy_front.jpg", false, 2, null, null },
                    { 5, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8731), "/images/products/tshirt_navy_back.jpg", false, 2, null, null },
                    { 6, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8733), "/images/products/garden_tool_set.jpg", false, 3, null, null },
                    { 7, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8734), "/images/products/gardentool_trowel.jpg", false, 3, null, null },
                    { 8, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8735), "/images/products/gardentool_fork.jpg", false, 3, null, null },
                    { 9, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8736), "/images/products/laptop_main.jpg", false, 4, null, null },
                    { 10, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8736), "/images/products/laptop_open.jpg", false, 4, null, null },
                    { 11, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8738), "/images/products/laptop_side.jpg", false, 4, null, null },
                    { 12, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8739), "/images/products/handbag_black_main.jpg", false, 5, null, null },
                    { 13, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8740), "/images/products/handbag_black_open.jpg", false, 5, null, null },
                    { 19, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8745), "/images/products/book_cover.jpg", false, 8, null, null },
                    { 20, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8746), "/images/products/book_back.jpg", false, 8, null, null },
                    { 21, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8747), "/images/products/hikingboots_pair.jpg", false, 9, null, null },
                    { 22, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8748), "/images/products/hikingboots_sole.jpg", false, 9, null, null },
                    { 23, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8750), "/images/products/hikingboots_side.jpg", false, 9, null, null },
                    { 29, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8755), "/images/products/gimbal_main.jpg", false, 12, null, null },
                    { 30, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8756), "/images/products/gimbal_folded.jpg", false, 12, null, null }
                });

            migrationBuilder.InsertData(
                table: "ProductSpecification",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsDeleted", "Key", "ProductId", "UpdatedBy", "UpdatedDate", "Value" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8822), false, "Screen Size", 1, null, null, "55 inches" },
                    { 2, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8824), false, "Resolution", 1, null, null, "4K Ultra HD (3840 x 2160)" },
                    { 3, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8825), false, "Display Technology", 1, null, null, "LED" },
                    { 4, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8826), false, "HDR", 1, null, null, "Yes" },
                    { 5, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8828), false, "Smart TV", 1, null, null, "Yes" },
                    { 6, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8829), false, "Connectivity", 1, null, null, "Wi-Fi, Bluetooth, HDMI, USB" },
                    { 7, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8830), false, "Material", 2, null, null, "100% Combed Cotton" },
                    { 8, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8831), false, "Color", 2, null, null, "Navy Blue" },
                    { 9, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8832), false, "Care Instructions", 2, null, null, "Machine wash cold, tumble dry low" },
                    { 10, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8833), false, "Material", 3, null, null, "Stainless Steel with Wooden Handles" },
                    { 11, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8834), false, "Pieces", 3, null, null, "3" },
                    { 12, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8835), false, "Tool Length", 3, null, null, "30 cm" },
                    { 13, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8836), false, "Processor", 4, null, null, "Intel Core i7" },
                    { 14, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8837), false, "RAM", 4, null, null, "16 GB" },
                    { 15, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8838), false, "Storage", 4, null, null, "512 GB SSD" },
                    { 16, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8840), false, "Display", 4, null, null, "15.6-inch Full HD (1920 x 1080)" },
                    { 17, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8841), false, "Battery Life", 4, null, null, "Up to 10 hours" },
                    { 18, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8842), false, "Operating System", 4, null, null, "Windows 11" },
                    { 19, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8843), false, "Material", 5, null, null, "Genuine Leather" },
                    { 20, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8844), false, "Color", 5, null, null, "Black" },
                    { 21, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8845), false, "Dimensions", 5, null, null, "35 x 25 x 12 cm" },
                    { 22, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8846), false, "Hardware", 5, null, null, "Gold-tone" },
                    { 32, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8893), false, "Format", 8, null, null, "Hardcover" },
                    { 33, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8894), false, "Pages", 8, null, null, "384" },
                    { 34, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8895), false, "Genre", 8, null, null, "Historical Fiction" },
                    { 35, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8897), false, "Language", 8, null, null, "English" },
                    { 36, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8898), false, "Material", 9, null, null, "Waterproof Leather and Mesh" },
                    { 37, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8899), false, "Sole", 9, null, null, "Rubber with Multi-directional Traction" },
                    { 38, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8900), false, "Closure", 9, null, null, "Lace-up" },
                    { 39, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8901), false, "Gender", 9, null, null, "Unisex" },
                    { 49, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8913), false, "Axes", 12, null, null, "3-axis" },
                    { 50, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8914), false, "Battery Life", 12, null, null, "12 hours" },
                    { 51, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8915), false, "Compatibility", 12, null, null, "Most smartphones up to 6.7 inches" },
                    { 52, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8916), false, "Weight", 12, null, null, "400g" }
                });

            migrationBuilder.InsertData(
                table: "ProductTag",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "IsDeleted", "ProductId", "TagName", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8986), false, 1, "4K", null, null },
                    { 2, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8988), false, 1, "Smart TV", null, null },
                    { 3, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8989), false, 1, "HDR", null, null },
                    { 4, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8990), false, 1, "Home Entertainment", null, null },
                    { 5, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8991), false, 2, "Men's Fashion", null, null },
                    { 6, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8991), false, 2, "Casual Wear", null, null },
                    { 7, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8992), false, 2, "Cotton", null, null },
                    { 8, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8993), false, 3, "Gardening", null, null },
                    { 9, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8995), false, 3, "Tools", null, null },
                    { 10, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8996), false, 3, "New Arrival", null, null },
                    { 11, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8997), false, 4, "Computing", null, null },
                    { 12, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8997), false, 4, "SSD", null, null },
                    { 13, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8998), false, 4, "Lightweight", null, null },
                    { 14, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(8999), false, 4, "New Arrival", null, null },
                    { 15, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9000), false, 5, "Women's Fashion", null, null },
                    { 16, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9001), false, 5, "Leather", null, null },
                    { 17, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9002), false, 5, "Designer", null, null },
                    { 18, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9002), false, 5, "Trending", null, null },
                    { 25, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9009), false, 8, "Historical Fiction", null, null },
                    { 26, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9009), false, 8, "Bestseller", null, null },
                    { 27, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9010), false, 8, "New Arrival", null, null },
                    { 28, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9011), false, 9, "Outdoor", null, null },
                    { 29, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9012), false, 9, "Waterproof", null, null },
                    { 30, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9013), false, 9, "Unisex", null, null },
                    { 31, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9014), false, 9, "Trending", null, null },
                    { 40, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9129), false, 12, "Photography", null, null },
                    { 41, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9130), false, 12, "Accessories", null, null },
                    { 42, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9130), false, 12, "New Arrival", null, null }
                });

            migrationBuilder.InsertData(
                table: "ProductVariant",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "DiscountPrice", "IsDeleted", "Price", "ProductId", "SKU", "StockQuantity", "UpdatedBy", "UpdatedDate", "VariantName" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9204), 16.989999999999998, false, 20.0, 2, "APP-MTSRT-002-NVY-S", 25, null, null, "Size - S" },
                    { 2, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9210), 16.989999999999998, false, 20.0, 2, "APP-MTSRT-002-NVY-M", 30, null, null, "Size - M" },
                    { 3, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9212), 16.989999999999998, false, 20.0, 2, "APP-MTSRT-002-NVY-L", 25, null, null, "Size - L" },
                    { 4, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9214), 18.989999999999998, false, 22.0, 2, "APP-MTSRT-002-NVY-XL", 20, null, null, "Size - XL" },
                    { 5, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9215), 129.99000000000001, false, 149.99000000000001, 5, "APP-HBAG-005-BLK", 15, null, null, "Color - Black" },
                    { 6, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9217), 129.99000000000001, false, 149.99000000000001, 5, "APP-HBAG-005-BRN", 10, null, null, "Color - Brown" },
                    { 7, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9218), 109.95, false, 129.94999999999999, 9, "SPT-HBOOT-009-07", 8, null, null, "Size - US 7 / EU 38" },
                    { 8, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9220), 109.95, false, 129.94999999999999, 9, "SPT-HBOOT-009-08", 10, null, null, "Size - US 8 / EU 39" },
                    { 9, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9222), 109.95, false, 129.94999999999999, 9, "SPT-HBOOT-009-09", 12, null, null, "Size - US 9 / EU 40" },
                    { 10, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9223), 109.95, false, 129.94999999999999, 9, "SPT-HBOOT-009-10", 10, null, null, "Size - US 10 / EU 41" },
                    { 11, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9225), 109.95, false, 129.94999999999999, 9, "SPT-HBOOT-009-11", 5, null, null, "Size - US 11 / EU 42" },
                    { 12, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9227), 649.99000000000001, false, 699.99000000000001, 1, "ELC-SMTV-001-55", 30, null, null, "Size - 55 inch" },
                    { 13, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9228), 849.99000000000001, false, 899.99000000000001, 1, "ELC-SMTV-001-65", 20, null, null, "Size - 65 inch" },
                    { 14, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9230), 899.99000000000001, false, 999.99000000000001, 4, "ELC-LPTOP-004-16-512", 20, null, null, "RAM - 16GB / Storage - 512GB" },
                    { 15, null, new DateTime(2025, 4, 26, 10, 48, 17, 186, DateTimeKind.Utc).AddTicks(9231), 1199.99, false, 1299.99, 4, "ELC-LPTOP-004-32-1TB", 15, null, null, "RAM - 32GB / Storage - 1TB" }
                });

            migrationBuilder.InsertData(
                table: "InvoiceAttachments",
                columns: new[] { "Id", "AttachmentContent", "AttachmentName", "CreatedBy", "CreatedDate", "InvoiceId", "IsDeleted", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "/files/invoices/INV-2025-001.pdf", "Invoice_INV-2025-001.pdf", null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(684), 1, false, null, null },
                    { 2, "/files/invoices/INV-2025-002.pdf", "Invoice_INV-2025-002.pdf", null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(686), 2, false, null, null },
                    { 3, "/files/invoices/INV-2025-003.pdf", "Invoice_INV-2025-003.pdf", null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(687), 3, false, null, null },
                    { 4, "/files/invoices/INV-2025-004.pdf", "Invoice_INV-2025-004.pdf", null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(689), 4, false, null, null },
                    { 5, "/files/invoices/INV-2025-005.pdf", "Invoice_INV-2025-005.pdf", null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(690), 5, false, null, null },
                    { 6, "/files/invoices/INV-2025-006.pdf", "Invoice_INV-2025-006.pdf", null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(691), 6, false, null, null },
                    { 7, "/files/invoices/INV-2025-007.pdf", "CreditNote_INV-2025-007.pdf", null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(692), 7, false, null, null },
                    { 8, "/files/invoices/INV-2025-008.pdf", "Invoice_INV-2025-008.pdf", null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(693), 8, false, null, null }
                });

            migrationBuilder.InsertData(
                table: "InvoiceItems",
                columns: new[] { "Id", "Amount", "CreatedBy", "CreatedDate", "Description", "InvoiceId", "IsDeleted", "Price", "Service", "Unit", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 649.99m, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(626), "55-inch 4K Smart TV with HDR", 1, false, 649.99m, "Smart TV 55 inch", "Unit", null, null },
                    { 2, 129.99m, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(631), "Premium black leather handbag", 2, false, 129.99m, "Leather Handbag", "Unit", null, null },
                    { 3, 799.99m, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(633), "24.1MP DSLR camera with 18-55mm lens", 3, false, 799.99m, "DSLR Camera", "Unit", null, null },
                    { 4, 109.95m, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(634), "Waterproof hiking boots size US 9", 4, false, 109.95m, "Hiking Boots", "Unit", null, null },
                    { 5, 75.99m, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(636), "Anti-aging skincare collection for normal skin", 5, false, 75.99m, "Skincare Set", "Unit", null, null },
                    { 6, 69.99m, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(638), "Interactive learning robot for kids", 6, false, 69.99m, "Learning Robot", "Unit", null, null },
                    { 7, -15.99m, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(640), "Credit for returned historical fiction book", 7, false, -15.99m, "Book Return", "Unit", null, null },
                    { 8, 149.99m, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(642), "Smart home starter kit with hub and bulbs", 8, false, 149.99m, "Smart Home Kit", "Unit", null, null },
                    { 9, 49.99m, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(643), "2-year extended warranty for Smart TV", 1, false, 49.99m, "Extended Warranty", "Unit", null, null },
                    { 10, 29.99m, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(645), "Tripod accessory for DSLR camera", 3, false, 29.99m, "Camera Tripod", "Unit", null, null }
                });

            migrationBuilder.InsertData(
                table: "TaxDetails",
                columns: new[] { "Id", "Amount", "CreatedBy", "CreatedDate", "InvoiceId", "IsDeleted", "Rate", "TaxType", "UpdatedBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 52.00m, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(729), 1, false, 8.00m, "VAT", null, null },
                    { 2, 10.40m, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(732), 2, false, 8.00m, "GST", null, null },
                    { 3, 64.00m, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(733), 3, false, 8.00m, "Consumption Tax", null, null },
                    { 4, 8.80m, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(735), 4, false, 8.00m, "GST", null, null },
                    { 5, 6.08m, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(736), 5, false, 8.00m, "VAT", null, null },
                    { 6, 5.60m, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(738), 6, false, 8.00m, "GST", null, null },
                    { 7, -1.28m, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(739), 7, false, 8.00m, "VAT", null, null },
                    { 8, 12.00m, null, new DateTime(2025, 4, 26, 10, 48, 17, 187, DateTimeKind.Utc).AddTicks(740), 8, false, 8.00m, "VAT", null, null }
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
                name: "IX_Currencies_Code",
                table: "Currencies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CompanyId",
                table: "Customers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceAttachments_InvoiceId",
                table: "InvoiceAttachments",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceId",
                table: "InvoiceItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CompanyId",
                table: "Invoices",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CompanyId1",
                table: "Invoices",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CustomerId",
                table: "Invoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceNumber",
                table: "Invoices",
                column: "InvoiceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_LocationId",
                table: "Invoices",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_OrderId",
                table: "Invoices",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CompanyId",
                table: "Locations",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CurrencyId",
                table: "Locations",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_TimezoneId",
                table: "Locations",
                column: "TimezoneId");

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
                name: "IX_OrderHeaders_CustomerId",
                table: "OrderHeaders",
                column: "CustomerId");

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
                name: "IX_TaxDetails_InvoiceId",
                table: "TaxDetails",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Timezones_Name",
                table: "Timezones",
                column: "Name",
                unique: true);

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
                name: "InvoiceAttachments");

            migrationBuilder.DropTable(
                name: "InvoiceItems");

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
                name: "TaxDetails");

            migrationBuilder.DropTable(
                name: "WishlistItems");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "OrderHeaders");

            migrationBuilder.DropTable(
                name: "Brand");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Timezones");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
