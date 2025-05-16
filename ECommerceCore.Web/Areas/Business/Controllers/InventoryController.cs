using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Business.Controllers
{
    public class InventoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
