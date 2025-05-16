using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Infrastructure.External.Payments;
using ECommerceCore.Domain.Enums;
using ECommerceCore.Application.Contracts.ViewModels.Orders;

namespace ECommerceCore.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = AppConstants.Role_Admin)]
    [Route("Admin/Order")]
    public class OrderController(IOrderService orderService, IPaymentService paymentService, ILogger<OrderController> logger) : Controller
    {
        private readonly IOrderService _orderService = orderService;
        private readonly IPaymentService _paymentService = paymentService;
        private readonly ILogger<OrderController> _logger = logger;

        /// <summary>
        /// Displays the index view for orders.
        /// </summary>
        /// <returns>The index view.</returns>
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet("")]
        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("Fetching data for the order index page.");

                // Create default query parameters for initial state
                var viewModel = new OrderIndexVM
                {
                    QueryParameters = new OrderQueryParameters
                    {
                        PageNumber = 1,
                        PageSize = 10,
                        SortColumn = "orderdate",
                        SortDirection = "desc"
                    }
                };

                _logger.LogInformation("Successfully retrieved data for the order index page.");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the order index page.");
                TempData["Error"] = "Unable to load orders.";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost("get-orders")]
        public async Task<IActionResult> GetOrders([FromBody] OrderQueryParameters queryParams)
        {
            try
            {
                var result = await _orderService.GetOrdersPaginatedAsync(queryParams);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching orders");
                return StatusCode(500, new { error = "Error fetching orders" });
            }
        }

        [HttpGet("details/{id?}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                OrderVM orderVM = await _orderService.GetOrderDetailsAsync(id);
                return View(orderVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order details for OrderId: {OrderId}", id);
                return View("Error"); // Consider creating an Error view
            }
        }


        //[HttpPost]
        //[Authorize(Roles = $"{AppConstants.Role_Admin},{AppConstants.Role_Employee}")]
        //public async Task<IActionResult> UpdateOrderDetails(OrderVM orderVM)
        //{
        //    try
        //    {
        //        await _orderService.UpdateOrderDetailsAsync(orderVM.OrderHeader);
        //        TempData["Success"] = "Order details updated successfully.";
        //        _logger.LogInformation("Order details updated for OrderId: {OrderId}", orderVM.OrderHeader.Id);
        //        return RedirectToAction(nameof(Details), new { orderId = orderVM.OrderHeader.Id });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error updating order details for OrderId: {OrderId}", orderVM.OrderHeader.Id);
        //        TempData["Error"] = "Error updating order details.";
        //        return RedirectToAction(nameof(Details), new { orderId = orderVM.OrderHeader.Id });
        //    }
        //}

        //[HttpPost]
        //[Authorize(Roles = $"{AppConstants.Role_Admin},{AppConstants.Role_Employee}")]
        //public async Task<IActionResult> StartProcessing(int orderId)
        //{
        //    try
        //    {
        //        await _orderService.UpdateOrderStatusAsync(orderId, OrderStatus.Processing.ToString());
        //        TempData["Success"] = "Order processing started.";
        //        _logger.LogInformation("Started processing for OrderId: {OrderId}", orderId);
        //        return RedirectToAction(nameof(Details), new { orderId });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error starting processing for OrderId: {OrderId}", orderId);
        //        TempData["Error"] = "Error starting order processing.";
        //        return RedirectToAction(nameof(Details), new { orderId });
        //    }
        //}

        //[HttpPost]
        //[Authorize(Roles = $"{AppConstants.Role_Admin},{AppConstants.Role_Employee}")]
        //public async Task<IActionResult> ShipOrder(OrderVM orderVM)
        //{
        //    try
        //    {
        //        await _orderService.ShipOrderAsync(orderVM.OrderHeader.Id, orderVM.OrderHeader.Carrier, orderVM.OrderHeader.TrackingNumber);
        //        TempData["Success"] = "Order shipped successfully.";
        //        _logger.LogInformation("Order shipped for OrderId: {OrderId}", orderVM.OrderHeader.Id);
        //        return RedirectToAction(nameof(Details), new { orderId = orderVM.OrderHeader.Id });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error shipping order for OrderId: {OrderId}", orderVM.OrderHeader.Id);
        //        TempData["Error"] = "Error shipping order.";
        //        return RedirectToAction(nameof(Details), new { orderId = orderVM.OrderHeader.Id });
        //    }
        //}

        //[HttpPost]
        //[Authorize(Roles = $"{AppConstants.Role_Admin},{AppConstants.Role_Employee}")]
        //public async Task<IActionResult> CancelOrder(int orderId)
        //{
        //    try
        //    {
        //        await _orderService.CancelOrderAsync(orderId);
        //        TempData["Success"] = "Order cancelled successfully.";
        //        _logger.LogInformation("Order cancelled for OrderId: {OrderId}", orderId);
        //        return RedirectToAction(nameof(Details), new { orderId });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error cancelling order for OrderId: {OrderId}", orderId);
        //        TempData["Error"] = "Error cancelling order.";
        //        return RedirectToAction(nameof(Details), new { orderId });
        //    }
        //}

        //[ActionName("Details")]
        //[HttpPost]
        //public async Task<IActionResult> Details_PAY_NOW(int orderHeaderId)
        //{
        //    try
        //    {
        //        var result = await _paymentService.CreateStripeSession(orderHeaderId, Request.Scheme, Request.Host.Value);
        //        if (!string.IsNullOrEmpty(result))
        //        {
        //            Response.Headers.Add("Location", result);
        //            return new StatusCodeResult(303); // Redirect to Stripe Checkout
        //        }
        //        return BadRequest("Failed to create Stripe session.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error creating Stripe session for OrderHeaderId: {OrderHeaderId}", orderHeaderId);
        //        return BadRequest("Failed to create Stripe session due to an error.");
        //    }
        //}

        //public async Task<IActionResult> PaymentConfirmation(int orderHeaderId)
        //{
        //    try
        //    {
        //        var success = await _paymentService.ConfirmPayment(orderHeaderId);
        //        if (success)
        //        {
        //            return Ok("Payment confirmed.");
        //        }
        //        return BadRequest("Payment confirmation failed.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error confirming payment for OrderHeaderId: {OrderHeaderId}", orderHeaderId);
        //        return BadRequest("Payment confirmation failed due to an error.");
        //    }
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAll(string status)
        //{
        //    try
        //    {
        //        var orders = await _orderService.GetOrdersByStatusAsync(User, status);
        //        return Json(new { data = orders });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error retrieving orders by status: {Status}", status);
        //        return BadRequest("Failed to retrieve orders.");
        //    }
        //}
    }
}