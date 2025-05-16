using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Application.Contracts.ViewModels.Orders
{
    public class OrderVM
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetail> OrderDetail { get; set; }
    }
}
