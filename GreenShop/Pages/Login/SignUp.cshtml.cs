using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GreenShop.Models;
using GreenShop.Service;

namespace GreenShop.Pages.Login
{
    public class SignUpModel : PageModel
    {
		private readonly GreenShopContext _context;
		private readonly AuthServicecs _authService;
		private readonly EmailService _emailService;

		public SignUpModel(GreenShopContext context, AuthServicecs authService, EmailService emailService)
		{
			_context = context;
			_authService = authService;
			_emailService = emailService;
		}

		[TempData]
		public string ErrorMessage { get; set; }

		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPostAsync(string username, string email)
		{
			if (await _authService.IsEmailTaken(email))
			{
				ErrorMessage = "Email is already registered.";
				return Page();
			}

            if (await _authService.IsUsernameTaken(username))
            {
                ErrorMessage = "Username is already taken. Please choose another one.";
                return Page();
            }

			// Generate verification code
			string verificationCode = new Random().Next(100000, 999999).ToString();

			// Send verification email
			try
			{
				await _emailService.SendVerificationCodeEmailAsync(email, verificationCode);
			}
			catch (Exception ex)
			{
				ErrorMessage = $"Failed to send verification email: {ex.Message}. Make sure you use a Google App Password and correct SMTP settings in appsettings.json.";
				return Page();
			}

			return RedirectToPage("/Login/Verify", new { 
                email = email, 
                username = username, 
                code = verificationCode,
                time = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()
            });
		}
	}
}
