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
                // Check if session cart value is null
                if (HttpContext.Session.GetInt32(AppConstants.SessionCart) == null)
                {
                    // Get the count of shopping cart items asynchronously
                    var cartItemsCount = await _unitOfWork.ShoppingCarts
                        .GetAllAsync(u => u.ApplicationUserId == claim.Value);

                    // Set the session value
                    HttpContext.Session.SetInt32(AppConstants.SessionCart, cartItemsCount.Count());
                }
                // Return the cart count from the session
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
