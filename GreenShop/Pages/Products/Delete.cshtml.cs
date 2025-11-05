using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using GreenShop.Hubs;
using GreenShop.Models;

namespace GreenShop.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly GreenShopContext _context;
        private readonly IHubContext<ProductHubs> _hubContext;

        public DeleteModel(GreenShopContext context, IHubContext<ProductHubs> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Products == null)
                return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            Product = product;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Product == null || _context.Products == null)
                return BadRequest(new { message = "Invalid product." });

            var productInDb = await _context.Products.FindAsync(Product.Id);
            if (productInDb == null)
                return NotFound(new { message = "Product not found." });

            // Soft delete
            productInDb.Status = 0;

            try
            {
                await _context.SaveChangesAsync();

                // Gửi SignalR realtime
                await _hubContext.Clients.All.SendAsync("ReceiveProductUpdate", Product.Id, "deleted");

                // Nếu là Ajax request
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return new OkResult();

                TempData["SuccessMessage"] = "Product deleted (soft delete) successfully!";
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Failed to delete product: {ex.Message}" });
            }

            return RedirectToPage("./Index");
        }
    }
}
