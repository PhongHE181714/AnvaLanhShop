using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;
using GreenShop.Hubs;

namespace GreenShop.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly GreenShopContext _context;
        private readonly IHubContext<ProductHubs> _hubContext;

        public EditModel(GreenShopContext context, IHubContext<ProductHubs> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [BindProperty]
        public Product Product { get; set; } = new Product();

        [BindProperty]
        public IFormFile? UploadedImage { get; set; }

        // Flat sizes from the form
        [BindProperty]
        public List<ProductSizeInput> ProductSizes { get; set; } = new();

        public List<SelectListItem> StatusList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Text = "Show", Value = "1" },
            new SelectListItem { Text = "Hide", Value = "0" }
        };

        public SelectList CategoryList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null) return NotFound();

            Product = await _context.Products
                .Include(p => p.ProductSizes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Product == null) return NotFound();

            var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
            CategoryList = new SelectList(categories, "Id", "Name", Product.CategoryId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Product.Name))
                return BadRequest(new { message = "Product name is required." });

            bool nameExists = await _context.Products
                .AnyAsync(p => p.Name.ToLower() == Product.Name.Trim().ToLower()
                            && p.Id != Product.Id);
            if (nameExists)
                return BadRequest(new { message = "Product name already exists." });

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

            var productInDb = await _context.Products
                .Include(p => p.ProductSizes)
                .FirstOrDefaultAsync(p => p.Id == Product.Id);

            if (productInDb == null) return NotFound();

            // Update basic fields
            productInDb.Name = Product.Name.Trim();
            productInDb.Description = Product.Description;
            productInDb.CategoryId = Product.CategoryId;
            productInDb.Status = Product.Status;

            // Image upload (optional on edit)
            if (UploadedImage != null)
            {
                var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(UploadedImage.FileName).ToLowerInvariant();

                if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
                    return BadRequest(new { message = "Invalid file extension. Only JPG, JPEG, PNG, and GIF are allowed." });

                if (UploadedImage.Length > 10 * 1024 * 1024)
                    return BadRequest(new { message = "File size exceeds 10MB limit." });

                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Product", fileName);
                var dir = Path.GetDirectoryName(filePath);
                if (dir != null && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await UploadedImage.CopyToAsync(stream);

                // Delete old image
                if (!string.IsNullOrEmpty(productInDb.Image))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Product", productInDb.Image);
                    if (System.IO.File.Exists(oldPath))
                        try { System.IO.File.Delete(oldPath); } catch { }
                }

                productInDb.Image = fileName;
            }

            // Replace all sizes: remove old, add new
            var oldSizes = productInDb.ProductSizes.ToList();
            foreach (var old in oldSizes)
                _context.ProductSizes.Remove(old);

            var newSizes = ProductSizes.Select(s => new ProductSize
            {
                Id = Guid.NewGuid(),
                ProductId = productInDb.Id,
                SizeName = s.SizeName.Trim(),
                Price = s.Price,
                Quantity = s.Quantity
            }).ToList();

            _context.ProductSizes.AddRange(newSizes);

            await _context.SaveChangesAsync();

            try
            {
                await _hubContext.Clients.All.SendAsync("ReceiveProductUpdate", Product.Id, "Updated");
            }
            catch { }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return new OkObjectResult(new { message = "Product updated successfully" });

            return RedirectToPage("./Index");
        }
    }
}
