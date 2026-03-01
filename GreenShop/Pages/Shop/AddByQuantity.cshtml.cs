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
        public IActionResult OnGet(int changeAction, Guid productId, Guid? sizeId = null)
        {
            Guid? userId = getIdFromToken();
            Models.Cart cart;
            if (userId != null)
            {
                // Get total stock for this product across all sizes or specifically for the selected size
                var product = _context.Products
                    .Include(p => p.ProductSizes)
                    .FirstOrDefault(p => p.Id == productId);

                int totalStock;
                if (sizeId != null && product?.ProductSizes != null)
                {
                    totalStock = product.ProductSizes.FirstOrDefault(s => s.Id == sizeId)?.Quantity ?? 0;
                }
                else 
                {
                    totalStock = product?.ProductSizes?.Sum(s => s.Quantity) ?? 0;
                }

                cart = _context.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId && c.ProductSizeId == sizeId);
                if (cart == null)
                {
                    int qty = Math.Min(changeAction, totalStock);
                    if (qty <= 0)
                        return RedirectToPage("./ProductDetails", new { id = productId });

                    cart = new Models.Cart();
                    cart.Id = Guid.NewGuid();
                    cart.ProductId = productId;
                    cart.ProductSizeId = sizeId;
                    cart.UserId = (Guid)userId;
                    cart.Quantity = qty;
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                    this.itemCart = getCart();

                    return RedirectToPage("./ProductDetails", new { id = productId });
                }
                else
                {
                    int newQty = cart.Quantity + changeAction;
                    if (newQty > totalStock) newQty = totalStock;
                    if (newQty < 1) newQty = 1;
                    cart.Quantity = newQty;
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
