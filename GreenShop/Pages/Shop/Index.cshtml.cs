using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;
using System.IdentityModel.Tokens.Jwt;

namespace GreenShop.Pages.Shop
{
    [AllowAnonymous]
    public class HomeModel : PageModel
    {
        private readonly GreenShop.Models.GreenShopContext _context;

        public HomeModel(GreenShop.Models.GreenShopContext context)
        {
            _context = context;
        }
        public IList<Product> Product { get; set; } = default!;
        public IActionResult OnGet()
        {
            Product=_context.Products.ToList();
            Product = Product.Take(8).ToList();
            this.itemCart = getCart();
			return Page();
            
        }
        public int? itemCart { get; set; }
        public int getCart()
        {
            Guid? userId = getIdFromToken();
            if (userId != null)
            {
                List<Models.Cart> cart = new List<Models.Cart>();
                cart = _context.Carts.Where(c => c.UserId == userId).ToList();
                return cart.Count;
            }
            else
            {
                return 0;
            }
           
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
