using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Customer.Controllers
{
    public class ContactUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
