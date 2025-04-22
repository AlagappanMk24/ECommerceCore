using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.Persistence;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Domain;

namespace ECommerceCore.Web.Controllers
{
    public class HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private const string CartCookieName = "TempCart"; // Name of the cookie

        /// <summary>
        /// Displays the index view with a list of products.
        /// If the user is logged in, it updates the shopping cart count in the session.
        /// </summary>
        /// <returns>A view with the list of products.</returns>
        public async Task<IActionResult> Index()
        {
            // Extract the user ID from claims
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // Initialize cart count
            int cartCount = 0;

            //To handle count of cart when your not login
            if (claims != null)
            {
                // Update the session cart count for the logged-in user
                var cartItems = await _unitOfWork.ShoppingCarts.GetAllAsync(u => u.ApplicationUserId == claims.Value);
                cartCount = cartItems.Count();
            }
            else
            {
                // Handle cart count for anonymous users
                var tempCart = GetCartFromCookie();
                cartCount = tempCart.Count; // Count of items in temporary cart
            }
            // Update session with cart count
            HttpContext.Session.SetInt32(AppConstants.SessionCart, cartCount);

            // Fetch the list of products with related properties for display
            IEnumerable<Product> productsList = await _unitOfWork.Products.GetAllAsync(includeProperties: "Category,ProductImages");
            return View(productsList);
        }

        /// <summary>
        /// Displays the details of a specific product in the shopping cart.
        /// </summary>
        /// <param name="productId">The ID of the product to display.</param>
        /// <returns>A view containing the shopping cart details for the specified product.</returns>
        public async Task<IActionResult> Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = await _unitOfWork.Products.GetAsync(u => u.Id == productId, includeProperties: "Category,ProductImages"),
                Count = 1,
                ProductId = productId
            };

            return View(cart);
        }

        /// <summary>
        /// Adds or updates a product in the shopping cart based on the submitted shopping cart information.
        /// If the product already exists in the cart, it updates the count; otherwise, it adds a new entry.
        /// </summary>
        /// <param name="shoppingCart">The shopping cart object containing the product and count.</param>
        /// <returns>A redirect to the index action with a success message.</returns>

        [HttpPost]
        public async Task<IActionResult> Details(ShoppingCart shoppingCart)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Extract the user ID from the claims
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId != null)
                {
                    shoppingCart.ApplicationUserId = userId;

                    // Check if the shopping cart entry already exists for this user and product
                    ShoppingCart cartFromDb = await _unitOfWork.ShoppingCarts.GetAsync(
                        u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);

                    if (cartFromDb != null)
                    {
                        // Update the existing cart entry
                        cartFromDb.Count += shoppingCart.Count;
                        _unitOfWork.ShoppingCarts.Update(cartFromDb);
                    }
                    else
                    {
                        // Add a new cart entry
                        await _unitOfWork.ShoppingCarts.AddAsync(shoppingCart);
                    }

                    // Save changes to the database
                    await _unitOfWork.SaveAsync();

                    // Counting unique products
                    //int cartCount = (await _unitOfWork.ShoppingCart.GetAllAsync(
                    //    u => u.ApplicationUserId == userId)).Count();

                    // Sum all quantities instead of counting unique products
                    int cartCount = (await _unitOfWork.ShoppingCarts.GetAllAsync(
                                        u => u.ApplicationUserId == userId))
                                        .Sum(cart => cart.Count); 

                    // Update the cart count in the session
                    HttpContext.Session.SetInt32(AppConstants.SessionCart, cartCount);

                    TempData["Success"] = "Cart updated successfully.";
                }
                else
                {
                    TempData["Error"] = "Unable to identify the user.";
                }
            }
            else
            {
                // For anonymous users, manage cart items in cookies
                var tempCart = GetCartFromCookie();
                //var existingItemIndex = tempCart.FindIndex(item => item.ProductId == shoppingCart.ProductId);
                var existingItem = tempCart.FirstOrDefault(item => item.ProductId == shoppingCart.ProductId);

                //if (existingItemIndex != -1)
                //{
                //    // Update the quantity if the item already exists in the temp cart
                //    tempCart[existingItemIndex].Count += shoppingCart.Count;
                //}
                if (existingItem != null)
                {
                    existingItem.Count += shoppingCart.Count;
                }
                else
                {
                    // Add a new item to the temp cart
                    //tempCart.Add(shoppingCart);
                    tempCart.Add(new ShoppingCart
                    {
                        ProductId = shoppingCart.ProductId,
                        Count = shoppingCart.Count
                    });
                }

                // Save the updated temp cart to cookies
                SetCartToCookie(tempCart);

                TempData["Success"] = "Item added to the temporary cart.";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Retrieves the shopping cart from the HTTP request cookies.
        /// </summary>
        /// <returns>A list of ShoppingCart objects, or an empty list if the cookie is not found or is empty.</returns>
        private List<ShoppingCart> GetCartFromCookie()
        {
            var cartJson = HttpContext.Request.Cookies[CartCookieName];
            return string.IsNullOrEmpty(cartJson) ? new List<ShoppingCart>() : JsonSerializer.Deserialize<List<ShoppingCart>>(cartJson);
        }

        /// <summary>
        /// Sets the shopping cart to the HTTP response cookies, including the full cart and the total quantity.
        /// </summary>
        /// <param name="cart">The list of ShoppingCart objects to be saved in the cookie.</param>
        private void SetCartToCookie(List<ShoppingCart> cart)
        {
            string cartJson = JsonSerializer.Serialize(cart);

            // Save the full cart as JSON
            HttpContext.Response.Cookies.Append("TempCart", cartJson,
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(1) });

            // Save total quantity separately for quick access
            int totalCartQuantity = cart.Sum(item => item.Count);
            HttpContext.Response.Cookies.Append("CartCount", totalCartQuantity.ToString(),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(1) });
        }

        /// <summary>
        /// Returns the Privacy view.
        /// </summary>
        /// <returns>The Privacy view.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Returns the Error view with the request ID.
        /// </summary>
        /// <returns>The Error view.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Returns the Landing Page view.
        /// </summary>
        /// <returns>The Landing Page view.</returns>
        public IActionResult LandingPage()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}