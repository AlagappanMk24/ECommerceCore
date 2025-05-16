using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Business.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
