using ECommerceCore.Application.Common.Results;
using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contract.ViewModels;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Application.Contracts.ViewModels.Orders;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Domain.Entities.Identity;
using ECommerceCore.Domain.Enums;
using ECommerceCore.Infrastructure.Services.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace ECommerceCore.Infrastructure.Services
{
    public class OrderService(IUnitOfWork unitOfWork, IEmailService emailService) : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IEmailService _emailService = emailService;
        public async Task<OrderVM> GetOrderDetailsAsync(int orderId)
        {
            return new OrderVM
            {
                OrderHeader = await _unitOfWork.OrderHeaders.GetAsync(o => o.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetail = await _unitOfWork.OrderDetails.GetAllAsync(d => d.OrderHeaderId == orderId, includeProperties: "Product")
            };
        }
        public async Task<PaginatedResult<OrderDto>> GetOrdersPaginatedAsync(OrderQueryParameters parameters)
        {
            try
            {
                // Base query
                var query = _unitOfWork.OrderHeaders.Query()
                    .Include(o => o.ApplicationUser)
                    .Include(o => o.Customer)
                    .AsQueryable();

                // Apply filters
                // Order status filter
                if (!string.IsNullOrEmpty(parameters.OrderStatus))
                {
                    query = query.Where(o => o.OrderStatus == parameters.OrderStatus);
                }

                // Payment status filter
                if (!string.IsNullOrEmpty(parameters.PaymentStatus))
                {
                    query = query.Where(o => o.PaymentStatus == parameters.PaymentStatus);
                }

                // Date range filter
                if (parameters.StartDate.HasValue)
                {
                    query = query.Where(o => o.OrderDate >= parameters.StartDate.Value);
                }

                if (parameters.EndDate.HasValue)
                {
                    var endDate = parameters.EndDate.Value.AddDays(1).AddSeconds(-1); // End of the selected day
                    query = query.Where(o => o.OrderDate <= endDate);
                }

                // Customer name search
                if (!string.IsNullOrEmpty(parameters.CustomerName))
                {
                    string searchTerm = parameters.CustomerName.ToLower().Trim();
                    query = query.Where(o =>
                        (o.ApplicationUser != null && o.ApplicationUser.Name.ToLower().Contains(searchTerm)) ||
                        (o.Customer != null && o.Customer.Name.ToLower().Contains(searchTerm))
                    );
                }

                // Search term (general search)
                if (!string.IsNullOrEmpty(parameters.SearchTerm))
                {
                    string searchTerm = parameters.SearchTerm.ToLower().Trim();
                    query = query.Where(o =>
                        (o.ApplicationUser != null && (
                            o.ApplicationUser.Name.ToLower().Contains(searchTerm) ||
                            o.ApplicationUser.Email.ToLower().Contains(searchTerm) ||
                            o.ApplicationUser.PhoneNumber.Contains(searchTerm)
                        )) ||
                        (o.Customer != null && (
                            o.Customer.Name.ToLower().Contains(searchTerm) ||
                            o.Customer.Email.ToLower().Contains(searchTerm) ||
                            o.Customer.PhoneNumber.Contains(searchTerm)
                        )) ||
                        o.TrackingNumber.Contains(searchTerm) ||
                        o.OrderStatus.ToLower().Contains(searchTerm) ||
                        o.PaymentStatus.ToLower().Contains(searchTerm)
                    );
                }

                // Apply sorting
                if (!string.IsNullOrEmpty(parameters.SortColumn))
                {
                    query = parameters.SortColumn.ToLower() switch
                    {
                        "orderdate" => parameters.SortDirection == "asc" ?
                            query.OrderBy(o => o.OrderDate) : query.OrderByDescending(o => o.OrderDate),
                        "ordertotal" => parameters.SortDirection == "asc" ?
                            query.OrderBy(o => o.OrderTotal) : query.OrderByDescending(o => o.OrderTotal),
                        "customername" => parameters.SortDirection == "asc" ?
                            query.OrderBy(o => o.Customer != null ? o.Customer.Name : o.ApplicationUser.Name) :
                            query.OrderByDescending(o => o.Customer != null ? o.Customer.Name : o.ApplicationUser.Name),
                        "orderstatus" => parameters.SortDirection == "asc" ?
                            query.OrderBy(o => o.OrderStatus) : query.OrderByDescending(o => o.OrderStatus),
                        "paymentstatus" => parameters.SortDirection == "asc" ?
                            query.OrderBy(o => o.PaymentStatus) : query.OrderByDescending(o => o.PaymentStatus),
                        _ => query.OrderByDescending(o => o.OrderDate)
                    };
                }
                else
                {
                    // Default sort by order date (newest first)
                    query = query.OrderByDescending(o => o.OrderDate);
                }

                // Get total count before pagination
                var totalCount = await query.CountAsync();

                // Apply pagination
                var items = await query
                    .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                    .Take(parameters.PageSize)
                    .Select(o => new OrderDto
                    {
                        Id = o.Id,
                        CustomerName = o.Customer != null ? o.Customer.Name : (o.ApplicationUser != null ? o.ApplicationUser.Name : "Guest"),
                        CustomerEmail = o.Customer != null ? o.Customer.Email : (o.ApplicationUser != null ? o.ApplicationUser.Email : ""),
                        PhoneNumber = o.Customer != null ? o.Customer.PhoneNumber : (o.ApplicationUser != null ? o.ApplicationUser.PhoneNumber : ""),
                        OrderDate = o.OrderDate,
                        OrderTotal = o.OrderTotal,
                        OrderStatus = o.OrderStatus,
                        PaymentStatus = o.PaymentStatus,
                        TrackingNumber = o.TrackingNumber ?? "Not Available"
                    })
                    .ToListAsync();

                return new PaginatedResult<OrderDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = parameters.PageNumber,
                    PageSize = parameters.PageSize
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task UpdateOrderDetailsAsync(OrderHeader orderHeader)
        {
            var orderFromDb = await _unitOfWork.OrderHeaders.GetAsync(o => o.Id == orderHeader.Id);
            orderFromDb.ShippingContactName = orderHeader.ShippingContactName;
            orderFromDb.ShippingContactPhone = orderHeader.ShippingContactPhone;
            orderFromDb.ShipToAddress.ShippingAddress1 = orderHeader.ShipToAddress.ShippingAddress1;
            orderFromDb.ShipToAddress.ShippingAddress2 = orderHeader.ShipToAddress.ShippingAddress2;
            orderFromDb.ShipToAddress.ShippingCity = orderHeader.ShipToAddress.ShippingCity;
            orderFromDb.ShipToAddress.ShippingState = orderHeader.ShipToAddress.ShippingState;
            orderFromDb.ShipToAddress.ShippingZipCode = orderHeader.ShipToAddress.ShippingZipCode;
            orderFromDb.ShipToAddress.ShippingCountry = orderHeader.ShipToAddress.ShippingCountry;
            orderFromDb.Carrier = orderHeader.Carrier ?? orderFromDb.Carrier;
            orderFromDb.TrackingNumber = orderHeader.TrackingNumber ?? orderFromDb.TrackingNumber;

            _unitOfWork.OrderHeaders.Update(orderFromDb);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            await _unitOfWork.OrderHeaders.UpdateStatusAsync(orderId, status);
            await _unitOfWork.SaveAsync();
        }
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
        public async Task CancelOrderAsync(int orderId)
        {
            var order = await _unitOfWork.OrderHeaders.GetAsync(o => o.Id == orderId);

            if (order.PaymentStatus == PaymentStatus.Approved.ToString())
            {
                var refundService = new RefundService();
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = order.PaymentIntentId
                };
                refundService.Create(options);
            }

            await _unitOfWork.OrderHeaders.UpdateStatusAsync(orderId, OrderStatus.Cancelled.ToString(), OrderStatus.Refunded.ToString());
            await _unitOfWork.SaveAsync();
        }
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
        public OrderHeader InitializeOrderHeader(string userId)
        {
            return new OrderHeader
            {
                ApplicationUserId = userId,
                OrderDate = DateTime.Now
            };
        }
        public void CalculateOrderTotal(ShoppingCartVM cartVM)
        {
            cartVM.OrderHeader.OrderTotal = 0; // Reset total before calculating

            foreach (var cart in cartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart); 
                cartVM.OrderHeader.OrderTotal += (decimal)(cart.Price * cart.Count);
            }
        }
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
        public async Task UpdateStripePaymentDetails(int orderId, string sessionId, string paymentIntentId)
        {
            await _unitOfWork.OrderHeaders.UpdateStripePaymentIdAsync(orderId, sessionId, paymentIntentId);
            await _unitOfWork.SaveAsync();
        }
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