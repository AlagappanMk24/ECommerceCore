using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Controllers
{
    public class ContactUsController(IContactUsService contactUsService) : Controller
    {
        private readonly IContactUsService _contactUsService = contactUsService;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // For security, prevent cross-site request forgery
        public async Task<IActionResult> Index(ContactUs contactUs)
        {
            await _contactUsService.SubmitContactForm(contactUs);
            ViewBag.SuccessMessage = "Thank you for contacting us! We will get back to you soon.";
            return View(); // Re-render view with success message
        }
    }
}
