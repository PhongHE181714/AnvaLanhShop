using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Project_PRN221.Pages.User
{
    public class IndexModel : PageModel
    {
        private readonly Project_PRN221.Models.GreenShopContext _context;
        public IndexModel(Project_PRN221.Models.GreenShopContext context)
        {
            _context = context;
        }

        public Project_PRN221.Models.User User { get; set; }

        public IActionResult OnGet()
        {
            Guid? userId = getIdFromToken();

            User = _context.Users.FirstOrDefault(u => u.Id == userId);

            return Page();
        }

        public IActionResult OnPost(string username, string email, string dob)
        {
            Guid? userId = getIdFromToken();

            if (userId == null)
            {
                return RedirectToPage("/Login");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return RedirectToPage("/Login");
            }

            user.Username = username;
            user.Email = email;
            user.Dob = dob;

            _context.SaveChanges();

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
