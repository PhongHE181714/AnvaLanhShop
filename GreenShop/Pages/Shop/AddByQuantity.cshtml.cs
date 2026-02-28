using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;

namespace GreenShop.Pages.Shop
{
    public class AddByQuantityModel : PageModel
    {
        private readonly GreenShop.Models.GreenShopContext _context;

        public AddByQuantityModel(GreenShop.Models.GreenShopContext context)
        {
            _context = context;
        }
        public IActionResult OnGet(int changeAction, Guid productId)
        {
            Guid? userId = getIdFromToken();
            Models.Cart cart;
            if (userId != null)
            {
                cart = _context.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
                if (cart == null)
                {
                    cart = new Models.Cart();
                    cart.Id = Guid.NewGuid();
                    cart.ProductId = productId;
                    cart.UserId = (Guid)userId;
                    cart.Quantity = changeAction;
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                    this.itemCart = getCart();

                    return RedirectToPage("./ProductDetails", new { id = productId });

                }
                else
                {
                    cart.Quantity += changeAction;
                    _context.Carts.Update(cart);
                    _context.SaveChanges();
                    this.itemCart = getCart();

                    return RedirectToPage("./ProductDetails", new { id = productId });

                }
            }
            else
            {
                this.itemCart = getCart();

                return RedirectToPage("./ProductDetails", new { id = productId, alert = "Login First!" });
            }

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
