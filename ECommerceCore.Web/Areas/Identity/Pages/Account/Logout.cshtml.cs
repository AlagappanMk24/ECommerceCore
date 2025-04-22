using ECommerceCore.Application.Contracts.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceCore.Web.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly IJwtService _jwtService;
        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger, IJwtService jwtService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _jwtService = jwtService;
        }
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            // Get the current user from HttpContext using UserManager
            var user = await _signInManager.UserManager.GetUserAsync(User);
            if (user != null)
            {
                // Invalidate the token server-side if stored in your database
                var userId = user.Id;
                await _jwtService.RevokeTokenAsync(user.Id);
                await _jwtService.CleanupExpiredTokensAsync();
            }
            // Sign out the user
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return Content("Success");
        }
    }
}