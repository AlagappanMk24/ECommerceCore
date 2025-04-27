using ECommerceCore.Application.Common.QueryParameters;
using ECommerceCore.Application.Common.Results;
using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contract.ViewModels;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Infrastructure.Services
{
    public class ProductService(IUnitOfWork unitOfWork) : IProductService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _unitOfWork.Products.GetAllAsync(includeProperties: "Category,ProductImages");
        }
        public async Task<PaginatedResult<ProductDto>> GetProductsPaginatedAsync(ProductQueryParameters parameters)
        {
            try
            {
                // Base query
                var query = _unitOfWork.Products.Query()
                    .Include(p => p.Category)
                    .Include(p => p.ProductImages)
                    .AsQueryable();
                // Apply enhanced search filters
                if (!string.IsNullOrEmpty(parameters.SearchTerm))
                {
                    string searchTerm = parameters.SearchTerm.ToLower().Trim();
                    query = query.Where(p =>
                        p.Title.ToLower().Contains(searchTerm) ||
                        p.Description.ToLower().Contains(searchTerm) ||
                        p.ShortDescription.ToLower().Contains(searchTerm) ||
                        p.SKU.ToLower().Contains(searchTerm) ||
                        p.Barcode.ToLower().Contains(searchTerm) ||
                        p.MetaTitle.ToLower().Contains(searchTerm) ||
                        p.MetaDescription.ToLower().Contains(searchTerm) ||
                        p.Brand.Name.ToLower().Contains(searchTerm) ||
                        p.Category.Name.ToLower().Contains(searchTerm) ||
                        (p.Tags != null && p.Tags.Any(t => t.TagName.ToLower().Contains(searchTerm)))
                    );
                }

                if (parameters.CategoryId.HasValue)
                {
                    query = query.Where(p => p.CategoryId == parameters.CategoryId.Value);
                }

                // Apply stock status filter 
                if (!string.IsNullOrEmpty(parameters.StockStatus))
                {
                    query = parameters.StockStatus switch
                    {
                        "in-stock" => query.Where(p => p.StockQuantity > 10),
                        "low-stock" => query.Where(p => p.StockQuantity > 0 && p.StockQuantity <= 10),
                        "out-of-stock" => query.Where(p => p.StockQuantity <= 0),
                        _ => query
                    };
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(parameters.SortColumn))
                {
                    query = parameters.SortColumn.ToLower() switch
                    {
                        "title" => parameters.SortDirection == "asc" ?
                    query.OrderBy(p => p.Title) : query.OrderByDescending(p => p.Title),
                        "sku" => parameters.SortDirection == "asc" ?
                            query.OrderBy(p => p.SKU) : query.OrderByDescending(p => p.SKU),
                        "price" => parameters.SortDirection == "asc" ?
                            query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price),
                        "brand" => parameters.SortDirection == "asc" ?
                            query.OrderBy(p => p.Brand.Name) : query.OrderByDescending(p => p.Brand.Name),
                        "category" => parameters.SortDirection == "asc" ?
                            query.OrderBy(p => p.Category.Name) : query.OrderByDescending(p => p.Category.Name),
                        "rating" => parameters.SortDirection == "asc" ?
                            query.OrderBy(p => p.AverageRating) : query.OrderByDescending(p => p.AverageRating),
                        "soldcount" => parameters.SortDirection == "asc" ?
                            query.OrderBy(p => p.SoldCount) : query.OrderByDescending(p => p.SoldCount),
                        "views" => parameters.SortDirection == "asc" ?
                            query.OrderBy(p => p.Views) : query.OrderByDescending(p => p.Views),
                        "newarrival" => query.OrderByDescending(p => p.IsNewArrival),
                        "trending" => query.OrderByDescending(p => p.IsTrending),
                        "featured" => query.OrderByDescending(p => p.IsFeatured),
                        _ => query.OrderBy(p => p.Title)
                    };
                }

                // Get total count before pagination
                var totalCount = await query.CountAsync();

                // Apply pagination
                var items = await query
                    .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                    .Take(parameters.PageSize)
                    .Select(p => new ProductDto
                    {
                          Id = p.Id,
                          Title = p.Title,
                          ShortDescription = p.ShortDescription,
                          SKU = p.SKU,
                          Price = p.Price,
                          StockQuantity = p.StockQuantity,
                          CategoryName = p.Category.Name,
                          BrandName = p.Brand.Name,
                          IsFeatured = p.IsFeatured,
                          IsNewArrival = p.IsNewArrival,
                          IsTrending = p.IsTrending,
                          Views = p.Views,
                          SoldCount = p.SoldCount,
                          AverageRating = p.AverageRating,
                          ProductImages = p.ProductImages.Select(img => new ProductImage
                          {
                            Id = img.Id,
                            ImageUrl = img.ImageUrl,
                            ProductId = img.ProductId
                          }).ToList()
                    })
                    .ToListAsync();

                return new PaginatedResult<ProductDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = parameters.PageNumber,
                    PageSize = parameters.PageSize
                };
            }
            catch (Exception ex)
            {
                // Log error
                throw;
            }
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _unitOfWork.Products.GetAsync(p => p.Id == id, includeProperties: "Category,ProductImages");
        }
        public async Task<string> UpsertProductAsync(ProductVM productVM, List<IFormFile> files, string webRootPath)
        {
            if (productVM.Product.Id == 0)
            {
                await _unitOfWork.Products.AddAsync(productVM.Product);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                _unitOfWork.Products.Update(productVM.Product);
                await _unitOfWork.SaveAsync();
            }

            if (files != null)
            {
                string productPath = $@"images\products\product-{productVM.Product.Id}";
                foreach (var file in files)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string finalPath = Path.Combine(webRootPath, productPath);

                    if (!Directory.Exists(finalPath)) Directory.CreateDirectory(finalPath);

                    using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ProductImages ??= new List<ProductImage>();
                    productVM.Product.ProductImages.Add(new ProductImage
                    {
                        ImageUrl = $@"\{productPath}\{fileName}",
                        ProductId = productVM.Product.Id
                    });
                }
                _unitOfWork.Products.Update(productVM.Product);
                await _unitOfWork.SaveAsync();
            }
            return productVM.Product.Id == 0 ? "Product created successfully" : "Product updated successfully";
        }
        public async Task<string> DeleteProductImageAsync(int imageId, string webRootPath)
        {
            var imageToBeDeleted = await _unitOfWork.ProductImages.GetAsync(u => u.Id == imageId);
            if (imageToBeDeleted == null) return "Image not found";

            var imagePath = Path.Combine(webRootPath, imageToBeDeleted.ImageUrl.TrimStart('\\'));
            if (File.Exists(imagePath)) File.Delete(imagePath);

            await _unitOfWork.ProductImages.RemoveAsync(imageToBeDeleted);
            await _unitOfWork.SaveAsync();
            return "Image deleted successfully";
        }
        public async Task<object> DeleteProductAsync(int id, string webRootPath)
        {
            var productToBeDeleted = await _unitOfWork.Products.GetAsync(u => u.Id == id);
            if (productToBeDeleted == null) return new { success = false, message = "Error while deleting" };

            string productPath = $@"images\products\product-{id}";
            string finalPath = Path.Combine(webRootPath, productPath);
            if (Directory.Exists(finalPath))
            {
                foreach (var filePath in Directory.GetFiles(finalPath))
                {
                    File.Delete(filePath);
                }
            }

            await _unitOfWork.Products.RemoveAsync(productToBeDeleted);
            await _unitOfWork.SaveAsync();
            return new { success = true, message = "Product deleted successfully" };
        }
    }
}

///<summary>
/// The `ProductService` class handles operations related to managing products.
/// This service interacts with the repository layer through the `IUnitOfWork` interface to perform CRUD operations for products and their associated images.
/// 
/// Key Functions:
/// 1. `GetAllProductsAsync` - Retrieves all products, including their associated categories.
/// 2. `GetProductByIdAsync` - Retrieves a product by its ID, including its associated images.
/// 3. `UpsertProductAsync` - Creates or updates a product, saving product images to a specified directory if provided.
/// 4. `DeleteProductImageAsync` - Deletes a specific product image from both the database and the file system.
/// 5. `DeleteProductAsync` - Deletes a product and all its associated files from the database and the file system.
/// 
/// Dependencies:
/// - `IUnitOfWork`: Manages repositories for accessing the database.
/// - `ProductVM`: ViewModel containing product details.
/// - `IFormFile`: Represents uploaded files.
/// - `webRootPath`: Base path for storing images.
/// 
/// Handles scenarios like adding new products, updating existing ones, managing product images, and deleting products/images.
///</summary>
