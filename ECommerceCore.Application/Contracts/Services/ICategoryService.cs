using ECommerceCore.Application.Common.Results;
using ECommerceCore.Application.Contract.ViewModels;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contract.Service
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category>? GetCategoryById(int id);
        Task<PaginatedResult<CategoryDto>> GetCategoriesPaginatedAsync(CategoryQueryParameters parameters);
    }
}
