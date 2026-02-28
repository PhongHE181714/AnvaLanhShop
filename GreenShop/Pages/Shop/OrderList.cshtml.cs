using GreenShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;
using System.IdentityModel.Tokens.Jwt;

namespace GreenShop.Pages.Shop
{
    public class OrderListModel : PageModel
    {
        private readonly GreenShop.Models.GreenShopContext _context;
        public OrderListModel(GreenShop.Models.GreenShopContext context)
        {
            _context = context;
        }
        public IList<Order> Order { get; set; } = default!;
        public async Task OnGetAsync()
        {
            Guid? userId = getIdFromToken();
            Order = await _context.Orders
                                  .Include(o => o.User)
                                  .Include(o => o.ShipmentDetails)
                                  .Where(o => o.UserId == userId)
                                  .OrderByDescending(o => o.CreateAt)
                                  .ToListAsync();
            this.itemCart = getCart();
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
