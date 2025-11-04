using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN221.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Project_PRN221.Pages.Comment
{
    public class AddCommentModel : PageModel
    {
        private readonly Project_PRN221.Models.GreenShopContext _context;
		public Product Product { get; set; } = default!;
		public AddCommentModel(Project_PRN221.Models.GreenShopContext context)
        {
            _context = context;
        }
        public IActionResult OnGet(int rate, string comment,Guid productId)
        {
            Guid? userId = getIdFromToken();
            if (userId != null)
            {
                Models.User user = _context.Users.FirstOrDefault(u => u.Id == userId);
                Models.Comment comments = new Models.Comment();
                comments.CommentedAt = DateTime.Now;
                comments.UserId = user.Id;
                comments.Rate = rate;
                comments.Content = comment;
                comments.ProductId=productId;
                comments.Id = Guid.NewGuid();
                _context.Comments.Add(comments);
                _context.SaveChanges();
            }
			return RedirectToPage("../Shop/ProductDetails", new { id = productId });
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
