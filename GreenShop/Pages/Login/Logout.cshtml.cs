using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GreenShop.Pages.Login
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
            TempData.Clear();
            Response.Cookies.Delete("AuthToken");
			Response.Redirect("/Shop/Index");
        }
    }
}
