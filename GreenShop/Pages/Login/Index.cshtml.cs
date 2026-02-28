using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GreenShop.Models;
using Microsoft.EntityFrameworkCore;
using GreenShop.Service;

namespace GreenShop.Pages.Login
{
	public class LoginPageModel : PageModel
	{
		private readonly GreenShopContext _context;
		private readonly AuthServicecs _authService;

		public LoginPageModel(GreenShopContext context, AuthServicecs authService)
		{
			_context = context;
			_authService = authService;
		}

		public IActionResult OnGet()
		{
			return Page();
		}
		public async Task<IActionResult> OnPostAsync(string loginInput, string password, bool rememberMe)
		{
            if (string.IsNullOrEmpty(loginInput) || string.IsNullOrEmpty(password))
            {
                TempData["ErrorMessage"] = "Please enter both login and password.";
                return Page();
            }

			loginInput = loginInput.Trim();
			password = password.Trim();

            // Search by Email or Username
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginInput || u.Username == loginInput);
			
			if (user == null || BCrypt.Net.BCrypt.Verify(password, user.Password) == false)
			{
				TempData["ErrorMessage"] = "Incorrect email/username or password.";
				return Page();
			}

            if (user.Status == 0)
            {
                TempData["ErrorMessage"] = "Your account is inactive. Please verify your email.";
                return RedirectToPage("/Login/Verify", new { email = user.Email });
            }

			var token = await _authService.CreateToken(user);
			
			var cookieOptions = new CookieOptions();
			if (rememberMe)
			{
				cookieOptions.Expires = DateTime.Now.AddDays(7);
			}
			Response.Cookies.Append("AuthToken", token, cookieOptions);

			return RedirectToPage("/Shop/Index");
		}
	}
}
