using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contract.ViewModels;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Domain.Entities.Identity;
using ECommerceCore.Infrastructure.External.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace ECommerceCore.Web.Controllers
{
    [Authorize(Roles = AppConstants.Role_Customer)]
    [Area("Customer")]
    public class CartController(IUserService userService, ICartService cartService, IOrderService orderService, IPaymentService paymentService, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ILogger<CartController> logger) : Controller
    {
        private readonly IUserService _userService = userService;
        private readonly ICartService _cartService = cartService;
        private readonly IOrderService _orderService = orderService;
        private readonly IPaymentService _paymentService = paymentService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<CartController> _logger = logger;

        /// <summary>
        /// Displays the shopping cart page for authenticated or anonymous users.
        /// </summary>
        [AllowAnonymous] // Allow both anonymous and authenticated users
        public async Task<IActionResult> Index()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _logger.LogInformation("Fetching cart details for user ID {UserId}.", userId);

                ShoppingCartVM shoppingCartVM;

                if (User.Identity.IsAuthenticated)
                {
                    // Fetch shopping cart details for authenticated users
                    shoppingCartVM = await _cartService.GetCartDetails(userId);
                    _logger.LogInformation("Cart details retrieved for authenticated user.");
                }
                else
                {
                    // Fetch shopping cart details for anonymous users
                    shoppingCartVM = await GetAnonymousCartDetails();
                    _logger.LogInformation("Cart details retrieved for anonymous user.");
                }

                return View(shoppingCartVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching cart details.");
                return View("Error");
            }
        }

        /// <summary>
        /// Increments the quantity of a cart item.
        /// </summary>
        public async Task<IActionResult> Plus(int cartId, int productId)
        {
            try
            {
                bool isAuthenticated = User.Identity.IsAuthenticated;
                _logger.LogInformation("Incrementing cart item with ID {CartId}.", cartId);
                await _cartService.IncrementCartItem(cartId, isAuthenticated, productId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while incrementing cart item with ID {CartId}.", cartId);
                return View("Error");
            }
        }

        /// <summary>
        /// Decrements the quantity of a cart item.
        /// </summary>
        public async Task<IActionResult> Minus(int cartId, int productId)
        {
            try
            {
                bool isAuthenticated = User.Identity.IsAuthenticated;
                _logger.LogInformation("Decrementing cart item with ID {CartId}.", cartId);
                await _cartService.DecrementCartItem(cartId, isAuthenticated, productId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while decrementing cart item with ID {CartId}.", cartId);
                return View("Error");
            }
        }

        /// <summary>
        /// Removes an item from the shopping cart.
        /// </summary>
        public async Task<IActionResult> Remove(int cartId, int productId)
        {
            try
            {
                bool isAuthenticated = User.Identity.IsAuthenticated;
                _logger.LogInformation("Removing cart item with ID {CartId}.", cartId);
                await _cartService.RemoveCartItem(cartId, isAuthenticated, productId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing cart item with ID {CartId}.", cartId);
                return View("Error");
            }
        }

        /// <summary>
        /// Displays the order summary page for authenticated users.
        /// </summary>
        [Authorize(Roles = AppConstants.Role_Customer)]
        public async Task<IActionResult> Summary()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _logger.LogInformation("Fetching summary details for user ID {UserId}.", userId);

                var shoppingCartVM = await _cartService.GetSummaryDetails(userId);
                return View(shoppingCartVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching summary details.");
                return View("Error");
            }
        }

        /// <summary>
        /// Submits the order and handles payment.
        /// </summary>
        [HttpPost]
        [ActionName("Summary")]
        [Authorize(Roles = AppConstants.Role_Customer)]
        public async Task<IActionResult> SummaryPost()
        {
            try
            {
                var userId = _userService.GetUserId(User);

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Unauthorized access attempt in Summary Post.");
                    return Unauthorized();
                }
                // Fetch the application user
                var applicationUser = await _userService.GetApplicationUser(userId);
                if (applicationUser == null)
                {
                    _logger.LogError("User with ID {UserId} not found.", userId);
                    return BadRequest("Invalid user.");
                }
                // Initialize ShoppingCartVM
                var shoppingCartVM = new ShoppingCartVM
                {
                    OrderHeader = new OrderHeader
                    {
                        ApplicationUserId = userId,
                        OrderDate = DateTime.Now,
                    },
                    ShoppingCartList = await _cartService.GetUserCart(userId)
                };

                // Map user details to the order header
                MapUserDetails(shoppingCartVM.OrderHeader, applicationUser);

                _logger.LogInformation("Preparing summary and creating order for user ID {UserId}.", userId);

                // Calculate order total
                _orderService.CalculateOrderTotal(shoppingCartVM);

                // Set payment and order status
                _orderService.SetOrderStatus(applicationUser, shoppingCartVM.OrderHeader);

                // Save OrderHeader and OrderDetails
                await _orderService.CreateOrder(shoppingCartVM);

                // Handle payment logic for regular customers
                if (applicationUser.CompanyId.GetValueOrDefault() == 0)
                {
                    var session = _paymentService.CreateStripeSession(shoppingCartVM, Request);
                    _orderService.UpdateStripePaymentDetails(shoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);

                    Response.Headers.Add("Location", session.Url);
                    return new StatusCodeResult(303); // Redirect to Stripe payment URL
                }

                _logger.LogInformation("Order successfully created with ID {OrderId}. Redirecting to OrderConfirmation.", shoppingCartVM.OrderHeader.Id);
                return RedirectToAction("OrderConfirmation", "Cart", new { id = shoppingCartVM.OrderHeader.Id });

                //return RedirectToAction(nameof(OrderConfirmation), new { id = shoppingCartVM.OrderHeader.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during SummaryPost.");
                return View("Error");
            }
        }

        /// <summary>
        /// Displays the order confirmation page.
        /// </summary>
        [Authorize(Roles = AppConstants.Role_Customer)]
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            try
            {
                _logger.LogInformation("Handling order confirmation for order ID {OrderId}.", id);
                await _orderService.HandleOrderConfirmation(id, HttpContext);
                return View(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during order confirmation for order ID {OrderId}.", id);
                return View("Error");
            }
        }

        /// <summary>
        /// Maps user details to the order header.
        /// </summary>
        private void MapUserDetails(OrderHeader orderHeader, ApplicationUser user)
        {
            orderHeader.ShippingContactName = user.Name;
            orderHeader.ShipToAddress.ShippingCity = user.City;
            orderHeader.ShippingContactPhone = user.PhoneNumber;
            orderHeader.ShipToAddress.ShippingState = user.State;
            orderHeader.ShipToAddress.ShippingZipCode = user.PostalCode;
            orderHeader.ShipToAddress.ShippingAddress1 = user.Address1;
            orderHeader.ShipToAddress.ShippingAddress2 = user.Address2;
        }

        /// <summary>
        /// Retrieves shopping cart details for anonymous users (from cookies).
        /// </summary>
        private async Task<ShoppingCartVM> GetAnonymousCartDetails()
        {
            var tempCart = GetCartFromCookie();

            var shoppingCartVM = new ShoppingCartVM
            {
                ShoppingCartList = tempCart,
                OrderHeader = new OrderHeader
                {
                    OrderTotal = 0 // Initialize total price
                }
            };

            // Populate Product details for each cart item and calculate prices
            foreach (var cartItem in shoppingCartVM.ShoppingCartList)
            {
                if (cartItem.ProductId != 0)
                {
                    // Get product details
                    cartItem.Product = await _unitOfWork.Products.GetAsync(p => p.Id == cartItem.ProductId);

                    // Calculate price based on quantity
                    cartItem.Price = _cartService.GetPriceBasedOnQuantity(cartItem);

                    // Update the order total
                    shoppingCartVM.OrderHeader.OrderTotal += (decimal)(cartItem.Price * cartItem.Count);
                }
            }

            return shoppingCartVM;
        }

        /// <summary>
        /// Retrieves cart data from browser cookies.
        /// </summary>
        private List<ShoppingCart> GetCartFromCookie()
        {
            try
            {
                var cartJson = Request.Cookies["TempCart"];
                return string.IsNullOrEmpty(cartJson)
                    ? new List<ShoppingCart>()
                    : JsonSerializer.Deserialize<List<ShoppingCart>>(cartJson);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving the cart from cookies.");
                return new List<ShoppingCart>();
            }
        }
    }
}
