using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contracts.ViewModels
{
    public class InvoiceIndexVM
    {
        public InvoiceQueryParameters QueryParameters { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
    }

    public class InvoiceQueryParameters
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; }
        public string? SearchTerm { get; set; }
        public int? CustomerId { get; set; }
        public string? Status { get; set; }
        public string? Type { get; set; }
    }
}
