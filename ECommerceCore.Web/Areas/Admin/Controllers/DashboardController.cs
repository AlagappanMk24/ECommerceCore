using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppConstants.Role_Admin)]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _unitOfWork.OrderHeaders.GetAllAsync();
            ViewBag.Orders = orders.Count();
            var categories = await _unitOfWork.Categories.GetAllAsync();
            ViewBag.Categories = categories.Count();
            var users = await _unitOfWork.ApplicationUsers.GetAllAsync();
            ViewBag.Users = users.Count();
            var products = await _unitOfWork.Products.GetAllAsync();
            ViewBag.Products = products.Count();

            return View();
        }
    }
}
