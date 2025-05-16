using ECommerceCore.Application.Common.QueryParams;

namespace ECommerceCore.Application.Contracts.ViewModels.Orders
{
    public class OrderIndexVM
    {
        public OrderQueryParameters QueryParameters { get; set; }
    }

    // OrderQueryParameters.cs
    public class OrderQueryParameters : QueryParameters
    {
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CustomerName { get; set; }
    }
}