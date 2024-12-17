using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.Persistence;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceCore.Web.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            //To get user Id 
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                if (HttpContext.Session.GetInt32(AppConstants.SessionCart) == null)
                {
                    HttpContext.Session.SetInt32(AppConstants.SessionCart, _unitOfWork.ShoppingCart
                   .GetAll(u => u.ApplicationUserId == claim.Value).Count());
                }
                return View(HttpContext.Session.GetInt32(AppConstants.SessionCart));

            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
