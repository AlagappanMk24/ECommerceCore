using ECommerceCore.Application.Common.QueryParams;

namespace ECommerceCore.Application.Contracts.ViewModels.Companies
{
    public class CompanyIndexVM
    {
        public CompanyQueryParameters QueryParameters { get; set; }
        public List<string> States { get; set; } = new List<string>();
    }
    public class CompanyQueryParameters : QueryParameters
    {
        public string? State { get; set; }
    }
}