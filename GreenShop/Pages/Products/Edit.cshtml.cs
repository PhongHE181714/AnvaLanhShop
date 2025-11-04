using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;

namespace GreenShop.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly GreenShop.Models.GreenShopContext _context;

        public List<SelectListItem> StatusList { get; set; } = new List<SelectListItem>
{
    new SelectListItem { Text = "Show", Value = "1" },
    new SelectListItem { Text = "Hide", Value = "0" }
};

        public EditModel(GreenShop.Models.GreenShopContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        // SelectList để dùng trong Razor <select asp-items="Model.CategoryList" />
        public SelectList CategoryList { get; set; } = default!;

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

            // Load danh sách category (Id là value, Name là text), và chọn giá trị hiện tại
            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            CategoryList = new SelectList(categories, "Id", "Name", Product.CategoryId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            System.Diagnostics.Debug.WriteLine("🟢 OnPostAsync() bắt đầu xử lý...");

            // In toàn bộ dữ liệu Product từ form
            System.Diagnostics.Debug.WriteLine("📋 Dữ liệu Product submit:");
            System.Diagnostics.Debug.WriteLine($"Id: {Product.Id}");
            System.Diagnostics.Debug.WriteLine($"Name: {Product.Name}");
            System.Diagnostics.Debug.WriteLine($"Price: {Product.Price}");
            System.Diagnostics.Debug.WriteLine($"Quantity: {Product.Quantity}");
            System.Diagnostics.Debug.WriteLine($"Image: {Product.Image}");
            System.Diagnostics.Debug.WriteLine($"Rate: {Product.Rate}");
            System.Diagnostics.Debug.WriteLine($"Description: {Product.Description}");
            System.Diagnostics.Debug.WriteLine($"CategoryId: {Product.CategoryId}");
            System.Diagnostics.Debug.WriteLine($"Status: {Product.Status}");

            if (!ModelState.IsValid)
            {
                foreach (var kvp in ModelState)
                {
                    foreach (var error in kvp.Value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Field {kvp.Key} invalid: {error.ErrorMessage}");
                    }
                }

                var categories = await _context.Categories.OrderBy(c => c.Name).ToListAsync();
                CategoryList = new SelectList(categories, "Id", "Name", Product.CategoryId);
                return Page();
            }


            System.Diagnostics.Debug.WriteLine($"🔍 Đang tìm Product với Id = {Product.Id}");
            var productInDb = await _context.Products.FirstOrDefaultAsync(p => p.Id == Product.Id);

            if (productInDb == null)
            {
                System.Diagnostics.Debug.WriteLine("❌ Không tìm thấy sản phẩm trong DB.");
                return NotFound();
            }

            System.Diagnostics.Debug.WriteLine("✏️ Cập nhật dữ liệu...");
            productInDb.Name = Product.Name;
            productInDb.Price = Product.Price;
            productInDb.Quantity = Product.Quantity;
            productInDb.Image = Product.Image;
            productInDb.Rate = Product.Rate;
            productInDb.Description = Product.Description;
            productInDb.CategoryId = Product.CategoryId;
            productInDb.Status = Product.Status;

            try
            {
                await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine("💾 Lưu thành công!");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Lỗi khi lưu: {ex.Message}");
                throw;
            }

            TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
            System.Diagnostics.Debug.WriteLine("✅ Chuyển hướng về Index.");
            return RedirectToPage("./Index");
        }







        private bool ProductExists(Guid id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
