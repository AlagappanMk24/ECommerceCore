using ECommerceCore.Application.Common.QueryParams;
using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contracts.ViewModels.Invoices
{
    public class InvoiceIndexVM
    {
        public InvoiceQueryParameters QueryParameters { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
    }

    public class InvoiceQueryParameters : QueryParameters
    {
        public int? CustomerId { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
    }
}
