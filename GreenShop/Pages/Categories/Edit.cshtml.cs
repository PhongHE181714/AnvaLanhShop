using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;
using Microsoft.AspNetCore.SignalR;
using GreenShop.Hubs;

namespace GreenShop.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly GreenShopContext _context;
        private readonly IHubContext<CategoryHubs> _hubContext;

        public EditModel(GreenShopContext context, IHubContext<CategoryHubs> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Categories == null) return NotFound();

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.Id == id);
            if (category == null) return NotFound();

            Category = category;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var existing = await _context.Categories.FindAsync(Category.Id);
            if (existing == null)
                return NotFound();

            var oldStatus = existing.Status;
            existing.Name = Category.Name;
            existing.Status = Category.Status;

            // Lấy danh sách product của Category
            var products = _context.Products.Where(p => p.CategoryId == Category.Id).ToList();

            // Nếu tắt Category → ẩn tất cả product
            if (oldStatus == 1 && Category.Status == 0)
            {
                foreach (var p in products)
                {
                    // Chỉ ẩn những product đang active
                    if (p.Status == 1)
                        p.Status = 0;
                }

                await _context.SaveChangesAsync();

                try
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveProductUpdate",
                        $"{Category.Id}", "hidden_by_category");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ SignalR send product hide failed: {ex.Message}");
                }
            }

            // Nếu bật Category → chỉ hiện những product trước đó đang active
            if (oldStatus == 0 && Category.Status == 1)
            {
                foreach (var p in products)
                {
                    // Chỉ bật lại những product đang 0 vì Category trước đó bị tắt
                    // Nếu product đã tắt từ trước (Status == 0) → giữ nguyên
                    if (p.Status == 0)
                        p.Status = 1;
                }

                await _context.SaveChangesAsync();

                try
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveProductUpdate",
                        $"{Category.Id}", "shown_by_category");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ SignalR send product show failed: {ex.Message}");
                }
            }

            await _context.SaveChangesAsync();

            // SignalR thông báo Category cập nhật
            try
            {
                await _hubContext.Clients.All.SendAsync("ReceiveCategoryUpdate",
                    Category.Id, "updated");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ SignalR send category update failed: {ex.Message}");
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return new OkResult();

            return RedirectToPage("/Categories/Index");
        }
    }
}
