using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;

namespace Project_PRN221.Pages.Accounts
{
    public class IndexModel : PageModel
    {
        private readonly Project_PRN221.Models.GreenShopContext _context;

        public IndexModel(Project_PRN221.Models.GreenShopContext context)
        {
            _context = context;
        }
        public string searchString { get; set; }

        public int pageLength { get; set; }
        public int pageSize { get; set; }
        public int? page { get; set; }
        public IList<Project_PRN221.Models.User> users { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? Pag,string? searchString)
        {
            Guid? userId = getIdFromToken();
            if (userId == null)
            {
                return RedirectToPage("../Shop/Product");
            }
            else
            {
                Models.User u = await _context.Users.Include(c => c.Role).FirstOrDefaultAsync(c => c.Id == userId);
                if (!u.Role.Name.Equals("Admin"))
                {
                    return RedirectToPage("../Shop/Product");
                }
            }
            this.searchString = searchString;
            if (_context.Users != null)
            {
                users= await _context.Users
                .Include(u => u.Role).ToListAsync();
            }
            // Search
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.Username.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                         u.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            // Filter by Role

            // Filter by Status


            if (Pag == null)
            {
                Pag = 1;
            }
            int pageSize = 3;
            users = users.OrderByDescending(o => o.CreateAt).ToList();
            pageLength = (users.Count - 1) / pageSize + 1;
            users = users.Skip((int)((Pag - 1) * pageSize)).Take(pageSize).ToList();
            page = Pag;

            searchString = searchString;
            return Page();
            // Pass the data to the view
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
