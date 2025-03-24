using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Application.Contract.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
        Task Send2FACodeToEmailAsync(string email, string token);
        Task SendOrderConfirmEmailAsync(string email, string subject, OrderHeader orderHeader);
    }
}
