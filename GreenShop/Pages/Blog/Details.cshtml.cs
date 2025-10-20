using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN221.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Project_PRN221.Pages.Blog
{
    public class DetailsModel : PageModel
    {
		private readonly GreenShopContext _context;
		public List<NewDetail> BlogsDetails { get; set; }
		public IEnumerable<News> RecentNews { get; set; }
		public News News { get; set; }	
		public DetailsModel(GreenShopContext context)
		{
			_context = context;
			BlogsDetails = new List<NewDetail>();
		}


		public void OnGet(Guid id)
        {
			BlogsDetails = _context.NewDetails.Where(x => x.NewId == id).ToList();
			RecentNews = _context.News.OrderByDescending(x => x.CreatedAt).Take(3).ToList();
			News = _context.News.Find(id);
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
