using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Operations.Controllers
{
    //[Area("Operations")]
    //[Authorize(Roles = "CustomerSupport,Admin,AdminSuper")]
    //public class SupportController : Controller
    //{
    //    private readonly ISupportService _supportService;

    //    public SupportController(ISupportService supportService)
    //    {
    //        _supportService = supportService;
    //    }

    //    [Authorize(Policy = "Customer.View")]
    //    public async Task<IActionResult> Index()
    //    {
    //        var tickets = await _supportService.GetSupportTicketsAsync();
    //        return View(tickets);
    //    }

    //    [Authorize(Policy = "Customer.Edit")]
    //    public IActionResult Respond(int id)
    //    {
    //        return View();
    //    }
    //}
}
