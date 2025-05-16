using ECommerceCore.Application.Common.QueryParams;
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
    public class CustomerQueryParameters : QueryParameters
    {
        public int? CompanyId { get; set; }
    }
}
