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

		public SignUpModel(GreenShopContext context, AuthServicecs authService)
		{
			_context = context;
			_authService = authService;
		}
		public void OnGet()
        {
        }
		public async Task<IActionResult> OnPostAsync(string username,string email,string password)
        {
		GreenShop.Models.User user = await	_authService.CreateNewUser2(email,username,password);
            var token = await _authService.CreateToken(user);
            Response.Cookies.Append("AuthToken", token);
            return RedirectToPage("/Shop/Index");
		}

	}
}
