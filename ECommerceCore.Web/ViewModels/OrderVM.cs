using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Web.ViewModels
{
    public class OrderVM
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetail> OrderDetail { get; set; }
    }
}
