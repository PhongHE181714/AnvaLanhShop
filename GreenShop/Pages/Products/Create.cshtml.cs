using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using GreenShop.Hubs;
using Microsoft.AspNetCore.Http;
using System.IO;
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

        [BindProperty]
        public IFormFile? UploadedImage { get; set; }

        // Sizes come in as a flat list via form binding
        [BindProperty]
        public List<ProductSizeInput> ProductSizes { get; set; } = new();

        public List<Category> categories { get; set; } = new List<Category>();

        public IActionResult OnGet()
        {
            categories = _context.Categories.ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Product.Name))
                return BadRequest(new { message = "Name is required." });

            bool exists = _context.Products.Any(p => p.Name.ToLower() == Product.Name.Trim().ToLower());
            if (exists)
                return StatusCode(409, new { message = "Product name already exists." });

            // Validate sizes
            if (ProductSizes == null || ProductSizes.Count == 0)
                return BadRequest(new { message = "At least one product size is required." });

            foreach (var s in ProductSizes)
            {
                if (string.IsNullOrWhiteSpace(s.SizeName))
                    return BadRequest(new { message = "Size name cannot be empty." });
                if (s.Price <= 0)
                    return BadRequest(new { message = $"Price for size '{s.SizeName}' must be greater than 0." });
                if (s.Quantity < 0)
                    return BadRequest(new { message = $"Quantity for size '{s.SizeName}' cannot be negative." });
            }

            // Image upload
            if (UploadedImage == null)
                return BadRequest(new { message = "Product image is required." });

            var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(UploadedImage.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
                return BadRequest(new { message = "Invalid file extension. Only JPG, JPEG, PNG, and GIF are allowed." });

            if (UploadedImage.Length > 10 * 1024 * 1024)
                return BadRequest(new { message = "File size exceeds 10MB limit." });

            var fileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "product", fileName);
            var directory = Path.GetDirectoryName(filePath);
            if (directory != null && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await UploadedImage.CopyToAsync(stream);

            // Build product
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Name = Product.Name.Trim(),
                Description = Product.Description,
                CategoryId = Product.CategoryId,
                Image = fileName,
                Status = 1,
                Rate = 0
            };

            // Build sizes
            var sizes = ProductSizes.Select(s => new ProductSize
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                SizeName = s.SizeName.Trim(),
                Price = s.Price,
                Quantity = s.Quantity
            }).ToList();

            _context.Products.Add(product);
            _context.ProductSizes.AddRange(sizes);
            await _context.SaveChangesAsync();

            try
            {
                await _hubContext.Clients.All.SendAsync("ReceiveProductUpdate", product.Id, "Created");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SignalR send create failed: {ex.Message}");
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return new OkObjectResult(new { message = "Product created successfully" });

            return RedirectToPage("./Index");
        }
    }

    // Flat DTO for size binding from multipart form
    public class ProductSizeInput
    {
        public string SizeName { get; set; } = "";
        public float Price { get; set; }
        public int Quantity { get; set; }
    }
}
