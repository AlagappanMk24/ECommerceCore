using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
