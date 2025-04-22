using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppConstants.Role_Admin)]
    public class CompanyController(ICompanyService companyService, ILogger<CompanyController> logger) : Controller
    {
        private readonly ICompanyService _companyService = companyService;
        private readonly ILogger<CompanyController> _logger = logger;

        /// <summary>
        /// Retrieves and displays a list of all companies.
        /// </summary>
        /// <returns>A view containing a list of companies.</returns>
        public async Task<IActionResult> Index()
        {
            try
            {
                List<Company> objCompanyList = await _companyService.GetAllCompaniesAsync();
                return View(objCompanyList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching companies");
                return View("Error"); // Return an error view or handle as needed
            }
        }

        /// <summary>
        /// Prepares the view for adding or updating a company based on the provided ID.
        /// </summary>
        /// <param name="Id">The ID of the company to be updated; if null or 0, prepares for creating a new company.</param>
        /// <returns>A view for creating or updating a company.</returns>
        public async Task<IActionResult> Upsert(int? Id)
        {
            try
            {
                if (Id == null || Id == 0)
                {
                    // Create
                    return View(new Company());
                }
                else
                {
                    // Update
                    Company company = await _companyService.GetCompanyByIdAsync(Id.Value);
                    if (company == null)
                    {
                        return NotFound();
                    }
                    return View(company);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while accessing company for Upsert");
                return View("Error"); // Return an error view or handle as needed
            }
        }

        /// <summary>
        /// Adds a new company or updates an existing company based on the provided model.
        /// </summary>
        /// <param name="obj">The company object containing the data to be saved.</param>
        /// <returns>Redirects to the Index action upon success; returns an error view if unsuccessful.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Company obj)
        {
            try
            {
                if (obj.Id == 0)
                {
                    await _companyService.AddCompanyAsync(obj);
                    TempData["Success"] = "Company added successfully";
                }
                else
                {
                    await _companyService.UpdateCompanyAsync(obj);
                    TempData["Success"] = "Company updated successfully";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving company");
                return View("Error"); // Return an error view or handle as needed
            }
        }

        /// <summary>
        /// Retrieves all companies and returns them as JSON data for API consumption.
        /// </summary>
        /// <returns>JSON containing the list of companies.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<Company> objCompanyList = await _companyService.GetAllCompaniesAsync();
                return Json(new { data = objCompanyList });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching companies for API");
                return Json(new { success = false, message = "Error occurred while fetching companies" });
            }
        }

        /// <summary>
        /// Deletes a company with the specified ID.
        /// </summary>
        /// <param name="Id">The ID of the company to be deleted.</param>
        /// <returns>A JSON response indicating the success or failure of the deletion.</returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(int? Id)
        {
            try
            {
                if (Id == null)
                {
                    return Json(new { success = false, message = "Company ID cannot be null" });
                }

                var companyToBeDeleted = await _companyService.GetCompanyByIdAsync(Id.Value);
                if (companyToBeDeleted == null)
                {
                    return Json(new { success = false, message = "Error while deleting" });
                }

                await _companyService.DeleteCompanyAsync(companyToBeDeleted);
                return Json(new { success = true, message = "Company deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting company");
                return Json(new { success = false, message = "Error occurred while deleting company" });
            }
        }
    }
}
