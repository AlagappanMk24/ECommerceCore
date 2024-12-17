using ECommerceCore.Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceCore.Web.ViewModels
{
    public class RoleManagementVM
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
        public IEnumerable<SelectListItem> CompanyList { get; set; }
    }
}
