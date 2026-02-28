using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;
using GreenShop.Service;
using System.Security.Claims;

namespace GreenShop.Pages.Login
{
    public class ResetPasswordModel : PageModel
    {
        private readonly GreenShopContext _context;
        private readonly IConfiguration _configuration;
        private readonly AuthServicecs _authService;

        public ResetPasswordModel(GreenShopContext context, IConfiguration configuration, AuthServicecs authService)
        {
            _context = context;
            _configuration = configuration;
            _authService = authService;
        }

        [BindProperty]
        public string Token { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        public bool IsTokenValid { get; set; }
        public string Email { get; set; }

        public IActionResult OnGet(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                ErrorMessage = "Invalid reset link.";
                IsTokenValid = false;
                return Page();
            }

            Token = token;

            // Validate token
            var jwtService = new JwtService(_configuration["JWT:Secret"]!, _configuration["JWT:ValidIssuer"]!);
            var principal = jwtService.GetPrincipal(token);

            if (principal == null)
            {
                ErrorMessage = "Invalid or expired reset link.";
                IsTokenValid = false;
                return Page();
            }

            // Check if token is expired
            if (JwtService.IsTokenExpired(token))
            {
                ErrorMessage = "This reset link has expired. Please request a new one.";
                IsTokenValid = false;
                return Page();
            }

            // Get email from token
            Email = principal.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(Email))
            {
                ErrorMessage = "Invalid reset link.";
                IsTokenValid = false;
                return Page();
            }

            IsTokenValid = true;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Token))
            {
                ErrorMessage = "Invalid reset link.";
                return RedirectToPage("/Login/ResetPassword", new { token = Token });
            }

            // Validate token
            var jwtService = new JwtService(_configuration["JWT:Secret"]!, _configuration["JWT:ValidIssuer"]!);
            var principal = jwtService.GetPrincipal(Token);

            if (principal == null || JwtService.IsTokenExpired(Token))
            {
                ErrorMessage = "Invalid or expired reset link.";
                return RedirectToPage("/Login/ResetPassword", new { token = Token });
            }

            // Get email from token
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                ErrorMessage = "Invalid reset link.";
                return RedirectToPage("/Login/ResetPassword", new { token = Token });
            }

            // Validate passwords
            if (!_authService.ValidatePassword(NewPassword))
            {
                ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.";
                return RedirectToPage("/Login/ResetPassword", new { token = Token });
            }

            if (NewPassword != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return RedirectToPage("/Login/ResetPassword", new { token = Token });
            }

            // Find user and update password
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                ErrorMessage = "User not found.";
                return RedirectToPage("/Login/ResetPassword", new { token = Token });
            }

            // Hash and update password
            user.Password = BCrypt.Net.BCrypt.HashPassword(NewPassword);
            await _context.SaveChangesAsync();

            SuccessMessage = "Password has been reset successfully. You can now log in with your new password.";
            return RedirectToPage("/Login/Index");
        }
    }
}
