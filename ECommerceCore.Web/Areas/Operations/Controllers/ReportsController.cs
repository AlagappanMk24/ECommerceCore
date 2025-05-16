using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Operations.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
