using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Application.Contract.Service
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category>? GetCategoryById(int id);
        Task<bool> CreateCategory(Category category);
        Task <bool> UpdateCategory(Category category);
        Task<bool> DeleteCategory(int id);
    }
}
