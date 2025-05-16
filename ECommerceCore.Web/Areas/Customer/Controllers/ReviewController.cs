using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Customer.Controllers
{
    public class ReviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
