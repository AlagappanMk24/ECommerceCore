using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Customer.Controllers
{
    public class InvoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
