using GreenShop.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace GreenShop.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly GreenShopContext _context;
        public IndexModel(GreenShopContext context) => _context = context;

        // FILTERS
        public string? searchString { get; set; }
        public Guid? categoryId { get; set; }
        public string? sortOrder { get; set; }

        // DATA
        public IList<Product> Product { get; set; } = default!;
        public IList<Category> Categories { get; set; } = default!;

        // PAGINATION
        public const int pageSize = 12;
        public int totalProducts { get; set; }
        public int pageLength => (int)Math.Ceiling(totalProducts / (double)pageSize);

        // Lấy current page từ query string, mặc định = 1
        public int currentPage
        {
            get
            {
                if (int.TryParse(Request.Query["page"], out var p) && p > 0)
                    return p;
                return 1;
            }
        }

        // URL HELPER – giữ filter khi phân trang
        public string PaginateUrl(int p)
        {
            var parts = new List<string> { $"page={p}" };

            if (!string.IsNullOrEmpty(searchString))
                parts.Add($"searchString={HttpUtility.UrlEncode(searchString)}");

            if (categoryId.HasValue && categoryId.Value != Guid.Empty)
                parts.Add($"categoryId={categoryId.Value}");

            if (!string.IsNullOrEmpty(sortOrder) && sortOrder != "name_asc")
                parts.Add($"sortOrder={sortOrder}");

            return $"?{string.Join("&", parts)}";
        }


        public async Task OnGetAsync()
        {

            // Lấy filter từ query string
            searchString = Request.Query["searchString"];
            sortOrder = Request.Query["sortOrder"];

            if (Guid.TryParse(Request.Query["categoryId"], out var catId) && catId != Guid.Empty)
                categoryId = catId;

            // Load danh mục cho dropdown
            Categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            // Query cơ bản
            IQueryable<Product> query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            // Tìm kiếm
            if (!string.IsNullOrWhiteSpace(searchString))
                query = query.Where(p => p.Name.Contains(searchString));

            // Lọc danh mục
            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            // Sắp xếp
            query = sortOrder switch
            {
                "name_desc" => query.OrderByDescending(p => p.Name),
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };

            // Tổng sản phẩm
            totalProducts = await query.CountAsync();

            // Phân trang
            Product = await query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
