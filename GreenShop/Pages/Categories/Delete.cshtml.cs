using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GreenShop.Models;

namespace GreenShop.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly GreenShopContext _context;

        public DeleteModel(GreenShopContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null) return NotFound();

            Category = await _context.Categories.FindAsync(id);
            if (Category == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Category == null) return BadRequest();

            var category = await _context.Categories.FindAsync(Category.Id);
            if (category == null) return NotFound();

            // Kiểm tra còn sản phẩm thuộc category
            bool hasProducts = _context.Products.Any(p => p.CategoryId == category.Id);
            if (hasProducts)
            {
                return StatusCode(409, new { message = "Danh mục này vẫn còn sản phẩm, vui lòng xóa hoặc chuyển sản phẩm sang danh mục khác trước khi xóa." });
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            // Nếu request Ajax → trả OK
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return new OkResult();

            return RedirectToPage("./Index");
        }
    }
}
