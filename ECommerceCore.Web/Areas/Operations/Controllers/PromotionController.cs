using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Operations.Controllers
{
    public class PromotionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
