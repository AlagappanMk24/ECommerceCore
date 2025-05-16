using ECommerceCore.Application.Common.Results;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Application.Contracts.ViewModels.Products;
using ECommerceCore.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace ECommerceCore.Application.Contract.Service
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<PaginatedResult<ProductDto>> GetProductsPaginatedAsync(ProductQueryParameters parameters);
        Task<PaginatedResult<ProductDto>> GetProductsPaginatedForVendorAsync(ProductQueryParameters parameters, string vendorId);
        Task<Product> GetProductByIdAsync(int id);
        Task<Product> GetProductByIdForVendorAsync(int id, string vendorId);
        Task<string> UpsertProductAsync(ProductVM productVM, List<IFormFile> files, string webRootPath, string vendorId);
        Task<string> DeleteProductImageAsync(int imageId, string webRootPath);
        Task<object> DeleteProductAsync(int id, string webRootPath);
    }
}
