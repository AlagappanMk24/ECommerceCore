using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contracts.Services;
using ECommerceCore.Application.Contracts.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Route("Customer/Product")]
    public class ProductController(IProductService productService,ICategoryService categoryService,IBrandService brandService,IUnitOfWork unitOfWork,ILogger<ProductController> logger) : Controller
    {
        private readonly IProductService _productService = productService;
        private readonly ICategoryService _categoryService = categoryService;
        private readonly IBrandService _brandService = brandService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<ProductController> _logger = logger;

        [HttpGet("")]
        [AllowAnonymous] // Public access for browsing
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Fetching data for the customer product index page.");
                var categories = await _categoryService.GetAllCategories();

                var viewModel = new ProductIndexVM
                {
                    QueryParameters = new ProductQueryParameters
                    {
                        PageNumber = 1,
                        PageSize = 12, // Different page size for customer-facing view
                        SortColumn = "title",
                        SortDirection = "asc"
                    },
                    Categories = categories
                };

                _logger.LogInformation("Successfully retrieved data for the customer product index page.");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the customer product index page.");
                TempData["Error"] = "Unable to load products.";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost("get-products")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts([FromBody] ProductQueryParameters queryParams)
        {
            try
            {
                var result = await _productService.GetProductsPaginatedAsync(queryParams);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products for customer");
                return StatusCode(500, new { error = "Error fetching products" });
            }
        }

        [HttpGet("details/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null) return NotFound();

                // Increment views
                product.Views++;
                _unitOfWork.Products.Update(product);
                await _unitOfWork.SaveAsync();

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching product details for ID: {id}");
                return StatusCode(500, new { error = "Error fetching product details" });
            }
        }
    }
}