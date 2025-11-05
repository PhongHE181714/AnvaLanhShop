using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;
using System.ComponentModel.Design;
using System.IdentityModel.Tokens.Jwt;
namespace GreenShop.Pages.Cart
{
    public class CartViewModel : PageModel
    {
        private readonly GreenShop.Models.GreenShopContext _context;

        public CartViewModel(GreenShop.Models.GreenShopContext context)
        {
            _context = context;
        }
       public IList<Models.Cart> Carts { get; set; }
        public ShipmentDetail shipment { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Street{ get; set; }

        public Models.User user { get; set; }
        public async Task OnGet()
        {
            Guid userId = (Guid)getIdFromToken();
            Carts= await _context.Carts.Include(x=>x.Product).Where(x=>x.UserId==userId).ToListAsync();
            shipment=await _context.ShipmentDetails.Where(x=>x.UserId == userId && x.Status==1).FirstOrDefaultAsync(); 
            
            if(shipment!=null)
            {
                string[] add=shipment.Address.Split(',');
                City = add[0];
                District = add[1];
                Ward = add[2];
                Street=add[3];
            }
            int id = 0;
            this.itemCart = getCart();
            this.totalPrice = getPrice();
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
        public double? totalPrice { get; set; }
        public double getPrice()
        {
            Guid? userId = getIdFromToken();
            if (userId != null)
            {
                List<Models.Cart> cart = new List<Models.Cart>();
                cart = _context.Carts.Include(x=>x.Product).Where(c => c.UserId == userId).ToList();
                double price = 0;
                foreach (var item in cart)
                {
                    price += (item.Quantity * item.Product.Price);
                }
                return price;
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
