using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contract.ViewModels;
using ECommerceCore.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppConstants.Role_Admin)]
    public class ProductController(IWebHostEnvironment webHostEnvironment, IProductService productService, ICategoryService categoryService, ILogger<ProductController> logger) : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly IProductService _productService = productService;
        private readonly ICategoryService _categoryService = categoryService;
        private readonly ILogger<ProductController> _logger = logger;

        /// <summary>
        /// Retrieves and displays a list of all products for the index page.
        /// </summary>
        /// <returns>A view containing a list of products.</returns>
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Fetching all products for the index page.");
                var products = await _productService.GetAllProductsAsync();
                _logger.LogInformation("Successfully retrieved products for the index page.");
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the product index page.");
                TempData["Error"] = "Unable to load products.";
                return RedirectToAction("Error", "Home");
            }
        }

        /// <summary>
        /// Prepares the view for adding or updating a product based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the product to be updated; if null, prepares for creating a new product.</param>
        /// <returns>A view for creating or updating a product.</returns>
        public async Task<IActionResult> Upsert(int? id)
        {
            try
            {
                _logger.LogInformation(id == null
                    ? "Loading the Upsert page for a new product."
                    : $"Loading the Upsert page for product ID: {id}.");

                var categories = await _categoryService.GetAllCategories();
                var productVM = new ProductVM
                {
                    CategoryList = categories.Select(c => new SelectListItem
                    {
                        Text = c.Name,
                        Value = c.Id.ToString()
                    }).ToList(),
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

        /// <summary>
        /// Adds a new product or updates an existing product based on the provided model and files.
        /// </summary>
        /// <param name="productVM">The product view model containing the product data and category list.</param>
        /// <param name="files">A list of image files associated with the product.</param>
        /// <returns>Redirects to the Index action upon success; returns an error view if unsuccessful.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductVM productVM, List<IFormFile> files)
        {
            try
            {
                _logger.LogInformation(productVM.Product.Id == 0
                    ? "Creating a new product."
                    : $"Updating the product with ID: {productVM.Product.Id}.");

                var result = await _productService.UpsertProductAsync(productVM, files, _webHostEnvironment.WebRootPath);
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

        /// <summary>
        /// Deletes a product image with the specified image ID.
        /// </summary>
        /// <param name="imageId">The ID of the image to be deleted.</param>
        /// <returns>Redirects to the Upsert action with the image ID.</returns>
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

        /// <summary>
        /// Retrieves all products and returns them as JSON data for API consumption.
        /// </summary>
        /// <returns>JSON containing the list of products.</returns>

        [HttpGet]
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

        /// <summary>
        /// Deletes a product with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the product to be deleted.</param>
        /// <returns>A JSON response indicating the success or failure of the deletion.</returns>
        [HttpDelete]
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
