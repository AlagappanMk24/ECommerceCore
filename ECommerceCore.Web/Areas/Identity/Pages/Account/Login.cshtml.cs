// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ECommerceCore.Infrastructure.External.SMS;
using ECommerceCore.Infrastructure.Services.Email;
using ECommerceCore.Application.Contracts.Services;
using ECommerceCore.Domain.Entities;

namespace ECommerceCore.Web.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmailService _emailService;
        private readonly ISmsSender _smsSender;
        private readonly IJwtService _jwtService;
        private readonly IAuthStateService _authStateService;
        private readonly UserManager<IdentityUser> _userManager;
        public LoginModel(
        SignInManager<IdentityUser> signInManager,
        ILogger<LoginModel> logger,
        IEmailService emailService,
        ISmsSender smsSender,
        IJwtService jwtService,
        IAuthStateService authStateService,
        UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _emailService = emailService;
            _smsSender = smsSender;
            _jwtService = jwtService;
            _authStateService = authStateService;
            _userManager = userManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        // Add these properties to check auth state
        public bool PasswordVerified { get; private set; }
        public bool EmailVerified { get; private set; }
        public string AuthStateId { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            [StringLength(6, MinimumLength = 6)]
            public string EmailOTP { get; set; }

            [StringLength(6, MinimumLength = 6)]
            public string SmsOTP { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
            var authState = await GetOrCreateAuthStateAsync(); // Ensure state is loaded
            PasswordVerified = authState.PasswordVerified;
            EmailVerified = authState.EmailVerified;
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            // Get auth state to determine the current phase
            var authState = await GetOrCreateAuthStateAsync();

            // Clear ModelState entries based on the current phase
            if (authState.PasswordVerified && !authState.EmailVerified)
            {
                // Phase 2: Email OTP - only EmailOTP is relevant
                ModelState.Remove("Input.Email");
                ModelState.Remove("Input.Password");
                ModelState.Remove("Input.SmsOTP");
            }
            else if (authState.PasswordVerified && authState.EmailVerified)
            {
                // Phase 3: SMS OTP - only SmsOTP is relevant
                ModelState.Remove("Input.Email");
                ModelState.Remove("Input.Password");
                ModelState.Remove("Input.EmailOTP");
            }

            if (ModelState.IsValid)
            {
                // PHASE 1: Password verification
                if (!authState.PasswordVerified)
                {
                    // First verify credentials without getting the user
                    var result = await _signInManager.PasswordSignInAsync(
                        Input.Email,
                        Input.Password,
                        isPersistent: false,
                        lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        // Only after successful password verification, get the user
                        var user = await _userManager.FindByEmailAsync(Input.Email);
                        if (user == null)
                        {
                            ModelState.AddModelError(string.Empty, "User not found.");
                            return Page();
                        }

                        // Update auth state with UserId and mark password as verified
                        authState.PasswordVerified = true;
                        authState.UserId = user.Id;
                        await _authStateService.UpdateAuthStateAsync(authState);
                        PasswordVerified = true;

                        // Use UserName as the email address
                        string emailAddress = !string.IsNullOrEmpty(user.Email) ? user.Email : user.UserName;

                        // Generate and send email OTP
                        authState.EmailOTP = GenerateOtp();
                        await _authStateService.UpdateAuthStateAsync(authState);
                        await _emailService.SendEmailAsync(
                           emailAddress,
                            "Your Email Verification Code",
                            $"Your OTP code is: {authState.EmailOTP}");

                        // Stay on same page but now show email OTP field
                        return Page();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return Page();
                    }
                }

                // From this point we can safely get the user from authState
                var verifiedUser = await _userManager.FindByIdAsync(authState.UserId);
                if (verifiedUser == null)
                {
                    ModelState.AddModelError(string.Empty, "Session expired. Please start over.");
                    await _authStateService.CleanupExpiredStatesAsync();
                    return Page();
                }

                // PHASE 2: Email OTP verification
                if (authState.PasswordVerified && !authState.EmailVerified)
                {
                    if (Input.EmailOTP == authState.EmailOTP)
                    {
                        authState.EmailVerified = true;
                        await _authStateService.UpdateAuthStateAsync(authState);
                        EmailVerified = true;

                        // Get user phone number
                        verifiedUser.PhoneNumber = "+918825718491";
                        if (!string.IsNullOrEmpty(verifiedUser.PhoneNumber))
                        {
                            // Generate and send SMS OTP
                            authState.SmsOTP = GenerateOtp();
                            await _authStateService.UpdateAuthStateAsync(authState);
                            await _smsSender.SendSmsAsync(
                                verifiedUser.PhoneNumber,
                                $"Your SMS verification code is: {authState.SmsOTP}");

                            // Stay on same page but now show SMS OTP field
                            return Page();
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "No phone number registered.");
                            return Page();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid email OTP.");
                        return Page();
                    }
                }

                // PHASE 3: SMS OTP verification
                else if (authState.PasswordVerified && authState.EmailVerified)
                {
                    if (Input.SmsOTP == authState.SmsOTP)
                    {
                        // Generate JWT token
                        var token = _jwtService.GenerateJwtToken(verifiedUser);
                        await _jwtService.StoreTokenAsync(verifiedUser.Id, token);

                        // Sign in the user
                        await _signInManager.SignInAsync(verifiedUser, Input.RememberMe);

                        // Cleanup auth state
                        await _authStateService.CleanupExpiredStatesAsync();

                        // Return JWT token (could be in a cookie or response body)
                        Response.Cookies.Append("X-Access-Token", token, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTime.UtcNow.AddHours(12)
                        });

                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid SMS OTP.");
                        return Page();
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
        private async Task<AuthState> GetOrCreateAuthStateAsync()
        {
            // Initialize as null
            AuthState authState = null;

            if (Request.Cookies.TryGetValue("AuthStateId", out var authStateId))
            {
                authState = await _authStateService.GetAuthStateAsync(authStateId);
            }

            if (authState == null)
            {
                authState = await _authStateService.CreateAuthStateAsync(null);
                Response.Cookies.Append("AuthStateId", authState.Id, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(15)
                });
            }
            // Sync properties with auth state
            PasswordVerified = authState.PasswordVerified;
            EmailVerified = authState.EmailVerified;

            return authState;
        }
        private string GenerateOtp(int length = 6)
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
