using ECommerceCore.Application.Common.Results;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contract.Service
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category>? GetCategoryById(int id);
        Task<OperationResult<Category>> CreateCategoryAsync(CreateCategoryRequest request);
        Task <bool> UpdateCategory(Category category);
        Task<bool> DeleteCategory(int id);
    }
}
