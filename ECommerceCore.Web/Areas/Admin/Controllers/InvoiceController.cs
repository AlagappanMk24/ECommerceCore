using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contracts.Services;
using ECommerceCore.Application.Contracts.ViewModels;
using ECommerceCore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppConstants.Role_Admin)]
    [Route("Admin/Invoice")]
    public class InvoiceController(IInvoiceService invoiceService, ICustomerService customerService, ILogger<InvoiceController> logger) : Controller
    {
        private readonly IInvoiceService _invoiceService = invoiceService;
        private readonly ICustomerService _customerService = customerService;
        private readonly ILogger<InvoiceController> _logger = logger;

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Fetching data for the invoice index page.");

                var customers = await _customerService.GetAllCustomers();


                // Create default query parameters for initial state
                var viewModel = new InvoiceIndexVM
                {
                    QueryParameters = new InvoiceQueryParameters
                    {
                        PageNumber = 1,
                        PageSize = 10,
                        SortColumn = "invoiceNumber",
                        SortDirection = "asc"
                    },
                    Customers = customers
                };

                _logger.LogInformation("Successfully retrieved data for the invoice index page.");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the invoice index page.");
                TempData["Error"] = "Unable to load invoices.";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost("get-invoices")]
        public async Task<IActionResult> GetInvoices([FromBody] InvoiceQueryParameters queryParams)
        {
            try
            {
                var result = await _invoiceService.GetInvoicesPaginatedAsync(queryParams);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching invoices");
                return StatusCode(500, new { error = "Error fetching invoices" });
            }
        }

        [HttpGet("upsert")]
        public IActionResult Upsert()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upsert(InvoiceUpsertVM model)
        {
            if (ModelState.IsValid)
            {
                // Process the invoice and items
                // Handle new customer creation if model.NewCustomer is provided
                // Save to database
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}