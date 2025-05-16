using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contracts.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Operations.Controllers
{
    [Area("Operations")]
    [Authorize(Roles = "Manager,Employee,DeliveryAgent,CustomerSupport,Admin,AdminSuper")]
    [Route("Operations/Product")]
    public class ProductController(IProductService productService, IUnitOfWork unitOfWork, ICategoryService categoryService, ILogger<ProductController> logger) : Controller
    {
        private readonly IProductService _productService = productService;
        private readonly ICategoryService _categoryService = categoryService;
        private readonly ILogger<ProductController> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [HttpGet("")]
        [Authorize(Policy = "Product.View")]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Fetching data for the operations product index page.");
                var categories = await _categoryService.GetAllCategories();

                var viewModel = new ProductIndexVM
                {
                    QueryParameters = new ProductQueryParameters
                    {
                        PageNumber = 1,
                        PageSize = 10,
                        SortColumn = "title",
                        SortDirection = "asc"
                    },
                    Categories = categories
                };

                _logger.LogInformation("Successfully retrieved data for the operations product index page.");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the operations product index page.");
                TempData["Error"] = "Unable to load products.";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost("get-products")]
        [Authorize(Policy = "Product.View")]
        public async Task<IActionResult> GetProducts([FromBody] ProductQueryParameters queryParams)
        {
            try
            {
                var result = await _productService.GetProductsPaginatedAsync(queryParams);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products for operations");
                return StatusCode(500, new { error = "Error fetching products" });
            }
        }

        [HttpGet("details/{id}")]
        [Authorize(Policy = "Product.View")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null) return NotFound();
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching product details for ID: {id} in Operations Area.");
                return StatusCode(500, new { error = "Error fetching product details" });
            }
        }

        [HttpPost("approve/{id}")]
        [Authorize(Roles = "Manager,Admin,AdminSuper")]
        [Authorize(Policy = "Product.Edit")]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                _logger.LogInformation($"Approving product with ID: {id} in Operations Area.");
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null) return NotFound();

                product.IsActive = true; // Approve the product
                _unitOfWork.Products.Update(product);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation($"Successfully approved product with ID: {id}.");
                TempData["success"] = "Product approved successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error approving product with ID: {id} in Operations Area.");
                TempData["Error"] = "Unable to approve the product.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("reject/{id}")]
        [Authorize(Roles = "Manager,Admin,AdminSuper")]
        [Authorize(Policy = "Product.Edit")]
        public async Task<IActionResult> Reject(int id)
        {
            try
            {
                _logger.LogInformation($"Rejecting product with ID: {id} in Operations Area.");
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null) return NotFound();

                product.IsActive = false; // Reject the product
                _unitOfWork.Products.Update(product);
                await _unitOfWork.SaveAsync();

                _logger.LogInformation($"Successfully rejected product with ID: {id}.");
                TempData["success"] = "Product rejected successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error rejecting product with ID: {id} in Operations Area.");
                TempData["Error"] = "Unable to reject the product.";
                return RedirectToAction("Index");
            }
        }
    }
}
