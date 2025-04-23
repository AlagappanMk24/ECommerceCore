using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contract.ViewModels;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Domain.Enums;
using ECommerceCore.Infrastructure.Services.Email;
using Microsoft.AspNetCore.Http;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace ECommerceCore.Infrastructure.Services
{
    public class OrderService(IUnitOfWork unitOfWork, IEmailService emailService) : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IEmailService _emailService = emailService;

        /// <summary>
        /// Retrieves the details of an order, including the order header and its associated details.
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve.</param>
        /// <returns>An instance of <see cref="OrderVM"/> containing order details.</returns>
        public async Task<OrderVM> GetOrderDetailsAsync(int orderId)
        {
            return new OrderVM
            {
                OrderHeader = await _unitOfWork.OrderHeaders.GetAsync(o => o.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = await _unitOfWork.OrderDetails.GetAllAsync(d => d.OrderHeaderId == orderId, includeProperties: "Product")
            };
        }

        /// <summary>
        /// Updates the details of an existing order header with the provided information.
        /// </summary>
        /// <param name="orderHeader">An instance of <see cref="OrderHeader"/> containing updated information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateOrderDetailsAsync(OrderHeader orderHeader)
        {
            var orderFromDb = await _unitOfWork.OrderHeaders.GetAsync(o => o.Id == orderHeader.Id);
            orderFromDb.Name = orderHeader.Name;
            orderFromDb.PhoneNumber = orderHeader.PhoneNumber;
            orderFromDb.StreetAddress = orderHeader.StreetAddress;
            orderFromDb.City = orderHeader.City;
            orderFromDb.State = orderHeader.State;
            orderFromDb.PostalCode = orderHeader.PostalCode;
            orderFromDb.Carrier = orderHeader.Carrier ?? orderFromDb.Carrier;
            orderFromDb.TrackingNumber = orderHeader.TrackingNumber ?? orderFromDb.TrackingNumber;

            _unitOfWork.OrderHeaders.Update(orderFromDb);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Updates the status of an order identified by the specified order ID.
        /// </summary>
        /// <param name="orderId">The ID of the order whose status is to be updated.</param>
        /// <param name="status">The new status to set for the order.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            await _unitOfWork.OrderHeaders.UpdateStatusAsync(orderId, status);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Ships the specified order by updating the carrier and tracking information, and setting the order status to shipped.
        /// </summary>
        /// <param name="orderId">The ID of the order to ship.</param>
        /// <param name="carrier">The name of the shipping carrier.</param>
        /// <param name="trackingNumber">The tracking number for the shipment.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ShipOrderAsync(int orderId, string carrier, string trackingNumber)
        {
            var order = await _unitOfWork.OrderHeaders.GetAsync(o => o.Id == orderId);
            order.Carrier = carrier;
            order.TrackingNumber = trackingNumber;
            order.OrderStatus = OrderStatus.Shipped.ToString();
            order.ShippingDate = DateTime.Now;

            if (order.PaymentStatus == PaymentStatus.ApprovedForDelayedPayment.ToString())
            {
                order.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
            }

            _unitOfWork.OrderHeaders.Update(order);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Cancels the order specified by the order ID, processing a refund if the payment was approved.
        /// </summary>
        /// <param name="orderId">The ID of the order to cancel.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CancelOrderAsync(int orderId)
        {
            var order = await _unitOfWork.OrderHeaders.GetAsync(o => o.Id == orderId);

            if (order.PaymentStatus == PaymentStatus.Approved.ToString())
            {
                var refundService = new RefundService();
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = order.PaymentIntenId
                };
                refundService.Create(options);
            }

            await _unitOfWork.OrderHeaders.UpdateStatusAsync(orderId, OrderStatus.Cancelled.ToString(), OrderStatus.Refunded.ToString());
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Retrieves a list of orders based on the specified status, considering user roles for access.
        /// </summary>
        /// <param name="user">The user requesting the orders.</param>
        /// <param name="status">The status to filter the orders by.</param>
        /// <returns>A collection of <see cref="OrderHeader"/> representing the orders matching the criteria.</returns>
        public async Task<IEnumerable<OrderHeader>> GetOrdersByStatusAsync(ClaimsPrincipal user, string status)
        {
            IEnumerable<OrderHeader> orders;

            if (user.IsInRole(AppConstants.Role_Admin) || user.IsInRole(AppConstants.Role_Employee))
            {
                orders = await _unitOfWork.OrderHeaders.GetAllAsync(includeProperties: "ApplicationUser");
            }
            else
            {
                var userId = ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
                orders = await _unitOfWork.OrderHeaders.GetAllAsync(o => o.ApplicationUserId == userId, includeProperties: "ApplicationUser");
            }

            return status.ToLower() switch
            {
                "pending" => orders.Where(o => o.PaymentStatus == PaymentStatus.Pending.ToString()),
                "inprocess" => orders.Where(o => o.OrderStatus == OrderStatus.Processing.ToString()),
                "completed" => orders.Where(o => o.OrderStatus == OrderStatus.Shipped.ToString()),
                "approved" => orders.Where(o => o.OrderStatus == OrderStatus.Approved.ToString()),
                _ => orders
            };
        }

        /// <summary>
        /// Initializes a new instance of <see cref="OrderHeader"/> with the specified user ID.
        /// </summary>
        /// <param name="userId">The ID of the user associated with the order.</param>
        /// <returns>A new <see cref="OrderHeader"/> instance with the current date.</returns>
        public OrderHeader InitializeOrderHeader(string userId)
        {
            return new OrderHeader
            {
                ApplicationUserId = userId,
                OrderDate = DateTime.Now
            };
        }

        /// <summary>
        /// Calculates the total price of the order based on the quantities in the shopping cart.
        /// </summary>
        /// <param name="cartVM">The shopping cart view model containing the items to calculate.</param>
        public void CalculateOrderTotal(ShoppingCartVM cartVM)
        {
            cartVM.OrderHeader.OrderTotal = 0; // Reset total before calculating

            foreach (var cart in cartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart); 
                cartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
        }

        /// <summary>
        /// Sets the payment and order status based on the user's company association.
        /// </summary>
        /// <param name="user">The user associated with the order.</param>
        /// <param name="orderHeader">The order header to update.</param>
        public void SetOrderStatus(ApplicationUser user, OrderHeader orderHeader)
        {
            if (user.CompanyId.GetValueOrDefault() == 0)
            {
                orderHeader.PaymentStatus = PaymentStatus.Pending.ToString();
                orderHeader.OrderStatus = OrderStatus.Pending.ToString();
            }
            else
            {
                orderHeader.PaymentStatus = PaymentStatus.ApprovedForDelayedPayment.ToString();
                orderHeader.OrderStatus = OrderStatus.Approved.ToString();
            }
        }

        /// <summary>
        /// Creates a new order based on the shopping cart information provided in the view model.
        /// </summary>
        /// <param name="cartVM">The shopping cart view model containing the order details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateOrder(ShoppingCartVM cartVM)
        {
            await _unitOfWork.OrderHeaders.AddAsync(cartVM.OrderHeader);
            await _unitOfWork.SaveAsync();

            foreach (var cart in cartVM.ShoppingCartList)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = cartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                await _unitOfWork.OrderDetails.AddAsync(orderDetail);
            }
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Updates the Stripe payment details for the specified order.
        /// </summary>
        /// <param name="orderId">The ID of the order to update.</param>
        /// <param name="sessionId">The session ID from Stripe.</param>
        /// <param name="paymentIntentId">The payment intent ID from Stripe.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateStripePaymentDetails(int orderId, string sessionId, string paymentIntentId)
        {
            await _unitOfWork.OrderHeaders.UpdateStripePaymentIdAsync(orderId, sessionId, paymentIntentId);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Determines the price of a product based on the quantity specified in the shopping cart.
        /// </summary>
        /// <param name="cart">The shopping cart item to evaluate.</param>
        /// <returns>The price based on the quantity.</returns>
        private double GetPriceBasedOnQuantity(ShoppingCart cart)
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

        /// <summary>
        /// Handles the order confirmation process, updating the order status and removing shopping cart items if necessary.
        /// </summary>
        /// <param name="id">The ID of the order to confirm.</param>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task HandleOrderConfirmation(int id, HttpContext httpContext)
        {
            // Retrieve the order header with ApplicationUser details
            var orderHeader = await _unitOfWork.OrderHeaders.GetAsync(u => u.Id == id, includeProperties: "ApplicationUser");

            if (orderHeader == null)
            {
                throw new InvalidOperationException($"Order with ID {id} not found.");
            }

            // Handle payment status
            if (orderHeader.PaymentStatus != PaymentStatus.ApprovedForDelayedPayment.ToString())
            {
                var sessionService = new SessionService();
                var session = sessionService.Get(orderHeader.SessionId);

                if (session != null && session.PaymentStatus.Equals("paid", StringComparison.OrdinalIgnoreCase))
                {
                    await _unitOfWork.OrderHeaders.UpdateStripePaymentIdAsync(id, session.Id, session.PaymentIntentId);
                    await _unitOfWork.OrderHeaders.UpdateStatusAsync(id, OrderStatus.Approved.ToString(), PaymentStatus.Approved.ToString());
                    await _unitOfWork.SaveAsync();
                }
                // Clear session if the payment is complete
                httpContext.Session.Clear();
            }

            // Remove shopping cart items for the user
            var shoppingCarts = await _unitOfWork.ShoppingCarts.GetAllAsync(u => u.ApplicationUserId == orderHeader.ApplicationUserId);
            if (shoppingCarts.Any())
            {
                await _unitOfWork.ShoppingCarts.RemoveRangeAsync(shoppingCarts);
                await _unitOfWork.SaveAsync();
            }

            if (!string.IsNullOrEmpty(orderHeader.ApplicationUser?.Email))
            {
                await _emailService.SendOrderConfirmEmailAsync(orderHeader.ApplicationUser.Email,
                    "New Order Confirmation",
                    orderHeader);
            }
        }
    }
}