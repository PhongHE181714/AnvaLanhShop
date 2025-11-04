using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;

namespace GreenShop.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly GreenShop.Models.GreenShopContext _context;

        public DeleteModel(GreenShop.Models.GreenShopContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            Product = product;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            // Soft delete: chỉ cập nhật Status = 0
            product.Status = 0;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Sản phẩm đã được ẩn (xóa mềm)!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi ẩn sản phẩm: {ex.Message}";
            }

            return RedirectToPage("./Index");
        }
    }
}
