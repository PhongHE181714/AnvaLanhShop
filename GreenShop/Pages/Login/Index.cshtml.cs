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
		public async Task<IActionResult> OnPostAsync(string email, string password)
		{

			email = email.Trim();
			password = password.Trim();
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
			
			if (user == null)
			{
				ModelState.AddModelError("LoginFailed", "Email or password is incorrect");
				return Page();
			}
			if (BCrypt.Net.BCrypt.Verify(password, user.Password) == false)
			{
				ModelState.AddModelError("LoginFailed", "Email or password is incorrect");
				return Page();
			}

			var token = await _authService.CreateToken(user);
			Response.Cookies.Append("AuthToken", token);

			return RedirectToPage("/Shop/Index");
		}
	}
}
