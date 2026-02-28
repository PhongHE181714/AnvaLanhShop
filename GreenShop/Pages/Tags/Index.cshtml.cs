using GreenShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GreenShop.Pages.Tags
{
    public class IndexModel : PageModel
    {
        private readonly GreenShopContext _context;

        public IndexModel(GreenShopContext context)
        {
            _context = context;
        }

        public IList<Tag> Tags { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Tags = await _context.Tags.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag == null)
                return NotFound();

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return RedirectToPage(); 
        }

    }
}
