using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Domain.Entities;
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

        /// <summary>
        /// Handles the submission of the Contact Us form.
        /// </summary>
        /// <param name="contactUs">The ContactUs model containing form data.</param>
        /// <returns>The Contact Us view with a success message.</returns>
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
