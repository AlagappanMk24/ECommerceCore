using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contract.ViewModels;
using ECommerceCore.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace ECommerceCore.Infrastructure.Services
{
    public class ProductService(IUnitOfWork unitOfWork) : IProductService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _unitOfWork.Product.GetAllAsync(includeProperties: "Category");
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _unitOfWork.Product.GetAsync(p => p.Id == id, includeProperties: "ProductImages");
        }
        public async Task<string> UpsertProductAsync(ProductVM productVM, List<IFormFile> files, string webRootPath)
        {
            if (productVM.Product.Id == 0)
            {
                await _unitOfWork.Product.AddAsync(productVM.Product);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                _unitOfWork.Product.Update(productVM.Product);
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
                _unitOfWork.Product.Update(productVM.Product);
                await _unitOfWork.SaveAsync();
            }
            return productVM.Product.Id == 0 ? "Product created successfully" : "Product updated successfully";
        }
        public async Task<string> DeleteProductImageAsync(int imageId, string webRootPath)
        {
            var imageToBeDeleted = await _unitOfWork.ProductImage.GetAsync(u => u.Id == imageId);
            if (imageToBeDeleted == null) return "Image not found";

            var imagePath = Path.Combine(webRootPath, imageToBeDeleted.ImageUrl.TrimStart('\\'));
            if (File.Exists(imagePath)) File.Delete(imagePath);

            await _unitOfWork.ProductImage.RemoveAsync(imageToBeDeleted);
            await _unitOfWork.SaveAsync();
            return "Image deleted successfully";
        }
        public async Task<object> DeleteProductAsync(int id, string webRootPath)
        {
            var productToBeDeleted = await _unitOfWork.Product.GetAsync(u => u.Id == id);
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

            await _unitOfWork.Product.RemoveAsync(productToBeDeleted);
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
