using ECommerceCore.Application.Common.Results;
using ECommerceCore.Application.Constants;
using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Application.Contracts.DTOs;
using ECommerceCore.Application.Contracts.ViewModels.Users;
using ECommerceCore.Domain.Entities;
using ECommerceCore.Infrastructure.Services;
using ECommerceCore.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ECommerceCore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = AppConstants.Role_Admin)]
    [Route("Admin/User")]
    public class UserController(IUserService userService, ILogger<UserController> logger, RoleManager<IdentityRole> roleManager, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) : Controller
    {
        private readonly IUserService _userService = userService;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly ILogger<UserController> _logger = logger;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        // VIEW LIST OF USERS
        [HttpGet("")]
        [Authorize(Roles = AppConstants.Role_Admin + "," + AppConstants.Role_Manager)]
        public async Task<IActionResult> Index()
        {
            try
            {
                _logger.LogInformation("Fetching data for the user management index page.");

                var companies = await _userService.GetAllCompaniesAsync();

                var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

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
                    Companies = companies.ToList(),
                    Roles = roles
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

        // FETCH PAGINATED USERS (AJAX or API call)
        [HttpPost("get-users")]
        [Authorize(Roles = AppConstants.Role_Admin + "," + AppConstants.Role_Manager)]
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

        // SHOW USER CREATE OR EDIT FORM
        [HttpGet("upsert/{id?}")]
        [Authorize(Roles = AppConstants.Role_Admin)]
        public async Task<IActionResult> Upsert(string? id)
        {
            try
            {
                UserDto userDto;

                if (string.IsNullOrEmpty(id))
                {
                    // Create mode
                    _logger.LogInformation("Loading create user form");
                    userDto = new UserDto(); // Empty DTO for create mode
                }
                else
                {
                    // Edit mode
                    _logger.LogInformation("Loading edit user form for user ID: {UserId}", id);
                    userDto = await _userService.GetUserByIdAsync(id);

                    if (userDto == null)
                    {
                        _logger.LogWarning("User with ID {UserId} not found", id);
                        TempData["Error"] = "User not found.";
                        return RedirectToAction(nameof(Index));
                    }
                }

                // Use UserService to create view model
                var viewModel = await _userService.CreateUserUpsertVMAsync(userDto);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading user upsert form");
                TempData["Error"] = "Unable to load user form.";
                return RedirectToAction(nameof(Index));
            }
        }

        // HANDLE CREATE OR UPDATE USER POST
        [HttpPost("upsert")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppConstants.Role_Admin)]
        public async Task<IActionResult> Upsert(UserDto userDto, IFormFile? file)
        {
            string newImagePath = null; // Track new image path for cleanup
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;

                // Handle profile image
                if (file != null && file.Length > 0)
                {
                    // Get the original extension
                    string originalExtension = Path.GetExtension(file.FileName).ToLower();

                    // Normalize all JPEG variants to .jpg
                    string normalizedExtension = originalExtension switch
                    {
                        ".jpeg" or ".jpe" or ".jfif" or ".jif" or ".jfi" or ".avif" => ".jpg",
                        _ => originalExtension
                    };

                    string fileName = Guid.NewGuid().ToString() + normalizedExtension;
                    string userPath = Path.Combine(webRootPath, "images", "users");

                    // Create directory if it doesn't exist
                    if (!Directory.Exists(userPath))
                    {
                        Directory.CreateDirectory(userPath);
                    }

                    // Save new image
                    newImagePath = Path.Combine(userPath, fileName);
                    using (var fileStream = new FileStream(Path.Combine(userPath, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    userDto.ProfileImageUrl = $"/images/users/{fileName}";
                }

                var currentUserId = _httpContextAccessor.GetCurrentUserId();
                if (string.IsNullOrEmpty(currentUserId))
                {
                    _logger.LogWarning("Could not determine the current user ID in Upsert action.");
                    TempData["Error"] = "Unable to determine the current user.";
                    if (newImagePath != null && System.IO.File.Exists(newImagePath))
                    {
                        System.IO.File.Delete(newImagePath); // Clean up new image
                    }
                    return View(await _userService.CreateUserUpsertVMAsync(userDto));
                }

                // Store old image path for potential rollback in update mode
                string oldImagePath = string.IsNullOrEmpty(userDto.Id) || string.IsNullOrEmpty(userDto.ProfileImageUrl)
                    ? null
                    : Path.Combine(webRootPath, userDto.ProfileImageUrl.TrimStart('/'));

                // Create or update user
                OperationResult<string> result;

                // Create or update user
                if (string.IsNullOrEmpty(userDto.Id))
                {
                    // Create new user
                    result = await _userService.CreateUserAsync(userDto, currentUserId);  
                }
                else
                {
                    // Update existing user
                    result = await _userService.UpdateUserAsync(userDto, currentUserId);
                }
                if (result.IsSuccess)
                {
                    // Delete old image only after successful update
                    if (oldImagePath != null && System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                    _logger.LogInformation("{Action} user succeeded: {Id}", string.IsNullOrEmpty(userDto.Id) ? "Created" : "Updated", userDto.Id);
                    TempData["Success"] = string.IsNullOrEmpty(userDto.Id) ? "User created successfully." : "User updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Clean up new image on failure
                    if (newImagePath != null && System.IO.File.Exists(newImagePath))
                    {
                        System.IO.File.Delete(newImagePath);
                    }
                    // Handle email-in-use error
                    if (result.ErrorMessage != null && result.ErrorMessage.Contains("Email is already in use"))
                    {
                        ModelState.AddModelError("Email", "This email address is already in use.");
                    }
                    else if (result.Errors.Any(e => e.Code == "DuplicateUserName" || e.Code == "DuplicateEmail"))
                    {
                        ModelState.AddModelError("Email", "This email address is already in use.");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        if (!string.IsNullOrEmpty(result.ErrorMessage) && !result.Errors.Any())
                        {
                            ModelState.AddModelError("", result.ErrorMessage);
                        }
                    }
                    _logger.LogWarning("User upsert failed with error: {Error}", result.ErrorMessage);
                    return View(await _userService.CreateUserUpsertVMAsync(userDto));
                }
            }
            catch (Exception ex)
            {
                // Clean up new image on exception
                if (newImagePath != null && System.IO.File.Exists(newImagePath))
                {
                    System.IO.File.Delete(newImagePath);
                }
                _logger.LogError(ex, "Error occurred during user upsert operation");
                TempData["Error"] = "An error occurred while saving the user.";
                return View(await _userService.CreateUserUpsertVMAsync(userDto));
            }
        }

        // DELETE USER
        [HttpPatch("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found for deletion.");
                    return NotFound(new { error = "User not found" });
                }

                // Delete profile image if it exists
                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    var webRootPath = _webHostEnvironment.WebRootPath;
                    var imagePath = Path.Combine(webRootPath, user.ProfileImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                var currentUserId = _httpContextAccessor.GetCurrentUserId();
                if (string.IsNullOrEmpty(currentUserId))
                {
                    _logger.LogWarning("Could not determine the current user ID in Upsert action.");
                    TempData["Error"] = "Unable to determine the current user.";
                }

                var result = await _userService.DeleteUserAsync(id, currentUserId);
                if (result.IsSuccess)
                {
                    _logger.LogInformation($"User with ID {id} soft-deleted successfully.");
                    return Ok(new { success = true, message = "User deleted successfully" });
                }

                _logger.LogWarning($"Failed to soft-delete user with ID {id}.");
                return BadRequest(new { error = "Failed to delete user" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error soft-deleting user with ID {id}");
                return StatusCode(500, new { error = "An error occurred while soft-deleting the user" });
            }
        }

        [HttpPost("release-email/{userId}")]
        [Authorize(Roles = AppConstants.Role_Admin)]
        public async Task<IActionResult> ReleaseEmail(string userId)
        {
            var result = await _userService.ReleaseEmailAsync(userId);
            if (result.IsSuccess)
            {
                TempData["Success"] = "Email released successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["Error"] = result.ErrorMessage;
            return RedirectToAction(nameof(Index));
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