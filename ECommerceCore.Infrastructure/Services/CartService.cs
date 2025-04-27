using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contract.ViewModels;
using ECommerceCore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ECommerceCore.Infrastructure.Services
{
    public class CartService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor) : ICartService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        private const string CartCookieName = "TempCart"; // Name of the cookie

        /// <summary>
        /// Fetches the shopping cart details for a specific user, including product images and calculating the order total.
        /// </summary>
        public async Task<ShoppingCartVM> GetCartDetails(string userId)
        {
            var shoppingCartVM = new ShoppingCartVM
            {
                ShoppingCartList = await _unitOfWork.ShoppingCarts.GetAllAsync(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            // Retrieve all product images to populate cart items with their respective images
            IEnumerable<ProductImage> productImages = await _unitOfWork.ProductImages.GetAllAsync();

            foreach (var cart in shoppingCartVM.ShoppingCartList)
            {
                // Match product images with their products
                cart.Product.ProductImages = productImages.Where(u => u.ProductId == cart.Product.Id).ToList();
                // Calculate price based on quantity
                cart.Price = GetPriceBasedOnQuantity(cart);
                // Update the order total
                shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            // Return the complete shopping cart details
            return shoppingCartVM;
        }

        /// <summary>
        /// Increments the quantity of a specific cart item by 1 and updates it in the database.
        /// </summary>
        public async Task IncrementCartItem(int cartId, bool isAuthenticated, int productId)
        {
            if (isAuthenticated)
            {
                // Fetch the cart item by ID
                var cartFromDb = await _unitOfWork.ShoppingCarts.GetAsync(u => u.Id == cartId);
                cartFromDb.Count += 1;
                _unitOfWork.ShoppingCarts.Update(cartFromDb);
                await _unitOfWork.SaveAsync();
                await UpdateSessionCartCountAsync(cartFromDb.ApplicationUserId);
            }
            else
            {
                IncrementAnonymousCartItem(productId);
            }
        }

        /// <summary>
        /// Decrements the quantity of a specific cart item by 1. Removes the cart item if the quantity reaches 1 and updates the session cart count.
        /// </summary>
        public async Task DecrementCartItem(int cartId, bool isAuthenticated, int productId)
        {
            if (isAuthenticated)
            {
                // Fetch the cart item with tracking enabled
                var cartFromDb = await _unitOfWork.ShoppingCarts.GetAsync(u => u.Id == cartId, tracked: true);
                if (cartFromDb != null)
                {
                    if (cartFromDb.Count <= 1)
                    {
                        await RemoveCartItem(cartId, isAuthenticated, productId);
                    }
                    else
                    {
                        // Decrement the item count
                        cartFromDb.Count -= 1;
                        // Update the cart in the database
                        _unitOfWork.ShoppingCarts.Update(cartFromDb);
                    }
                }
                await _unitOfWork.SaveAsync();
                await UpdateSessionCartCountAsync(cartFromDb.ApplicationUserId);
            }
            else
            {
                DecrementAnonymousCartItem(productId);
            }
        }

        /// <summary>
        /// Removes a specific cart item from the database and updates the session cart count for the user.
        /// </summary>
        public async Task RemoveCartItem(int cartId, bool isAuthenticated, int productId)
        {
            if (isAuthenticated)
            {
                // Fetch the cart item with tracking enabled
                var cartFromDb = await _unitOfWork.ShoppingCarts.GetAsync(u => u.Id == cartId, tracked: true);
                if (cartFromDb != null)
                {
                    string userId = cartFromDb.ApplicationUserId;
                    await _unitOfWork.ShoppingCarts.RemoveAsync(cartFromDb);
                    await _unitOfWork.SaveAsync();
                    await UpdateSessionCartCountAsync(userId);
                }
            }
            else
            {
                RemoveAnonymousCartItem(productId);
            }
        }
        public async Task<IEnumerable<ShoppingCart>> GetUserCart(string userId)
        {
            return await _unitOfWork.ShoppingCarts.GetAllAsync(u => u.ApplicationUserId == userId, includeProperties: "Product");
        }
        /// <summary>
        /// Retrieves a detailed summary of the shopping cart for a user, including user details and the order total.
        /// </summary>
        public async Task<ShoppingCartVM> GetSummaryDetails(string userId)
        {
            // Fetch cart details and initialize order header
            var shoppingCartVM = new ShoppingCartVM
            {
                ShoppingCartList = await _unitOfWork.ShoppingCarts.GetAllAsync(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };
            // Fetch user details
            shoppingCartVM.OrderHeader.ApplicationUser = await _unitOfWork.ApplicationUsers.GetAsync(u => u.Id == userId);

            // Map user details to the order header
            MapUserDetails(shoppingCartVM.OrderHeader, shoppingCartVM.OrderHeader.ApplicationUser);

            shoppingCartVM.ShoppingCartList = await _unitOfWork.ShoppingCarts.GetAllAsync(u => u.ApplicationUserId == userId, includeProperties: "Product,Product.ProductImages");

            foreach (var cart in shoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                shoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return shoppingCartVM;
        }

        /// <summary>
        /// Updates the cart item count in the user’s session based on the number of items in the shopping cart.
        /// </summary>
        private async Task UpdateSessionCartCountAsync(string userId)
        {
            var cartItems = await _unitOfWork.ShoppingCarts.GetAllAsync(u => u.ApplicationUserId == userId);
            int totalCartQuantity = cartItems.Any() ? cartItems.Sum(item => item.Count) : 0; // Ensure 0 when empty

            // Store the updated count in session
            _httpContextAccessor.HttpContext.Session.SetInt32(AppConstants.SessionCart, totalCartQuantity);

            // Also update the cookie to keep it in sync
            _httpContextAccessor.HttpContext.Response.Cookies.Append("CartCount", totalCartQuantity.ToString(), new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(1),
                HttpOnly = true
            });
            //var newCartCount = await _unitOfWork.ShoppingCart.CountAsync(u => u.ApplicationUserId == userId);
            //_httpContextAccessor.HttpContext.Session.SetInt32(AppConstants.SessionCart, newCartCount -1);
            //var cartItems = await _unitOfWork.ShoppingCart.GetAllAsync(u => u.ApplicationUserId == userId);
            //int totalCartQuantity = cartItems.Sum(item => item.Count);

            //// Store the total count in session or cookies
            //_httpContextAccessor.HttpContext.Session.SetInt32("CartCount", totalCartQuantity);
        }

        /// <summary>
        /// Maps user details (like name, phone, and address) to the OrderHeader object for order processing.
        /// </summary>
        private void MapUserDetails(OrderHeader orderHeader, ApplicationUser user)
        {
            orderHeader.ShippingContactName = user.Name;
            orderHeader.ShippingContactPhone = user.PhoneNumber;
            orderHeader.ShipToAddress.ShippingAddress1 = user.Address1;
            orderHeader.ShipToAddress.ShippingAddress2 = user.Address2;
            orderHeader.ShipToAddress.ShippingCity = user.City;
            orderHeader.ShipToAddress.ShippingState = user.State;
            orderHeader.ShipToAddress.ShippingZipCode = user.PostalCode;
        }

        /// <summary>
        /// Determines the price of a product based on the quantity in the shopping cart.
        /// </summary>
        public double GetPriceBasedOnQuantity(ShoppingCart cart)
        {
            // Check if the product is discounted and if the discount is currently active
            if (cart.Product.IsDiscounted &&
                (!cart.Product.DiscountStartDate.HasValue || cart.Product.DiscountStartDate.Value <= DateTime.Now) &&
                (!cart.Product.DiscountEndDate.HasValue || cart.Product.DiscountEndDate.Value >= DateTime.Now))
            {
                return cart.Product.DiscountPrice;
            }

            // Return regular price if not discounted or discount is not active
            return cart.Product.Price;
        }
        private void IncrementAnonymousCartItem(int productId)
        {
            var tempCart = GetCartFromCookie();

            // Find the cart item to increment
            var cartItem = tempCart.FirstOrDefault(item => item.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Count += 1; // Increment the count
                
            }
            SaveCartToCookie(tempCart); // Save the updated cart back to the cookie
        }

        private void DecrementAnonymousCartItem(int productId)
        {
            var tempCart = GetCartFromCookie();

            // Find the cart item to decrement
            var cartItem = tempCart.FirstOrDefault(item => item.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Count -= 1; // Decrement the count
                if (cartItem.Count <= 0)
                {
                    tempCart.Remove(cartItem); // Remove item if count is 0
                }
                SaveCartToCookie(tempCart); // Save the updated cart back to the cookie
            }
        }

        private void RemoveAnonymousCartItem(int productId)
        {
            var tempCart = GetCartFromCookie();

            // Find the cart item to remove
            var cartItem = tempCart.FirstOrDefault(item => item.ProductId == productId);
            if (cartItem != null)
            {
                tempCart.Remove(cartItem); // Remove the item
                SaveCartToCookie(tempCart); // Save the updated cart back to the cookie
            }
        }
        private List<ShoppingCart> GetCartFromCookie()
        {
            var cartJson = _httpContextAccessor.HttpContext.Request.Cookies[CartCookieName];
            return string.IsNullOrEmpty(cartJson) ? new List<ShoppingCart>() : JsonSerializer.Deserialize<List<ShoppingCart>>(cartJson);
        }

        private void SaveCartToCookie(List<ShoppingCart> cart)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return; // Ensure HttpContext is available

            var cartJson = JsonSerializer.Serialize(cart);
            httpContext.Response.Cookies.Append("TempCart", cartJson,
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(1) });

            // Update cart count in cookies
            int totalCartQuantity = cart.Sum(item => item.Count);
            httpContext.Response.Cookies.Append("CartCount", totalCartQuantity.ToString(),
                new CookieOptions { Expires = DateTimeOffset.Now.AddDays(1) });
        }
    }
}