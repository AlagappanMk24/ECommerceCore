using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Business.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
