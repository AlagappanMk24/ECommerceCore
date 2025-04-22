using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contract.ViewModels;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ECommerceCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    [Authorize(Roles = AppConstants.Role_Admin)]
    public class CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger) : Controller
    {
        private readonly ICategoryService _categoryService = categoryService;
        private readonly ILogger<CategoryController> _logger = logger;
        private const string LogPrefix = "Category Management";

        /// <summary>
        /// Displays the list of all categories.
        /// </summary>
        /// <returns>An IActionResult containing the view with the category list.</returns>
        // GET: Admin/Category
        [HttpGet("")]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["Operation"] = "Category List Retrieval",
                ["User"] = User.Identity?.Name ?? "Anonymous"
            }))
                try
                {
                    _logger.LogInformation("{LogPrefix}: Initiating retrieval of all categories", LogPrefix);
                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                    var categoriesQuery = await _categoryService.GetAllCategories();
                    int totalRecords = categoriesQuery.Count();

                    var categories = categoriesQuery
                       .OrderBy(c => c.DisplayOrder)
                       .Skip((pageNumber - 1) * pageSize)
                       .Take(pageSize)
                       .ToList();

                    stopwatch.Stop();
                    _logger.LogInformation("{LogPrefix}: Successfully retrieved {CategoryCount} categories in {ElapsedMilliseconds}ms", LogPrefix, categories.Count(), stopwatch.ElapsedMilliseconds);
                    var categoryVM = new CategoryVM
                    {
                        Categories = categories,
                        CurrentPage = pageNumber,
                        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                        PageSize = pageSize,
                        TotalRecords = totalRecords
                    };
                    return View(categoryVM);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{LogPrefix}: CRITICAL FAILURE in category retrieval. Error details: {ErrorMessage}", LogPrefix, ex.Message);
                    TempData["Error"] = "System encountered an error while loading categories";
                    return RedirectToAction("Error", "Home");
                }
        }

        // GET: Admin/Category/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            _logger.LogDebug("{LogPrefix}: Create category form requested", LogPrefix);
            return View(new CategoryVM { Category = new Category() });
        }

        /// <summary>
        /// Creates a new category based on the provided data.
        /// </summary>
        /// <param name="category">The category object containing the data to be created.</param>
        /// <returns>An IActionResult that redirects to the index view upon successful creation or returns the same view with an error message if unsuccessful.</returns>
        // POST: Admin/Category/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CategoryDto categoryDto)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["Operation"] = "Category Creation",
                ["User"] = User.Identity?.Name ?? "Anonymous",
                ["CategoryName"] = categoryDto.Name,
                ["DisplayOrder"] = categoryDto.DisplayOrder
            }))
                try
                {
                    _logger.LogInformation("{LogPrefix}: Attempting to create new category '{CategoryName}'", LogPrefix, categoryDto.Name);
                    var request = new CreateCategoryRequest
                    {
                        CategoryDto = categoryDto,
                        CurrentUser = User.Identity?.Name
                    };
                    var result = await _categoryService.CreateCategoryAsync(request);
                    if (result.IsSuccess)
                    {
                        _logger.LogInformation("{LogPrefix}: Successfully created category : {CategoryDetails}", LogPrefix, new { categoryDto.Name, categoryDto.DisplayOrder });
                        TempData["Success"] = $"Category '{categoryDto.Name}' created successfully";
                        return Json(new
                        {
                            success = result.IsSuccess,
                            message = result.IsSuccess
                               ? $"Category '{categoryDto.Name}' created successfully"
                               : result.ErrorMessage
                        });
                    }
                    _logger.LogError("{LogPrefix}: Failed to persist category '{CategoryName}' to database", LogPrefix, categoryDto.Name);
                    TempData["Error"] = $"Failed to create category '{categoryDto.Name}'";
                }
                catch (DbUpdateException dbEx)
                {
                    _logger.LogCritical(dbEx, "{LogPrefix}: DATABASE ERROR while creating category. SQL State: {SqlState}", LogPrefix, dbEx.InnerException is SqlException sqlEx ? sqlEx.State.ToString() : "N/A");
                    TempData["Error"] = "Database error occurred while creating category";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{LogPrefix}: SYSTEM ERROR during category creation. Error Code: {ErrorCode}", LogPrefix, "CAT_CREATE_500");
                    TempData["Error"] = "Unexpected system error occurred";
                }
            return View(new CategoryVM
            {
                Category = new Category
                {
                    Name = categoryDto.Name,
                    DisplayOrder = categoryDto.DisplayOrder
                }
            });
        }

        /// <summary>
        /// Retrieves a category by its ID and displays the edit form.
        /// </summary>
        /// <param name="id">The ID of the category to edit.</param>
        /// <returns>An IActionResult containing the edit view with the category data, or a NotFound result if the category is not found.</returns>
        // GET: Admin/Category/Edit/5
        [HttpGet("Edit/{id:int}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                _logger.LogWarning("{LogPrefix}: Invalid ID {CategoryId} requested for edit", LogPrefix, id);
                return NotFound();
            }
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["Operation"] = "Category Edit Retrieval",
                ["CategoryId"] = id.Value
            }))
                try
                {
                    _logger.LogDebug("{LogPrefix}: Retrieving category ID {CategoryId} for editing", LogPrefix, id.Value);
                    var category = await _categoryService.GetCategoryById(id.Value); // Assuming you have a method to get a category by ID

                    if (category == null)
                    {
                        _logger.LogWarning("{LogPrefix}: Requested category ID {CategoryId} not found", LogPrefix, id.Value);
                        return NotFound();
                    }
                    _logger.LogInformation("{LogPrefix}: Successfully retrieved category ID {CategoryId} for editing", LogPrefix, id.Value);
                    return Json(category); // Return category as JSON
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{LogPrefix}: Failed to retrieve category ID {CategoryId} for editing. Status: {Status}", LogPrefix, id.Value, "FAILED");
                    return Json(new
                    {
                        success = false,
                        error = "Failed to load category data"
                    });
                }
        }

        /// <summary>
        /// Updates an existing category based on the provided data.
        /// </summary>
        /// <param name="category">The category object containing the updated data.</param>
        /// <returns>An IActionResult that redirects to the index view upon successful update or returns the same view with an error message if unsuccessful.</returns>
        // POST: Admin/Category/Edit/5
        [HttpPost("Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["Operation"] = "Category Update",
                ["CategoryId"] = category.Id,
                ["OriginalValues"] = await GetOriginalCategoryValues(category.Id)
            }))
            {
                try
                {
                    _logger.LogInformation("{LogPrefix}: Attempting to update category ID {CategoryId}", LogPrefix, category.Id);

                    var result = await _categoryService.UpdateCategory(category);

                    if (result)
                    {
                        _logger.LogInformation("{LogPrefix}: Successfully updated category ID {CategoryId}. Changes: {Changes}",
                            LogPrefix, category.Id, GetChangeLog(category));

                        TempData["Success"] = $"Category '{category.Name}' updated successfully";
                        return RedirectToAction(nameof(Index));
                    }

                    _logger.LogError("{LogPrefix}: Failed to persist updates to category ID {CategoryId}", LogPrefix, category.Id);
                    TempData["Error"] = $"Failed to update category '{category.Name}'";
                }
                catch (DBConcurrencyException cex)
                {
                    _logger.LogError(cex, "{LogPrefix}: CONCURRENCY CONFLICT updating category ID {CategoryId}", LogPrefix, category.Id);
                    TempData["Error"] = "Category was modified by another user. Please refresh and try again.";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{LogPrefix}: SYSTEM ERROR updating category ID {CategoryId}. Error Code: {ErrorCode}",
                        LogPrefix, category.Id, "CAT_UPDATE_500");

                    TempData["Error"] = "Unexpected system error occurred";
                }

                return View(category);
            }
        }

        /// <summary>
        /// Displays the view for confirming the deletion of a category.
        /// </summary>
        /// <param name="id">The identifier of the category to be deleted.</param>
        /// <returns>An IActionResult containing the view for confirming deletion or a NotFound result if the category does not exist.</returns>
        // GET: Admin/Category/Delete/5
        [HttpGet("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["Operation"] = "Category Delete Confirmation",
                ["CategoryId"] = id
            }))
            {
                try
                {
                    _logger.LogDebug("{LogPrefix}: Retrieving category ID {CategoryId} for deletion confirmation", LogPrefix, id);

                    var category = await _categoryService.GetCategoryById(id);

                    if (category == null)
                    {
                        _logger.LogWarning("{LogPrefix}: Category ID {CategoryId} not found for deletion", LogPrefix, id);
                        return NotFound();
                    }

                    _logger.LogInformation("{LogPrefix}: Retrieved category ID {CategoryId} for deletion confirmation: {CategoryName}",
                        LogPrefix, id, category.Name);

                    return View(category);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{LogPrefix}: Failed to retrieve category ID {CategoryId} for deletion", LogPrefix, id);
                    TempData["Error"] = "Failed to load category for deletion";
                    return RedirectToAction(nameof(Index));
                }
            }
        }

        /// <summary>
        /// Deletes a category based on the provided identifier.
        /// </summary>
        /// <param name="id">The identifier of the category to be deleted.</param>
        /// <returns>An IActionResult that redirects to the index view upon successful deletion or redirects to the index with an error message if unsuccessful.</returns>
        // POST: Admin/Category/Delete/5
        [HttpPost("Delete/{id:int}")]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["Operation"] = "Category Deletion",
                ["CategoryId"] = id
            }))
            {
                try
                {
                    _logger.LogInformation("{LogPrefix}: Attempting to delete category ID {CategoryId}", LogPrefix, id);

                    var category = await _categoryService.GetCategoryById(id);
                    if (category == null)
                    {
                        _logger.LogWarning("{LogPrefix}: Category ID {CategoryId} not found during deletion attempt", LogPrefix, id);
                        TempData["Error"] = "Category not found";
                        return RedirectToAction(nameof(Index));
                    }

                    var result = await _categoryService.DeleteCategory(id);

                    if (result)
                    {
                        _logger.LogInformation("{LogPrefix}: Successfully deleted category ID {CategoryId}: {CategoryName}",
                            LogPrefix, id, category.Name);

                        TempData["Success"] = $"Category '{category.Name}' deleted successfully";
                        return RedirectToAction(nameof(Index));
                    }

                    _logger.LogError("{LogPrefix}: Failed to delete category ID {CategoryId}", LogPrefix, id);
                    TempData["Error"] = $"Failed to delete category '{category.Name}'";
                }
                catch (DbUpdateException dbEx) when (dbEx.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    _logger.LogError(dbEx, "{LogPrefix}: REFERENCE CONSTRAINT violation deleting category ID {CategoryId}", LogPrefix, id);
                    TempData["Error"] = "Cannot delete category as it's being referenced by other records";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{LogPrefix}: SYSTEM ERROR deleting category ID {CategoryId}. Error Code: {ErrorCode}",
                        LogPrefix, id, "CAT_DELETE_500");

                    TempData["Error"] = "Unexpected system error occurred";
                }
                return RedirectToAction(nameof(Index));
            }
        }

        // AJAX Endpoint: Admin/Category/GetCategory/5
        [HttpGet("GetCategory/{id:int}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetCategory(int id)
        {
            try
            {
                _logger.LogDebug("Fetching category {CategoryId} for AJAX request", id);
                var category = await _categoryService.GetCategoryById(id);

                if (category == null)
                {
                    _logger.LogWarning("Category {CategoryId} not found for AJAX request", id);
                    return NotFound(new { success = false, message = "Category not found" });
                }

                return Ok(new
                {
                    success = true,
                    data = new { category.Id, category.Name, category.DisplayOrder }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching category {CategoryId} for AJAX request", id);
                return StatusCode(500, new { success = false, message = "Error loading category data" });
            }
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories(int pageNumber = 1, int pageSize = 10)
        {
            var categoriesQuery = await _categoryService.GetAllCategories();
            int totalRecords = categoriesQuery.Count();

            var categories = categoriesQuery
               .OrderBy(c => c.DisplayOrder)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToList();

            var model = new CategoryVM
            {
                Categories = categories,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
            return PartialView("_CategoriesTablePartial", model);
        }

        #region Helper Methods
        private async Task<object> GetOriginalCategoryValues(int categoryId)
        {
            try
            {
                var original = await _categoryService.GetCategoryById(categoryId);
                return original != null
                    ? new { original.Name, original.DisplayOrder }
                    : null;
            }
            catch
            {
                return "Unable to retrieve original values";
            }
        }
        private async Task<string> GetChangeLog(Category updatedCategory)
        {
            try
            {
                var original = await _categoryService.GetCategoryById(updatedCategory.Id);
                if (original == null) return "Original record not found";

                var changes = new List<string>();
                if (original.Name != updatedCategory.Name)
                    changes.Add($"Name: '{original.Name}' → '{updatedCategory.Name}'");
                if (original.DisplayOrder != updatedCategory.DisplayOrder)
                    changes.Add($"Order: {original.DisplayOrder} → {updatedCategory.DisplayOrder}");

                return changes.Any() ? string.Join(", ", changes) : "No changes detected";
            }
            catch
            {
                return "Change detection failed";
            }
        }
        #endregion
    }
}
