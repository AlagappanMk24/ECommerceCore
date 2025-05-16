using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Business.Controllers
{
    public class PromotionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
