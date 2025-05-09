﻿using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contracts.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceCore.Web.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = AppConstants.Role_Admin)]
    [Route("Admin/User")]
    public class UserController(IUserService userService, ILogger<UserController> logger) : Controller
    {
        private readonly IUserService _userService = userService;
        private readonly ILogger<UserController> _logger = logger;

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Fetching data for the user management index page.");
                var companies = await _userService.GetAllCompanies();
                // Create default query parameters for initial state
                var viewModel = new UserIndexVM
                {
                    QueryParameters = new UserQueryParameters
                    {
                        PageNumber = 1,
                        PageSize = 10,
                        SortColumn = "name",
                        SortDirection = "asc"
                    },
                    Companies = companies.ToList()
                };
                _logger.LogInformation("Successfully retrieved data for the user management index page.");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading the user management index page.");
                TempData["Error"] = "Unable to load users.";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost("get-users")]
        public async Task<IActionResult> GetUsers([FromBody] UserQueryParameters queryParams)
        {
            try
            {
                var result = await _userService.GetUsersPaginatedAsync(queryParams);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users");
                return StatusCode(500, new { error = "Error fetching users" });
            }
        }
    }
    //public class UserController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager) : Controller
    //{
    //    private readonly UserManager<ApplicationUser> _userManager = userManager;
    //    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    //    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    //    /// <summary>
    //    /// Displays the user management index view.
    //    /// </summary>
    //    /// <returns>The index view.</returns>
    //    public IActionResult Index()
    //    {
    //        return View();
    //    }

    //    /// <summary>
    //    /// Manages the roles assigned to a specific user.
    //    /// Retrieves the user's current roles and available roles for selection.
    //    /// </summary>
    //    /// <param name="userId">The ID of the user to manage roles for.</param>
    //    /// <returns>A view model for role management.</returns>
    //    [HttpGet]
    //    [Route("Admin/User/RoleManagment")]
    //    public async Task<IActionResult> RoleManagement(string userId)
    //    {
    //        var applicationUser = await _unitOfWork.ApplicationUsers.GetAsync(u => u.Id == userId, includeProperties: "Company");
    //        var roles = await _userManager.GetRolesAsync(applicationUser);

    //        RoleManagementVM roleVM = new RoleManagementVM()
    //        {
    //            ApplicationUser = applicationUser,
    //            RoleList = _roleManager.Roles.Select(i => new SelectListItem
    //            {
    //                Text = i.Name,
    //                Value = i.Name
    //            }).ToList(),
    //            CompanyList = (await _unitOfWork.Companies.GetAllAsync()).Select(i => new SelectListItem
    //            {
    //                Text = i.Name,
    //                Value = i.Id.ToString()
    //            }).ToList(),
    //        };

    //        roleVM.ApplicationUser.Role = roles.FirstOrDefault();
    //        return View(roleVM);
    //    }

    //    /// <summary>
    //    /// Updates the role of a user based on the submitted role management view model.
    //    /// This method also handles the association with a company if the user role changes.
    //    /// </summary>
    //    /// <param name="roleManagementVM">The view model containing role management data.</param>
    //    /// <returns>A redirect to the index action.</returns>
    //    [HttpPost]
    //    [Route("Admin/User/RoleManagment")]
    //    public async Task<IActionResult> RoleManagement(RoleManagementVM roleManagementVM)
    //    {
    //        var applicationUser = await _unitOfWork.ApplicationUsers.GetAsync(u => u.Id == roleManagementVM.ApplicationUser.Id);
    //        var oldRole = (await _userManager.GetRolesAsync(applicationUser)).FirstOrDefault();

    //        if (!(roleManagementVM.ApplicationUser.Role == oldRole))
    //        {
    //            //a role was updated
    //            if (roleManagementVM.ApplicationUser.Role == AppConstants.Role_Company)
    //            {
    //                applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
    //            }
    //            if (oldRole == AppConstants.Role_Company)
    //            {
    //                applicationUser.CompanyId = null;
    //            }

    //            _unitOfWork.ApplicationUsers.Update(applicationUser);
    //            await _unitOfWork.SaveAsync();

    //            // Remove old role and add new role
    //            await _userManager.RemoveFromRoleAsync(applicationUser, oldRole);
    //            await _userManager.AddToRoleAsync(applicationUser, roleManagementVM.ApplicationUser.Role);

    //        }
    //        else
    //        {
    //            if (oldRole == AppConstants.Role_Company && applicationUser.CompanyId != roleManagementVM.ApplicationUser.CompanyId)
    //            {
    //                applicationUser.CompanyId = roleManagementVM.ApplicationUser.CompanyId;
    //                _unitOfWork.ApplicationUsers.Update(applicationUser);
    //                await _unitOfWork.SaveAsync();
    //            }
    //        }

    //        return RedirectToAction("Index");
    //    }

    //    /// <summary>
    //    /// Retrieves all users along with their roles and associated companies.
    //    /// </summary>
    //    /// <returns>A JSON object containing a list of users.</returns>
    //    [HttpGet]
    //    public async Task<IActionResult> GetAll()
    //    {
    //        var objUserList = await _unitOfWork.ApplicationUsers.GetAllAsync(includeProperties: "Company");

    //        foreach (var user in objUserList)
    //        {
    //            user.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "No Role"; // Default role if none assigned

    //            if (user.Company == null)
    //            {
    //                user.Company = new Company()
    //                {
    //                    Name = ""
    //                };
    //            }
    //        }

    //        return Json(new { data = objUserList });
    //    }

    //    /// <summary>
    //    /// Locks or unlocks a user based on the current lockout status.
    //    /// </summary>
    //    /// <param name="id">The ID of the user to lock or unlock.</param>
    //    /// <returns>A JSON response indicating the success of the operation.</returns>
    //    [HttpPost]
    //    public async Task<IActionResult> LockUnlock([FromBody] string id)
    //    {
    //        var objFromDb = await _unitOfWork.ApplicationUsers.GetAsync(u => u.Id == id);
    //        if (objFromDb == null)
    //        {
    //            return Json(new { success = false, message = "Error while Locking/Unlocking" });
    //        }

    //        if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
    //        {
    //            // User is currently locked and we need to unlock them
    //            objFromDb.LockoutEnd = DateTime.Now;
    //        }
    //        else
    //        {
    //            objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
    //        }

    //        _unitOfWork.ApplicationUsers.Update(objFromDb);
    //        await _unitOfWork.SaveAsync();
    //        return Json(new { success = true, message = "Operation Successful" });
    //    }
    //}
}
