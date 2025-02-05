using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contract.ViewModels;
using ECommerceCore.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppConstants.Role_Admin)]
    public class CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger) : Controller
    {
        private readonly ICategoryService _categoryService = categoryService;
        private readonly ILogger<CategoryController> _logger = logger;

        /// <summary>
        /// Displays the list of all categories.
        /// </summary>
        /// <returns>An IActionResult containing the view with the category list.</returns>
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Fetching all categories.");
                var categories =await _categoryService.GetAllCategories();
                var categoryVM = new CategoryVM
                {
                    Categories = categories.ToList(), // Populate categories
                    Category = null // No need to pre-initialize; handled dynamically
                };
                return View(categoryVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching categories.");
                TempData["Error"] = "Unable to load categories. Please try again.";
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// Creates a new category based on the provided data.
        /// </summary>
        /// <param name="category">The category object containing the data to be created.</param>
        /// <returns>An IActionResult that redirects to the index view upon successful creation or returns the same view with an error message if unsuccessful.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            try
            {
                _logger.LogInformation("Creating a new category.");
                if (await _categoryService.CreateCategory(category))
                {
                    _logger.LogInformation("Category created successfully.");
                    TempData["Success"] = "Category added successfully";
                    return RedirectToAction("Index");
                }
                _logger.LogWarning("Failed to create category.");
                TempData["Error"] = "Unable to add category. Please try again.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a category.");
                TempData["Error"] = "An unexpected error occurred. Please try again later.";
            }
            return View(category);
        }

        /// <summary>
        /// Updates an existing category based on the provided data.
        /// </summary>
        /// <param name="category">The category object containing the updated data.</param>
        /// <returns>An IActionResult that redirects to the index view upon successful update or returns the same view with an error message if unsuccessful.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            try
            {
                _logger.LogInformation($"Updating category with ID: {category.Id}.");
                if (await _categoryService.UpdateCategory(category))
                {
                    _logger.LogInformation("Category updated successfully.");
                    TempData["Success"] = "Category updated successfully";
                    return RedirectToAction("Index");
                }
                _logger.LogWarning("Failed to update category.");
                TempData["Error"] = "Unable to update category. Please try again.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the category.");
                TempData["Error"] = "An error occurred. Please try again.";
            }

            return View(category);
        }

        /// <summary>
        /// Displays the view for confirming the deletion of a category.
        /// </summary>
        /// <param name="id">The identifier of the category to be deleted.</param>
        /// <returns>An IActionResult containing the view for confirming deletion or a NotFound result if the category does not exist.</returns>
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching category with ID: {id} for deletion.");
                var category = await _categoryService.GetCategoryById(id);

                if (category == null)
                {
                    _logger.LogWarning($"Category with ID: {id} not found.");
                    return NotFound();
                }
                return View(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the category for deletion.");
                TempData["Error"] = "Unable to load category. Please try again.";
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// Deletes a category based on the provided identifier.
        /// </summary>
        /// <param name="id">The identifier of the category to be deleted.</param>
        /// <returns>An IActionResult that redirects to the index view upon successful deletion or redirects to the index with an error message if unsuccessful.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting category with ID: {id}.");
                if (await _categoryService.DeleteCategory(id))
                {
                    _logger.LogInformation("Category deleted successfully.");
                    TempData["Success"] = "Category deleted successfully";
                    return RedirectToAction("Index");
                }
                _logger.LogWarning("Failed to delete category.");
                TempData["Error"] = "Unable to delete category. Please try again.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the category.");
                TempData["Error"] = "An error occurred. Please try again.";
            }

            return RedirectToAction("Index");
        }
    }
}
