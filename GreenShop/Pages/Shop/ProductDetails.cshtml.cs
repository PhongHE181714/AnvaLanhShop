using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;
using System.IdentityModel.Tokens.Jwt;

namespace GreenShop.Pages.Shop
{
    public class ProductDetailsModel : PageModel
    {

		public Product Product { get; set; } = default!;
		public List<Product> SimilarProducts = new List<Product>();
		public List<GreenShop.Models.Comment> Comments = new List<GreenShop.Models.Comment>();

        private readonly GreenShopContext _context;
        public ProductDetailsModel(GreenShopContext context)
		{
			_context = context;
		}
		public async Task OnGet(Guid id)
        {
            itemCart = getCart();
			Product = await _context.Products
                .Include(p => p.ProductSizes)
                .FirstOrDefaultAsync(m => m.Id == id);

			SimilarProducts = await _context.Products.Take(4).ToListAsync();

			Comments = await _context.Comments.Include(c=>c.User).Where(c => c.ProductId == id).ToListAsync();
		}
        public int? itemCart { get; set; }
        public int getCart()
        {
            Guid? userId = getIdFromToken();
            if (userId != null)
            {
                List<GreenShop.Models.Cart> cart = new List<GreenShop.Models.Cart>();
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
