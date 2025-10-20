using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using GreenShop.Models;
using GreenShop.Service;
using System.Security.Claims;

namespace GreenShop.Pages.Login
{
	public class LoginWithGoogleModel : PageModel
	{
		private readonly GreenShop.Models.GreenShopContext _context;
		private readonly AuthServicecs _authService;
		public LoginWithGoogleModel(GreenShop.Models.GreenShopContext context, AuthServicecs authService)
		{
			_authService = authService;
			_context = context;
		}
		public async Task<IActionResult> OnGetAsync()
		{
			var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			if (!result.Succeeded)
			{
				return RedirectToPage("/Login");
			}
			var claims = result.Principal.Claims.ToList();
			// Lấy các giá trị cụ thể từ các claim theo loại
			var googleId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
			var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
			LoginGoogleModel googleModel = new LoginGoogleModel
			{
				GoogleId = googleId,
				Email = email,
				Username = name,
			};
			Models.User u = await _context.Users.FirstOrDefaultAsync(c => c.Email == email && c.GoogleId == googleId);
			if (u != null)
			{
				// Create token
				var token = await _authService.CreateToken(u);
				Response.Cookies.Append("AuthToken", token);
				return RedirectToPage("/Shop/Index");
			}
			else
			{
				//Create new user
			var user = await _authService.CreateNewUser(googleModel);
				var token = await _authService.CreateToken(user);
				Response.Cookies.Append("AuthToken", token);
				return RedirectToPage("/Shop/Index");
			}
		}
	
	}
}
