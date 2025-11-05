using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;

namespace GreenShop.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly GreenShopContext _context;

        public IndexModel(GreenShopContext context)
        {
            _context = context;
        }

        public IList<Category> Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.Categories == null)
                return NotFound();

            // ✅ Lấy toàn bộ danh sách category, sắp xếp theo tên (nếu muốn)
            Category = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            return Page();
        }
    }
}
