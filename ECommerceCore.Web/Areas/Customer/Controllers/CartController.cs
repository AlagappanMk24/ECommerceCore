using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
