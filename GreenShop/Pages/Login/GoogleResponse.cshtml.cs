using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;

namespace GreenShop.Pages.Login
{
    public class GoogleResponseModel : PageModel
    {
        public IActionResult OnGet()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Page("LoginWithGoogle", null, null, Request.Scheme)
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        public async Task<IActionResult> OnPostAsync()
        {
            

            return RedirectToPage("");
        }
    }
}
