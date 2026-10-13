using GreenShop.Models;
using GreenShop.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
//using Newtonsoft.Json.Linq;

namespace GreenShop.Pages.Login
{
    public class ForgetPasswordModel : PageModel
    {
        private readonly GreenShopContext _context;
        private readonly AuthServicecs _authService;
        private readonly EmailService _emailService;

        public ForgetPasswordModel(GreenShopContext context, AuthServicecs authService, EmailService emailService)
        {
            _context = context;
            _authService = authService;
            _emailService = emailService;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string RecaptchaResponse { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // Clear previous messages
            ErrorMessage = null;
            SuccessMessage = null;

            if (string.IsNullOrEmpty(Email))
            {
                ErrorMessage = "Please enter your email.";
                return Page();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
            if (user == null)
            {
                // For security reasons, don't reveal that the user doesn't exist
                SuccessMessage = "If an account exists with this email, a reset link has been sent.";
                return Page();
            }

            // Generate reset token using JWT
            var token = _authService.GenerateToken(Email);

            // Create reset link
            var resetLink = Url.Page(
                "/Login/ResetPassword",
                pageHandler: null,
                values: new { token = token },
                protocol: Request.Scheme);

            try
            {
                await _emailService.SendPasswordResetEmailAsync(Email, resetLink);
                SuccessMessage = "If an account exists with this email, a reset link has been sent.";
                ErrorMessage = null;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to send reset email: {ex.Message}. Check the Google App Password and SMTP settings in .env.";
                SuccessMessage = null;
            }

            return Page();
        }
    }
}
