namespace ECommerceCore.Application.Contracts.DTOs
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string InvoiceType { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
