using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.IdentityModel.Tokens.Jwt;

namespace Project_PRN222.Pages.User
{
    public class ChangePasswordModel : PageModel
    {


        private readonly GreenShop.Models.GreenShopContext _context;
        public ChangePasswordModel(GreenShop.Models.GreenShopContext context)
        {
            _context = context;
        }
        public GreenShop.Models.User User { get; set; }

        public  IActionResult OnGet()
        {
            Guid? userId = getIdFromToken();

            User = _context.Users.FirstOrDefault(u => u.Id == userId);

            return Page();
        }


        [BindProperty]
        public string OldPassword { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (NewPassword != ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu xác nhận không khớp.";
                return Page();
            }

            var userId = getIdFromToken(); 
            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            User = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (User == null)
            {
                return RedirectToPage("/Login");
            }

            if (!BCrypt.Net.BCrypt.Verify(OldPassword, User.Password)) 
            {
                TempData["ErrorMessage"] = "Mật khẩu cũ không đúng.";
                return Page();
            }


            User.Password = BCrypt.Net.BCrypt.HashPassword(NewPassword); 
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Mật khẩu đã được thay đổi thành công!";
            return RedirectToPage("/User/Index");
        }

        public Guid? getIdFromToken()
        {

            var token = Request.Cookies["AuthToken"];

            if (token == null)
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            if (jwtToken == null)
                return null;
            // Assuming the GUID is stored in a claim named "id"
            var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (Guid.TryParse(idClaim, out Guid guidId))
            {
                return guidId;
            }
            else
            {
                return null;
            }
        }
    }
}
