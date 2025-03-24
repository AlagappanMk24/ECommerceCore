using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Application.Contract.ViewModels
{
    public class OrderVM
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetail> OrderDetail { get; set; }
    }
}
