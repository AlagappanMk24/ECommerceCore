using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Customer.Controllers
{
    //[Area("Customer")]
    //[Authorize(Roles = "Customer")]
    //public class NotificationController(INotificationService notificationService) : Controller
    //{
    //    private readonly INotificationService _notificationService = notificationService;

    //    [Authorize(Policy = "Notification.View")]
    //    public async Task<IActionResult> Index()
    //    {
    //        var notifications = await _notificationService.GetNotificationsAsync(User.Identity.Name);
    //        return View(notifications);
    //    }

    //    [Authorize(Policy = "Notification.Edit")]
    //    [HttpPost]
    //    public async Task<IActionResult> MarkAsRead(int id)
    //    {
    //        await _notificationService.MarkAsReadAsync(id, User.Identity.Name);
    //        return RedirectToAction("Index");
    //    }
    //}
}
