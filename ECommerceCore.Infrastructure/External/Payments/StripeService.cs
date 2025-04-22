using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.ViewModels;
using ECommerceCore.Domain.Enums;
using ECommerceCore.Infrastructure.External.Payments;
using Microsoft.AspNetCore.Http;
using Stripe.Checkout;

namespace ECommerceCore.Infrastructure.Services
{
    public class StripeService(IUnitOfWork unitOfWork) : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Creates a Stripe payment session for the shopping cart and updates the Stripe payment ID in the order header.
        /// </summary>
        public Session CreateStripeSession(ShoppingCartVM shoppingCartVM, HttpRequest request)
        {
            var domain = request.Scheme + "://" + request.Host.Value + "/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={shoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + "Customer/Cart/Index",
                LineItems = shoppingCartVM.ShoppingCartList.Select(cart => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(cart.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions { Name = cart.Product.Title }
                    },
                    Quantity = cart.Count
                }).ToList(),
                Mode = "payment"
            };

            var service = new SessionService();
          
            return service.Create(options);
        }

        /// <summary>
        /// Creates a Stripe checkout session for the specified order, preparing it for payment.
        /// </summary>
        /// <param name="orderHeaderId">The ID of the order for which to create the session.</param>
        /// <param name="scheme">The URL scheme (http or https).</param>
        /// <param name="host">The host for the application.</param>
        /// <returns>The URL for the created Stripe checkout session.</returns>
        public async Task<string> CreateStripeSession(int orderHeaderId, string scheme, string host)
        {
            var orderHeader = await _unitOfWork.OrderHeaders.GetAsync(
                u => u.Id == orderHeaderId,
                includeProperties: "ApplicationUser"
            );
            var orderDetails = await _unitOfWork.OrderDetails.GetAllAsync(
                u => u.OrderHeaderId == orderHeaderId,
                includeProperties: "Product"
            );

            var domain = $"{scheme}://{host}/";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = domain + $"Admin/Order/PaymentConfirmation?orderHeaderId={orderHeaderId}",
                CancelUrl = domain + $"Admin/Order/details?orderId={orderHeaderId}",
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in orderDetails)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100), // Convert to cents
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new Stripe.Checkout.SessionService();
            var session = service.Create(options);

            await _unitOfWork.OrderHeaders.UpdateStripePaymentIdAsync(orderHeaderId, session.Id, session.PaymentIntentId);
            await _unitOfWork.SaveAsync();

            return session.Url;
        }

        /// <summary>
        /// Confirms the payment for the specified order if it is in a delayed payment state.
        /// </summary>
        /// <param name="orderHeaderId">The ID of the order to confirm the payment for.</param>
        /// <returns>A boolean indicating whether the payment was successfully confirmed.</returns>
        public async Task<bool> ConfirmPayment(int orderHeaderId)
        {
            var orderHeader = await _unitOfWork.OrderHeaders.GetAsync(
                u => u.Id == orderHeaderId,
                includeProperties: "ApplicationUser"
            );

            if (orderHeader.PaymentStatus == PaymentStatus.ApprovedForDelayedPayment.ToString())
            {
                var service = new SessionService();
                var session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    await _unitOfWork.OrderHeaders.UpdateStripePaymentIdAsync(orderHeaderId, session.Id, session.PaymentIntentId);
                    await _unitOfWork.OrderHeaders.UpdateStatusAsync(orderHeaderId, orderHeader.OrderStatus, PaymentStatus.Approved.ToString());
                    await _unitOfWork.SaveAsync();
                    return true;
                }
            }
            return false;
        }

    }
}
