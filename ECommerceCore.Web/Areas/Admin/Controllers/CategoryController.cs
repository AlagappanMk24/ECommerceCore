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
    [Authorize(Roles = AppConstants.Role_Admin)]
    [Route("Admin/Category")]
    public class CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger) : Controller
    {
        private readonly ICategoryService _categoryService = categoryService;
        private readonly ILogger<CategoryController> _logger = logger;

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Fetching data for the category index page.");
                var categories = await _categoryService.GetAllCategories();

                // Create default query parameters for initial state
                var viewModel = new CategoryIndexVM
                {
                    QueryParameters = new CategoryQueryParameters
                    {
                        PageNumber = 1,
                        PageSize = 10,
                        SortColumn = "displayorder",
                        SortDirection = "asc"
                    },
                    ParentCategories = categories.Where(c => c.ParentCategoryId == null).ToList()
                };

                _logger.LogInformation("Successfully retrieved data for the category index page.");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the category index page.");
                TempData["Error"] = "Unable to load categories.";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost("get-categories")]
        public async Task<IActionResult> GetCategories([FromBody] CategoryQueryParameters queryParams)
        {
            try
            {
                var result = await _categoryService.GetCategoriesPaginatedAsync(queryParams);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories");
                return StatusCode(500, new { error = "Error fetching categories" });
            }
        }
    }
}
