using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.ViewModels;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Infrastructure.External.Payments;
using ECommerceCore.Domain.Enums;

namespace ECommerceCore.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class OrderController(IOrderService orderService, IPaymentService paymentService, ILogger<OrderController> logger) : Controller
    {
        private readonly IOrderService _orderService = orderService;
        private readonly IPaymentService _paymentService = paymentService;
        private readonly ILogger<OrderController> _logger = logger;

        /// <summary>
        /// Displays the index view for orders.
        /// </summary>
        /// <returns>The index view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Retrieves the details of a specific order by its ID.
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve details for.</param>
        /// <returns>The order details view.</returns>
        public async Task<IActionResult> Details(int orderId)
        {
            try
            {
                OrderVM orderVM = await _orderService.GetOrderDetailsAsync(orderId);
                return View(orderVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order details for OrderId: {OrderId}", orderId);
                return View("Error"); // Consider creating an Error view
            }
        }

        /// <summary>
        /// Updates the details of an existing order.
        /// </summary>
        /// <param name="orderVM">The view model containing order details to update.</param>
        /// <returns>A redirect to the order details view.</returns>
        [HttpPost]
        [Authorize(Roles = $"{AppConstants.Role_Admin},{AppConstants.Role_Employee}")]
        public async Task<IActionResult> UpdateOrderDetails(OrderVM orderVM)
        {
            try
            {
                await _orderService.UpdateOrderDetailsAsync(orderVM.OrderHeader);
                TempData["Success"] = "Order details updated successfully.";
                _logger.LogInformation("Order details updated for OrderId: {OrderId}", orderVM.OrderHeader.Id);
                return RedirectToAction(nameof(Details), new { orderId = orderVM.OrderHeader.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order details for OrderId: {OrderId}", orderVM.OrderHeader.Id);
                TempData["Error"] = "Error updating order details.";
                return RedirectToAction(nameof(Details), new { orderId = orderVM.OrderHeader.Id });
            }
        }

        /// <summary>
        /// Starts processing an order.
        /// </summary>
        /// <param name="orderId">The ID of the order to start processing.</param>
        /// <returns>A redirect to the order details view.</returns>
        [HttpPost]
        [Authorize(Roles = $"{AppConstants.Role_Admin},{AppConstants.Role_Employee}")]
        public async Task<IActionResult> StartProcessing(int orderId)
        {
            try
            {
                await _orderService.UpdateOrderStatusAsync(orderId, OrderStatus.Processing.ToString());
                TempData["Success"] = "Order processing started.";
                _logger.LogInformation("Started processing for OrderId: {OrderId}", orderId);
                return RedirectToAction(nameof(Details), new { orderId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting processing for OrderId: {OrderId}", orderId);
                TempData["Error"] = "Error starting order processing.";
                return RedirectToAction(nameof(Details), new { orderId });
            }
        }

        /// <summary>
        /// Ships an order.
        /// </summary>
        /// <param name="orderVM">The view model containing order shipment details.</param>
        /// <returns>A redirect to the order details view.</returns>
        [HttpPost]
        [Authorize(Roles = $"{AppConstants.Role_Admin},{AppConstants.Role_Employee}")]
        public async Task<IActionResult> ShipOrder(OrderVM orderVM)
        {
            try
            {
                await _orderService.ShipOrderAsync(orderVM.OrderHeader.Id, orderVM.OrderHeader.Carrier, orderVM.OrderHeader.TrackingNumber);
                TempData["Success"] = "Order shipped successfully.";
                _logger.LogInformation("Order shipped for OrderId: {OrderId}", orderVM.OrderHeader.Id);
                return RedirectToAction(nameof(Details), new { orderId = orderVM.OrderHeader.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error shipping order for OrderId: {OrderId}", orderVM.OrderHeader.Id);
                TempData["Error"] = "Error shipping order.";
                return RedirectToAction(nameof(Details), new { orderId = orderVM.OrderHeader.Id });
            }
        }

        /// <summary>
        /// Cancels an order.
        /// </summary>
        /// <param name="orderId">The ID of the order to cancel.</param>
        /// <returns>A redirect to the order details view.</returns>
        [HttpPost]
        [Authorize(Roles = $"{AppConstants.Role_Admin},{AppConstants.Role_Employee}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                await _orderService.CancelOrderAsync(orderId);
                TempData["Success"] = "Order cancelled successfully.";
                _logger.LogInformation("Order cancelled for OrderId: {OrderId}", orderId);
                return RedirectToAction(nameof(Details), new { orderId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order for OrderId: {OrderId}", orderId);
                TempData["Error"] = "Error cancelling order.";
                return RedirectToAction(nameof(Details), new { orderId });
            }
        }

        /// <summary>
        /// Creates a Stripe session for payment processing.
        /// </summary>
        /// <param name="orderHeaderId">The ID of the order header.</param>
        /// <returns>A redirect to the Stripe Checkout or a BadRequest if the session creation fails.</returns>
        [ActionName("Details")]
        [HttpPost]
        public async Task<IActionResult> Details_PAY_NOW(int orderHeaderId)
        {
            try
            {
                var result = await _paymentService.CreateStripeSession(orderHeaderId, Request.Scheme, Request.Host.Value);
                if (!string.IsNullOrEmpty(result))
                {
                    Response.Headers.Add("Location", result);
                    return new StatusCodeResult(303); // Redirect to Stripe Checkout
                }
                return BadRequest("Failed to create Stripe session.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Stripe session for OrderHeaderId: {OrderHeaderId}", orderHeaderId);
                return BadRequest("Failed to create Stripe session due to an error.");
            }
        }

        /// <summary>
        /// Confirms payment for an order.
        /// </summary>
        /// <param name="orderHeaderId">The ID of the order header.</param>
        /// <returns>An OK result if payment is confirmed; otherwise, a BadRequest.</returns>
        public async Task<IActionResult> PaymentConfirmation(int orderHeaderId)
        {
            try
            {
                var success = await _paymentService.ConfirmPayment(orderHeaderId);
                if (success)
                {
                    return Ok("Payment confirmed.");
                }
                return BadRequest("Payment confirmation failed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error confirming payment for OrderHeaderId: {OrderHeaderId}", orderHeaderId);
                return BadRequest("Payment confirmation failed due to an error.");
            }
        }

        /// <summary>
        /// Retrieves all orders based on the specified status.
        /// </summary>
        /// <param name="status">The status to filter orders.</param>
        /// <returns>A JSON object containing the list of orders.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(string status)
        {
            try
            {
                var orders = await _orderService.GetOrdersByStatusAsync(User, status);
                return Json(new { data = orders });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders by status: {Status}", status);
                return BadRequest("Failed to retrieve orders.");
            }
        }
    }
}