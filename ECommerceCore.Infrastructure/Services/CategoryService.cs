using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Infrastructure.Services
{
    public class CategoryService(IUnitOfWork unitOfWork) : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Retrieves all categories from the database.
        /// </summary>
        /// <returns>An enumerable list of all categories.</returns>
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _unitOfWork.Categories.GetAllAsync();
        }

        /// <summary>
        /// Retrieves a category by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve.</param>
        /// <returns>The category with the specified ID, or null if not found.</returns>
        public async Task<Category>? GetCategoryById(int id)
        {
            return await _unitOfWork.Categories.GetAsync(c => c.Id == id);
        }

        /// <summary>
        /// Creates a new category in the database.
        /// </summary>
        /// <param name="category">The category object to create.</param>
        /// <exception cref="ArgumentNullException">Thrown when the category object is null.</exception>
        /// <returns>A boolean value indicating whether the creation was successful.</returns>
        public async Task<bool> CreateCategory(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveAsync();
            return true;
        }

        /// <summary>
        /// Updates an existing category in the database.
        /// </summary>
        /// <param name="category">The category object to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the category object is null.</exception>
        /// <returns>A boolean value indicating whether the update was successful.</returns>
        public async Task<bool> UpdateCategory(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveAsync();
            return true;
        }

        /// <summary>
        /// Deletes a category from the database by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>A boolean value indicating whether the deletion was successful. Returns false if the category was not found.</returns>
        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == id);
            if (category == null)
                return false;

            await _unitOfWork.Categories.RemoveAsync(category);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}