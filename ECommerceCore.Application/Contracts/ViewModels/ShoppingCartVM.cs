using ECommerceCore.Domain.Models.Entities;

namespace ECommerceCore.Application.Contract.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
