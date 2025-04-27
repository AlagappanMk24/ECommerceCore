using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Application.Contracts.Services;
using ECommerceCore.Application.Contracts.ViewModels.Customers;
using ECommerceCore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppConstants.Role_Admin)]
    [Route("Admin/Customer")]
    public class CustomerController(ICustomerService customerService, ICompanyService companyService, ILogger<CustomerController> logger) : Controller
    {
        private readonly ICustomerService _customerService = customerService;
        private readonly ICompanyService _companyService = companyService;
        private readonly ILogger<CustomerController> _logger = logger;

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Fetching data for the customer index page.");
                var companies = await _companyService.GetAllCompaniesAsync();

                // Create default query parameters for initial state
                var viewModel = new CustomerIndexVM
                {
                    QueryParameters = new CustomerQueryParameters
                    {
                        PageNumber = 1,
                        PageSize = 10,
                        SortColumn = "name",
                        SortDirection = "asc"
                    },
                    Companies = companies.ToList()
                };

                _logger.LogInformation("Successfully retrieved data for the customer index page.");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the customer index page.");
                TempData["Error"] = "Unable to load customers.";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost("get-customers")]
        public async Task<IActionResult> GetCustomers([FromBody] CustomerQueryParameters queryParams)
        {
            try
            {
                var result = await _customerService.GetCustomersPaginatedAsync(queryParams);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching customers");
                return StatusCode(500, new { error = "Error fetching customers" });
            }
        }

        [HttpGet("upsert")]
        public async Task<IActionResult> Upsert(int? id)
        {
            try
            {
                var companies = await _companyService.GetAllCompaniesAsync();
                CustomerUpsertVM viewModel = new()
                {
                    Companies = companies.ToList(),
                    Customer = new(),
                };

                if (id == null || id == 0)
                {
                    // Create mode
                    return View(viewModel);
                }
                else
                {
                    // Edit mode
                    var customer = await _customerService.GetCustomerById(id.Value);
                    if (customer == null)
                    {
                        TempData["Error"] = "Customer not found.";
                        return RedirectToAction("Index");
                    }

                    viewModel.Customer = customer;
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading customer upsert page.");
                TempData["Error"] = "Error occurred while loading customer data.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost("upsert")]
        public async Task<IActionResult> Upsert(CustomerUpsertVM viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (viewModel.Customer.Id == 0)
                    {
                        // Create new customer
                        await _customerService.AddCustomer(viewModel.Customer);
                        TempData["Success"] = "Customer created successfully.";
                    }
                    else
                    {
                        // Update existing customer
                        await _customerService.UpdateCustomer(viewModel.Customer);
                        TempData["Success"] = "Customer updated successfully.";
                    }
                    return RedirectToAction("Index");
                }

                // If we get here, there was validation error
                viewModel.Companies = await _companyService.GetAllCompaniesAsync();
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving customer data.");
                TempData["Error"] = "Error occurred while saving customer data.";
                viewModel.Companies = await _companyService.GetAllCompaniesAsync();
                return View(viewModel);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _customerService.DeleteCustomer(id);
                if (result)
                {
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Could not delete customer." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer");
                return StatusCode(500, new { success = false, message = "Error deleting customer." });
            }
        }

        [HttpGet("details")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerWithDetails(id);
                if (customer == null)
                {
                    TempData["Error"] = "Customer not found.";
                    return RedirectToAction("Index");
                }
                var customerDetailsVM = new CustomerDetailsVM
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    CompanyId = customer.CompanyId,
                    CompanyName = customer.Company?.Name,
                    Address = new AddressDto
                    {
                        Address1 = customer.Address?.Address1,
                        Address2 = customer.Address?.Address2,
                        City = customer.Address?.City,
                        State = customer.Address?.State,
                        ZipCode = customer.Address?.ZipCode,
                        Country = customer.Address?.Country
                    },
                    OrderCount = customer.Orders?.Count ?? 0,
                    InvoiceCount = customer.Invoices?.Count ?? 0,
                    TotalSpent = (decimal)(customer.Orders?.Sum(o => o.OrderTotal) ?? 0),

                    RecentOrders = customer.Orders?
               .OrderByDescending(o => o.OrderDate)
               .Take(5)
               .Select(o => new OrderSummaryDto
               {
                   Id = o.Id,
                   OrderDate = o.OrderDate,
                   Status = o.OrderStatus.ToString(), // Assuming Status is enum, you can adjust
                   TotalAmount = (decimal)o.OrderTotal
               }).ToList() ?? new List<OrderSummaryDto>(),

                    RecentInvoices = customer.Invoices?
               .OrderByDescending(i => i.IssueDate)
               .Take(5)
               .Select(i => new InvoiceSummaryDto
               {
                   Id = i.Id,
                   InvoiceNumber = i.InvoiceNumber,
                   DueDate = i.PaymentDue,
                   Amount = i.TotalAmount,
                   IsPaid = i.Status == InvoiceStatus.Paid // Assuming you have InvoiceStatus.Paid
               }).ToList() ?? new List<InvoiceSummaryDto>()
                };
                return View(customerDetailsVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading customer details");
                TempData["Error"] = "Error loading customer details.";
                return RedirectToAction("Index");
            }
        }
    }
}
