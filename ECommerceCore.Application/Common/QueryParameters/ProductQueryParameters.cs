namespace ECommerceCore.Application.Common.QueryParameters
{
    public class ProductQueryParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; } = "asc";
        public int? CategoryId { get; set; }
        public string? StockStatus { get; set; } // "in-stock", "low-stock", "out-of-stock"
    }
}
