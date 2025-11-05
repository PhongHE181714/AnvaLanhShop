using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using GreenShop.Hubs;
using GreenShop.Models;

namespace GreenShop.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly GreenShopContext _context;
        private readonly IHubContext<ProductHubs> _hubContext;

        public CreateModel(GreenShopContext context, IHubContext<ProductHubs> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [BindProperty]
        public Product Product { get; set; } = new Product();

        public List<Category> categories { get; set; } = new List<Category>();

        public IActionResult OnGet()
        {
            categories = _context.Categories.ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Product.Name))
            {
                ModelState.AddModelError("Product.Name", "Name is required.");
                return BadRequest(new { message = "Name is required." });
            }

            bool exists = _context.Products.Any(p => p.Name.ToLower() == Product.Name.Trim().ToLower());
            if (exists)
                return StatusCode(409, new { message = "Product name already exists." });

            Product.Id = Guid.NewGuid();
            Product.Status = 1;

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            try
            {
                await _hubContext.Clients.All.SendAsync("ReceiveProductUpdate", Product.Id, "Created");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ SignalR send create failed: {ex.Message}");
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return new OkResult();

            return RedirectToPage("./Index");
        }
    }
}
