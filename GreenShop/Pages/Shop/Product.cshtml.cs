using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;
using System.IdentityModel.Tokens.Jwt;

namespace GreenShop.Pages.Shop
{
    public class ProductModel : PageModel
    {
        private readonly GreenShop.Models.GreenShopContext _context;

        public ProductModel(GreenShop.Models.GreenShopContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get; set; } = default!;
        public IList<Product> ProductB { get; set; } = default!;
        public IList<Product> ProductT { get; set; } = default!;
		public IList<Product> ProductC { get; set; } = default!;


		public int? priceLow { get; set; }

        public string CategoryId { get; set; }
        public string Search {  get; set; }
        public int? priceHigh { get; set; }
        public int? Sort { get; set; }
        public List<GreenShop.Models.Category> categories { get; set; }
        public async Task OnGetAsync(string Category,int? sort,string search)
        {

             this.Sort = sort;
             var products =  _context.Products.AsQueryable();
            
            if (_context.Categories != null)
            {
                categories = await _context.Categories.ToListAsync();
            }
            if (Category != null) {
                products = products.Where(x => x.CategoryId.ToString() == Category);
            }
            if (search != null)
            {
                this.Search = search;
                products = products.Where(u => u.Name.ToLower().Contains(search.ToLower()));

            }
            if (priceHigh != null && priceLow != null)
            {
                products = products.Where(u => u.Price >= priceLow && u.Price <= priceHigh);


            }

            if (Sort != null)
            {
                //newest
                if (Sort == 1)
                {

                }
                //price Low
                else if (Sort == 2)
                {
                    products = products.OrderBy(u => u.Price);
                }
                //price High
                else if (Sort == 3)
                {
                    products = products.OrderByDescending(u => u.Price);
                }
                //rating
                else if (Sort == 4)
                {
                    products = products.OrderByDescending(u => u.Rate);
                }
            }
            Product=products.ToList();
            Guid tam = new Guid("D8F15903-EFA0-423A-A9F8-856909C4E27B");
            ProductT = products.Where(x => x.CategoryId == tam).ToList();
			Guid com = new Guid("D8F15903-EFA0-423A-A9F8-856909C4E27C");
			ProductC = products.Where(x => x.CategoryId == com).ToList();
			/* ViewBag.toDate = toDate;
             ViewBag.fromDate = fromDate;*/
			this.itemCart = getCart();
            this.CategoryId = Category;
            
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
