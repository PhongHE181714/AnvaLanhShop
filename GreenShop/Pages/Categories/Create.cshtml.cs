using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using GreenShop.Hubs;
using GreenShop.Models;

namespace GreenShop.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly GreenShopContext _context;
        private readonly IHubContext<CategoryHubs> _hubContext;

        public CreateModel(GreenShopContext context, IHubContext<CategoryHubs> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [BindProperty]
        public Category Category { get; set; } = new Category();

        public IActionResult OnGet() => Page();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (string.IsNullOrWhiteSpace(Category.Name))
            {
                ModelState.AddModelError("Category.Name", "Name is required.");
                return BadRequest(new { message = "Name is required." });
            }

            bool exists = _context.Categories
                .Any(c => c.Name.ToLower() == Category.Name.Trim().ToLower());

            if (exists)
            {
                // ❌ Trùng tên → trả 409 Conflict để JS hiển thị lỗi
                return StatusCode(409, new { message = "Category name already exists." });
            }

            Category.Id = Guid.NewGuid();
            Category.Name = Category.Name.Trim();
            Category.Status = 1;

            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();

            try
            {
                await _hubContext.Clients.All.SendAsync("ReceiveCategoryUpdate", Category.Id, "created");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ SignalR send create failed: {ex.Message}");
            }

            // ✅ Nếu là Ajax request → trả về OK
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return new OkResult();
            }

            // ✅ Nếu là request thường → redirect như cũ
            return RedirectToPage("/Categories/Index");
        }


    }
}
