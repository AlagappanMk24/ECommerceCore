using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contracts.ViewModels.Customers
{
    // View model for customer creation/editing
    public class CustomerUpsertVM
    {
        public Customer Customer { get; set; }
        public List<Company> Companies { get; set; } = new List<Company>();
    }
}
