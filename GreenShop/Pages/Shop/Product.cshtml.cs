using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;
using System.IdentityModel.Tokens.Jwt;

namespace GreenShop.Pages.Shop
{
    public class ProductModel : PageModel
    {
        private readonly GreenShopContext _context;

        public ProductModel(GreenShopContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get; set; } = default!;
        public IList<Product> ProductB { get; set; } = default!;
        public IList<Product> ProductT { get; set; } = default!;
        public IList<Product> ProductC { get; set; } = default!;
        public int? priceLow { get; set; }
        public string CategoryId { get; set; }
        public string Search { get; set; }
        public int? priceHigh { get; set; }
        public int? Sort { get; set; }

        // ✅ Thêm khởi tạo rỗng để tránh null
        public List<Category> categories { get; set; } = new List<Category>();

        // --- Phân trang ---
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public int? itemCart { get; set; }

        public async Task OnGetAsync(
    [FromQuery] string? Category,
    [FromQuery] int? sort,
    [FromQuery] string? search,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 8,
    [FromQuery] int? priceLow = null,
    [FromQuery] int? priceHigh = null)
        {
            this.Sort = sort;
            this.Search = search;
            this.priceLow = priceLow;
            this.priceHigh = priceHigh;
            this.CategoryId = Category;

            // ✅ Load danh mục sản phẩm
            categories = await _context.Categories.ToListAsync();

            var products = _context.Products.AsQueryable();

            // --- Lọc danh mục ---
            if (!string.IsNullOrEmpty(Category))
            {
                products = products.Where(p => p.CategoryId.ToString() == Category);
            }

            // --- Tìm kiếm ---
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.ToLower().Contains(search.ToLower()));
            }

            // --- Lọc giá ---
            if (priceLow.HasValue && priceHigh.HasValue)
            {
                products = products.Where(p => p.Price >= priceLow && p.Price <= priceHigh);
            }

            // --- Sắp xếp ---
            if (sort.HasValue)
            {
                switch (sort)
                {
                    case 2: // Giá thấp nhất
                        products = products.OrderBy(p => p.Price);
                        break;
                    case 3: // Giá cao nhất
                        products = products.OrderByDescending(p => p.Price);
                        break;
                    case 4: // Đánh giá cao
                        products = products.OrderByDescending(p => p.Rate);
                        break;
                    default:
                        break;
                }
            }

            // --- ✅ Phân trang ---
            var totalCount = await products.CountAsync();
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            CurrentPage = Math.Max(1, Math.Min(page, TotalPages)); // tránh vượt giới hạn

            Product = await products
                .Skip((CurrentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // --- Nhóm sản phẩm đặc biệt ---
            Guid tam = new Guid("D8F15903-EFA0-423A-A9F8-856909C4E27B");
            ProductT = await _context.Products.Where(x => x.CategoryId == tam).ToListAsync();

            Guid com = new Guid("D8F15903-EFA0-423A-A9F8-856909C4E27C");
            ProductC = await _context.Products.Where(x => x.CategoryId == com).ToListAsync();

            // --- Giỏ hàng ---
            this.itemCart = getCart();
        }

        // --- Đếm sản phẩm trong giỏ ---
        public int getCart()
        {
            Guid? userId = getIdFromToken();
            if (userId != null)
            {
                var cart = _context.Carts.Where(c => c.UserId == userId).ToList();
                return cart.Count;
            }
            else
            {
                return 0;
            }
        }

        // --- Lấy ID người dùng từ token ---
        public Guid? getIdFromToken()
        {
            var token = Request.Cookies["AuthToken"];
            if (token == null) return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            if (jwtToken == null) return null;

            var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (Guid.TryParse(idClaim, out Guid guidId))
            {
                return guidId;
            }

            return null;
        }
    }
}
