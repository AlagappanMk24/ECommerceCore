using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppConstants.Role_Admin)]
    public class DashboardController(IUnitOfWork unitOfWork) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        /// <summary>
        /// Displays the main dashboard view, showing counts of orders, categories, users, and products.
        /// </summary>
        /// <returns>The dashboard view with data counts.</returns>
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