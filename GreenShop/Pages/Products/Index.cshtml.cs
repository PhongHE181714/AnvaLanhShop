using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;

namespace GreenShop.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly GreenShop.Models.GreenShopContext _context;

        public IndexModel(GreenShop.Models.GreenShopContext context)
        {
            _context = context;
        }

        public string searchString { get; set; }

        public IList<Product> Product { get; set; } = default!;

        public int pageLength { get; set; }

        public int pageSize { get; set; }
        public int? page { get; set; }
        public async Task<IActionResult> OnGetAsync(int? page, int? Pag, string searchString)
        {
            if(searchString==null)
            {
                searchString = "";
            }
            Guid? userId = getIdFromToken();
            //if (userId == null)
            //{
            //    return RedirectToPage("../Shop/Product");
            //}
            //else
            //{
            //    GreenShop.Models.User u = await _context.Users.Include(c => c.Role).FirstOrDefaultAsync(c => c.Id == userId);
            //    if (!u.Role.Name.Equals("Admin"))
            //    {
            //        return RedirectToPage("../Shop/Product");
            //    }
            //}
            this.searchString = searchString;

            var query = _context.Products.AsQueryable();

            if (Pag == null)
            {
                Pag = 1;
            }

            if (searchString != null)

            {
                query = query.Where(x => x.Name.Contains(searchString));
            }
            int pageSize = 3;
            pageLength = (query.ToList().Count - 1) / pageSize + 1;
            Product = query
                .Skip((int)((Pag - 1) * pageSize))
                .Take(pageSize)
                .ToList();
            ;

            /* ViewBag.toDate = toDate;
             ViewBag.fromDate = fromDate;*/
            this.page = Pag;
            return Page();
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
