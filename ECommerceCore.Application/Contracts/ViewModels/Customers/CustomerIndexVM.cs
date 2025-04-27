using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contracts.ViewModels.Customers
{
    // View model for the customer index page
    public class CustomerIndexVM
    {
        public CustomerQueryParameters QueryParameters { get; set; }
        public List<Company> Companies { get; set; } = new List<Company>();
    }

    // Query parameters for filtering and sorting customers
    public class CustomerQueryParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; }
        public int? CompanyId { get; set; }
    }
}
