using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.Persistence;
using ECommerceCore.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppConstants.Role_Admin)]
    public class CompanyController(IUnitOfWork unitOfWork) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        //For to show Product
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return View(objCompanyList);
        }
        //For Upsert
        public IActionResult Upsert(int? Id)
        {

            if (Id == null || Id == 0)
            {
                //create 
                return View(new Company());
            }
            else
            {
                //Update
                Company company = _unitOfWork.Company.Get(u => u.Id == Id);
                return View(company);
            }
        }


        //For to Upsert Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {

                if (obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                    TempData["Success"] = "Product added successfully";
                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                    TempData["Success"] = "Product updated successfully";
                }
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            else
            {
                return View(obj); // return to the form with validation errors
            }

        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }
        [HttpDelete]
        public IActionResult Delete(int? Id)
        {
            var companyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == Id);
            if (companyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Product delete Successful" });

        }

        #endregion
    }
}
