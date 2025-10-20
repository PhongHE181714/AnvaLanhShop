using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;
using GreenShop.Pages.Shop;
using System.IdentityModel.Tokens.Jwt;

namespace Project_PRN221.Pages.Blog
{
    public class IndexModel : PageModel
    {
        private readonly GreenShopContext _context;

        public List<GreenShop.Models.BlogModel> Blogs { get; set; }
        public IEnumerable<News> RecentNews { get; set; }
        public IndexModel(GreenShopContext context)
		{
			_context = context;
            Blogs = new List<GreenShop.Models.BlogModel>();
        }
       
  

        public void OnGet()
        {
			var newsList = _context.News.Include(x => x.NewDetails.OrderBy(x => x.ContentIndex)).ToList();
            foreach (var news in newsList)
            {
                var content = news.NewDetails.FirstOrDefault()?.Content;
                if(content != null)
                {
                    content = content.Length>50? content.Substring(0,49)+"...": content ;
                }
                GreenShop.Models.BlogModel blog = new GreenShop.Models.BlogModel();
                blog.Id = news.Id;
                blog.Title = news.Title;
                blog.Description = content; 
                blog.Img = news.Img;
                blog.CreatedAt = news.CreatedAt;
                Blogs.Add(blog);

            }
            RecentNews = _context.News.OrderByDescending(x => x.CreatedAt).Take(3).ToList();
            this.itemCart = getCart();


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
