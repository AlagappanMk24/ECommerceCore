﻿using ECommerceCore.Domain.Entities.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerceCore.Application.Contract.ViewModels
{
    public class RoleManagementVM
    {
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
        public IEnumerable<SelectListItem> CompanyList { get; set; }
    }
}
