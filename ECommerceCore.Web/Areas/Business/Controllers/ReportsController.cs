using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Business.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
