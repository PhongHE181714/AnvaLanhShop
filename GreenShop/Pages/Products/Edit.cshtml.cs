using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;
using GreenShop.Hubs;

namespace GreenShop.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly GreenShopContext _context;
        private readonly IHubContext<ProductHubs> _hubContext;

        public EditModel(GreenShopContext context, IHubContext<ProductHubs> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [BindProperty]
        public Product Product { get; set; } = new Product();

        public List<SelectListItem> StatusList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Text = "Show", Value = "1" },
            new SelectListItem { Text = "Hide", Value = "0" }
        };

        public SelectList CategoryList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null) return NotFound();

            Product = await _context.Products.FindAsync(id);
            if (Product == null) return NotFound();

            var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            CategoryList = new SelectList(categories, "Id", "Name", Product.CategoryId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid input." });

            var productInDb = await _context.Products.FindAsync(Product.Id);
            if (productInDb == null) return NotFound();

            productInDb.Name = Product.Name;
            productInDb.Price = Product.Price;
            productInDb.Quantity = Product.Quantity;
            productInDb.Image = Product.Image;
            productInDb.Description = Product.Description;
            productInDb.CategoryId = Product.CategoryId;
            productInDb.Status = Product.Status;

            await _context.SaveChangesAsync();

            try
            {
                await _hubContext.Clients.All.SendAsync("ReceiveProductUpdate", Product.Id, "Updated");
            }
            catch { }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return new OkResult();

            return RedirectToPage("./Index");
        }
    }
}
