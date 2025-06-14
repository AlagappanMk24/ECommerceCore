﻿using ECommerceCore.Domain.Entities;
using ECommerceCore.Domain.Models;

namespace ECommerceCore.Infrastructure.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
        Task Send2FACodeToEmailAsync(string email, string token);
        Task SendOrderConfirmEmailAsync(string email, string subject, OrderHeader orderHeader);
        Task SendCleanupReportEmailAsync(IEnumerable<string> toEmails, CleanupReportModel report);
    }
}
