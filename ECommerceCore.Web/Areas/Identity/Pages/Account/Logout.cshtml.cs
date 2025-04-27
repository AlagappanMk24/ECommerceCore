using ECommerceCore.Application.Contracts.Services;
using ECommerceCore.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceCore.Web.Areas.Identity.Pages.Account
{
    public class LogoutModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<LogoutModel> logger, IJwtService jwtService) : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<LogoutModel> _logger = logger;
        private readonly IJwtService _jwtService = jwtService;

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            try
            {
                // Get the current user
                var user = await _signInManager.UserManager.GetUserAsync(User);
                if (user != null)
                {
                    // Revoke JWT token
                    await _jwtService.RevokeTokenAsync(user.Id);
                    await _jwtService.CleanupExpiredTokensAsync();
                    _logger.LogInformation("JWT token revoked for user {UserId}", user.Id);

                    // Check if user logged in via external provider
                    var externalLogins = await _userManager.GetLoginsAsync(user);
                    if (externalLogins.Any())
                    {
                        _logger.LogInformation("User {UserId} has external logins: {Providers}", 
                            user.Id, string.Join(", ", externalLogins.Select(l => l.LoginProvider)));
                    }
                }
                else
                {
                    _logger.LogWarning("No user found during logout.");
                }

                // Sign out from ASP.NET Core Identity (clears local session)
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User {UserName} signed out from Identity", user?.UserName ?? "Unknown");

                // Clear all authentication cookies
                await HttpContext.SignOutAsync(); // Clears external provider cookies
                Response.Cookies.Delete("X-Access-Token");

                // Return JSON response for AJAX
                return new JsonResult(new { status = "Success", message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return StatusCode(500, new { status = "Error", message = "An error occurred during logout" });
            }
        }
    }
}