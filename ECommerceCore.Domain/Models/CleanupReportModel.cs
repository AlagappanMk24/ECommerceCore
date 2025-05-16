namespace ECommerceCore.Domain.Models
{
    public class CleanupReportModel
    {
        public int DeletedCount { get; set; }
        public DateTime CleanupTime { get; set; }
        public int RetentionDays { get; set; }
        public DateTime Threshold { get; set; }
    }
}
