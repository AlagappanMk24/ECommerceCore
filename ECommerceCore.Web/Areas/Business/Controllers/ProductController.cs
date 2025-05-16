using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contracts.Services;
using ECommerceCore.Application.Contracts.ViewModels.Products;
using ECommerceCore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceCore.Web.Areas.Business.Controllers
{
    [Area("Business")]
    [Authorize(Roles = "Vendor,Company,Admin,AdminSuper")]
    [Route("Business/Product")]
    public class ProductController(IWebHostEnvironment webHostEnvironment,IProductService productService,ICategoryService categoryService,IBrandService brandService,ILogger<ProductController> logger) : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly IProductService _productService = productService;
        private readonly ICategoryService _categoryService = categoryService;
        private readonly IBrandService _brandService = brandService;
        private readonly ILogger<ProductController> _logger = logger;

        [HttpGet("")]
        [Authorize(Policy = "Product.View")]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Fetching data for the business product index page.");
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

                _logger.LogInformation("Successfully retrieved data for the business product index page.");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the business product index page.");
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
                var result = await _productService.GetProductsPaginatedForVendorAsync(queryParams, User.Identity.Name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products for business");
                return StatusCode(500, new { error = "Error fetching products" });
            }
        }

        [HttpGet("upsert/{id}")]
        [Authorize(Policy = "Product.Create")]
        public async Task<IActionResult> Upsert(int? id)
        {
            try
            {
                _logger.LogInformation(id == null
                    ? "Loading the Upsert page for a new product in Business Area."
                    : $"Loading the Upsert page for product ID: {id} in Business Area.");

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
                    //Product = id == null ? new Product() : await _productService.GetProductByIdForVendorAsync(id.Value, User.Identity.Name)
                };

                if (id != null && productVM.Product == null)
                {
                    _logger.LogWarning($"Product ID: {id} not found or not authorized for user: {User.Identity.Name}.");
                    TempData["Error"] = "Product not found or you are not authorized to edit it.";
                    return RedirectToAction("Index");
                }

                _logger.LogInformation("Successfully loaded the Upsert page in Business Area.");
                return View(productVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the Upsert page in Business Area.");
                TempData["Error"] = "Unable to load product form.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("upsert")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Product.Create")]
        public async Task<IActionResult> Upsert(ProductVM productVM, List<IFormFile> files)
        {
            try
            {
                _logger.LogInformation(productVM.Product.Id == 0
                    ? "Creating a new product in Business Area."
                    : $"Updating the product with ID: {productVM.Product.Id} in Business Area.");

                var result = await _productService.UpsertProductAsync(productVM, files, _webHostEnvironment.WebRootPath, User.Identity.Name);
                _logger.LogInformation("Successfully saved product: {Result}", result);

                TempData["success"] = result;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving the product in Business Area.");
                TempData["Error"] = "Unable to save the product.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet("details/{id}")]
        [Authorize(Policy = "Product.View")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdForVendorAsync(id, User.Identity.Name);
                if (product == null)
                {
                    _logger.LogWarning($"Product ID: {id} not found or not authorized for user: {User.Identity.Name}.");
                    return NotFound();
                }
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching product details for ID: {id} in Business Area.");
                return StatusCode(500, new { error = "Error fetching product details" });
            }
        }

        [HttpGet("delete-image/{imageId}")]
        [Authorize(Policy = "Product.Edit")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            try
            {
                _logger.LogInformation($"Deleting image with ID: {imageId} in Business Area.");
                var result = await _productService.DeleteProductImageAsync(imageId, _webHostEnvironment.WebRootPath);
                _logger.LogInformation("Successfully deleted image: {Result}", result);

                TempData["success"] = result;
                return RedirectToAction(nameof(Upsert), new { id = imageId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting image with ID: {imageId} in Business Area.");
                TempData["Error"] = "Unable to delete the image.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet("get-all")]
        [Authorize(Policy = "Product.View")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all products for Business Area via API call.");
                var paginatedResult = await _productService.GetProductsPaginatedForVendorAsync(
                    new ProductQueryParameters { PageNumber = 1, PageSize = int.MaxValue },
                    User.Identity.Name);
                var objProductList = paginatedResult.Items;
                _logger.LogInformation("Successfully retrieved products for API call in Business Area.");

                return Json(new { data = objProductList });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all products via API call in Business Area.");
                return Json(new { success = false, message = "Unable to fetch products." });
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "Product.Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogWarning($"Initiating delete operation for product ID: {id} in Business Area.");
                //var product = await _productService.GetProductByIdForVendorAsync(id, User.Identity.Name);
                //if (product == null)
                //{
                //    _logger.LogWarning($"Product ID: {id} not found or not authorized for user: {User.Identity.Name}.");
                //    return Json(new { success = false, message = "Product not found or you are not authorized to delete it." });
                //}

                var result = await _productService.DeleteProductAsync(id, _webHostEnvironment.WebRootPath);
                _logger.LogInformation("Successfully deleted product: {Result}", result);

                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting product with ID: {id} in Business Area.");
                return Json(new { success = false, message = "Error while deleting product." });
            }
        }
    }
}
