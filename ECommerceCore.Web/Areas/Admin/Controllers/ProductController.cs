using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contracts.Services;
using ECommerceCore.Application.Contracts.ViewModels.Products;
using ECommerceCore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = AppConstants.Role_Admin)]
    [Authorize(Policy = "ProductAccess")]
    [Route("Admin/Product")]
    public class ProductController(IWebHostEnvironment webHostEnvironment, IProductService productService, ICategoryService categoryService, IBrandService brandService, ILogger<ProductController> logger) : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly IProductService _productService = productService;
        private readonly ICategoryService _categoryService = categoryService;
        private readonly IBrandService _brandService = brandService;
        private readonly ILogger<ProductController> _logger = logger;

        [HttpGet("")]
        [Authorize(Policy = AuthorizationConstants.Permissions.Product_View)]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Fetching data for the product index page.");
                var categories = await _categoryService.GetAllCategories();

                // Create default query parameters for initial state
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

                _logger.LogInformation("Successfully retrieved data for the product index page.");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the product index page.");
                TempData["Error"] = "Unable to load products.";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost("get-products")]
        [Authorize(Policy = AuthorizationConstants.Permissions.Product_View)]
        public async Task<IActionResult> GetProducts([FromBody] ProductQueryParameters queryParams)
        {
            try
            {
                var result = await _productService.GetProductsPaginatedAsync(queryParams);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products");
                return StatusCode(500, new { error = "Error fetching products" });
            }
        }

        [HttpGet("upsert/{id?}")]
        [Authorize(Policy = AuthorizationConstants.Permissions.Product_Manage)]
        public async Task<IActionResult> Upsert(int? id)
        {
            try
            {
                _logger.LogInformation(id == null
                    ? "Loading the Upsert page for a new product."
                    : $"Loading the Upsert page for product ID: {id}.");

                var categories = await _categoryService.GetAllCategories();
                var brands = await _brandService.GetAllBrands();
                var productVM = new ProductVM
                {
                    CategoryList = categories.Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }).ToList(),
                    BrandList = brands.Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                    Product = id == null ? new Product() : await _productService.GetProductByIdAsync(id.Value)
                };

                _logger.LogInformation("Successfully loaded the Upsert page.");
                return View(productVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the Upsert page.");
                TempData["Error"] = "Unable to load product form.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("upsert")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = AuthorizationConstants.Permissions.Product_Manage)]
        public async Task<IActionResult> Upsert(ProductVM productVM, List<IFormFile> files)
        {
            try
            {
                _logger.LogInformation(productVM.Product.Id == 0
                    ? "Creating a new product."
                    : $"Updating the product with ID: {productVM.Product.Id}.");

                var result = await _productService.UpsertProductAsync(productVM, files, _webHostEnvironment.WebRootPath, null);
                _logger.LogInformation("Successfully saved product: {Result}", result);

                TempData["success"] = result;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving the product.");
                TempData["Error"] = "Unable to save the product.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet("details/{id}")]
        [Authorize(Policy = AuthorizationConstants.Permissions.Product_View)]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var result = await _productService.GetProductByIdAsync(id);
                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products");
                return StatusCode(500, new { error = "Error fetching products" });
            }
        }

        [HttpGet("delete-image/{imageId}")]
        [Authorize(Policy = AuthorizationConstants.Permissions.Product_Edit)]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            try
            {
                _logger.LogInformation($"Deleting image with ID: {imageId}.");
                var result = await _productService.DeleteProductImageAsync(imageId, _webHostEnvironment.WebRootPath);
                _logger.LogInformation("Successfully deleted image: {Result}", result);

                TempData["success"] = result;
                return RedirectToAction(nameof(Upsert), new { id = imageId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting image with ID: {imageId}.");
                TempData["Error"] = "Unable to delete the image.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet("get-all")]
        [Authorize(Policy = AuthorizationConstants.Permissions.Product_View)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all products via API call.");
                var objProductList = await _productService.GetAllProductsAsync();
                _logger.LogInformation("Successfully retrieved products for API call.");

                return Json(new { data = objProductList });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all products via API call.");
                return Json(new { success = false, message = "Unable to fetch products." });
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy = AuthorizationConstants.Permissions.Product_Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogWarning($"Initiating delete operation for product ID: {id}.");
                var result = await _productService.DeleteProductAsync(id, _webHostEnvironment.WebRootPath);
                _logger.LogInformation("Successfully deleted product: {Result}", result);

                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting product with ID: {id}.");
                return Json(new { success = false, message = "Error while deleting product." });
            }
        }
    }
}