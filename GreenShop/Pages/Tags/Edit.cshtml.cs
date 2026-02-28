using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GreenShop.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenShop.Pages.Tags
{
    public class EditModel : PageModel
    {
        private readonly GreenShopContext _context;

        public EditModel(GreenShopContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Tag Tag { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Tag = await _context.Tags.FindAsync(id);

            if (Tag == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Tag.Name))
                ModelState.AddModelError("Tag.Name", "Tag name is required");

            if (!string.IsNullOrWhiteSpace(Tag.Name))
            {
                bool nameExists = await _context.Tags
                    .AnyAsync(t => t.Name.ToLower() == Tag.Name.Trim().ToLower()
                                && t.Id != Tag.Id);
                if (nameExists)
                    ModelState.AddModelError("Tag.Name", "Tag name already exists");
            }
            if (!ModelState.IsValid)
                return Page();
            var tagInDb = await _context.Tags.FindAsync(Tag.Id);
            if (tagInDb == null)
                return NotFound();
            tagInDb.Name = Tag.Name.Trim();
            tagInDb.Description = Tag.Description;
            tagInDb.Status = Tag.Status;

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

    }
}
