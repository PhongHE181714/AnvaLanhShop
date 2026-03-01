using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;

namespace GreenShop.Pages.Cart
{
    public class DeleteModel : PageModel
    {
        private readonly GreenShop.Models.GreenShopContext _context;

        public DeleteModel(GreenShop.Models.GreenShopContext context)
        {
            _context = context;
        }
        public IActionResult OnGet(int changeAction, Guid productId, Guid? sizeId = null)
        {
            Guid? userId = getIdFromToken();
            Models.Cart cart;
            if (userId != null)
            {
                cart = _context.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId && c.ProductSizeId == sizeId);

                _context.Carts.Remove(cart);
                _context.SaveChanges();
                return RedirectToPage("./CartView");
            }
            else
            {

                return RedirectToPage("./CartView", new { alert = "Login First!" });
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
