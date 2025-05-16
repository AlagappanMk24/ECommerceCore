using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Operations.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
