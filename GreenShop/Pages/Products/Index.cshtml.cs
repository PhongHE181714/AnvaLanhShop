using GreenShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GreenShop.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly GreenShopContext _context;

        public IndexModel(GreenShopContext context)
        {
            _context = context;
        }

        public string searchString { get; set; }
        public IList<Product> Product { get; set; } = default!;
        public int pageLength { get; set; }     // Tổng số trang
        public int pageSize { get; set; } = 12; // Số sản phẩm mỗi trang
        public int page { get; set; }           // Trang hiện tại
        public int totalProducts { get; set; }  // Tổng sản phẩm thực tế

        public async Task<IActionResult> OnGetAsync(int? page, string searchString)
        {
            this.searchString = searchString ?? string.Empty;

            var query = _context.Products.AsQueryable();

            // Lọc theo tên
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Name.Contains(searchString));
            }

            // Tổng sản phẩm sau khi lọc
            totalProducts = await query.CountAsync();

            // Trang hiện tại
            this.page = page ?? 1;
            if (this.page < 1) this.page = 1;

            // Tính tổng số trang
            pageLength = (int)Math.Ceiling(totalProducts / (double)pageSize);

            // Lấy đúng sản phẩm cho trang hiện tại
            Product = await query
                .OrderBy(p => p.Id)
                .Skip((this.page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Page();
        }
    }
}
