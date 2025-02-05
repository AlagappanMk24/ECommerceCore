using ECommerceCore.Application.Contract.ViewModels;
using ECommerceCore.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace ECommerceCore.Application.Contract.Service
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<string> UpsertProductAsync(ProductVM productVM, List<IFormFile> files, string webRootPath);
        Task<string> DeleteProductImageAsync(int imageId, string webRootPath);
        Task<object> DeleteProductAsync(int id, string webRootPath);
    }
}
