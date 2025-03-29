using ECommerceCore.Application.Contract.Service;
using ECommerceCore.Domain.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceCore.Web.Controllers
{
    public class UserController(IUserService userService, ILogger<UserController> logger) : Controller
    {
        private readonly IUserService _userService = userService;
        private readonly ILogger<UserController> _logger = logger;

        /// <summary>
        /// Returns the index view.
        /// </summary>
        /// <returns>The index view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Retrieves and displays the user's profile.
        /// </summary>
        /// <returns>The user's profile view, or an error response.</returns>
        public async Task<IActionResult> Profile()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in claims.");
                    return Unauthorized();
                }
                var userProfile = await _userService.GetApplicationUser(userId);
                if (userProfile == null)
                {
                    _logger.LogWarning($"User profile not found for user ID: {userId}");
                }
                return View(userProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user profile.");
                return StatusCode(500, "An error occurred. Please try again later."); // Return 500 Internal Server Error
            }
        }

        /// <summary>
        /// Retrieves the user's profile for editing.
        /// </summary>
        /// <returns>The user's profile edit view, or an error response.</returns>
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in claims.");
                    return Unauthorized();
                }

                var userProfile = await _userService.GetApplicationUser(userId);
                if (userProfile == null)
                {
                    _logger.LogWarning($"User profile not found for user ID: {userId}");
                    return NotFound();
                }
                return View(userProfile);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user profile for editing.");
                return StatusCode(500, "An error occurred. Please try again later.");
            }
        }

        /// <summary>
        /// Updates the user's profile.
        /// </summary>
        /// <param name="model">The updated user profile model.</param>
        /// <returns>A redirect to the profile view, or the edit view with errors.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(ApplicationUser model)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in claims.");
                    return Unauthorized();
                }
                bool isUpdated = await _userService.UpdateUserAsync(userId, model);
                if (isUpdated)
                {
                    _logger.LogInformation($"User with ID: {userId} updated their profile successfully.");
                    return RedirectToAction("Profile"); // Redirect to the profile page
                }

                _logger.LogWarning($"Failed to update profile for user with ID: {userId}.");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user profile.");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again later.");
                return View(model); // Return the view with the error
            }
        }
    }
}
