using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contracts.ViewModels.Customers
{
    // View model for customer details page
    public class CustomerDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public AddressDto Address { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }

        // Related data summaries
        public int OrderCount { get; set; }
        public int InvoiceCount { get; set; }
        public decimal TotalSpent { get; set; }

        // Recent orders and invoices
        public List<OrderSummaryDto> RecentOrders { get; set; } = new List<OrderSummaryDto>();
        public List<InvoiceSummaryDto> RecentInvoices { get; set; } = new List<InvoiceSummaryDto>();
    }
}