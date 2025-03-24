using ECommerceCore.Application.Contract.ViewModels;
using Microsoft.AspNetCore.Http;
using Stripe.Checkout;

namespace ECommerceCore.Application.Contract.Service
{
    public interface IPaymentService
    {
        Session CreateStripeSession(ShoppingCartVM shoppingCartVM, HttpRequest request);
        Task<string> CreateStripeSession(int orderHeaderId, string scheme, string host);
        Task<bool> ConfirmPayment(int orderHeaderId);
    }
}
