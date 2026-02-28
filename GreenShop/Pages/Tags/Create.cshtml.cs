using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GreenShop.Models;

namespace GreenShop.Pages.Tags
{
    public class CreateModel : PageModel
    {
        private readonly GreenShopContext _context;

        public CreateModel(GreenShopContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Tag Tag { get; set; } = new Tag();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool exists = _context.Tags
                .Any(t => t.Name.ToLower() == Tag.Name.Trim().ToLower());

            if (exists)
            {
                ModelState.AddModelError("Tag.Name", "Tag name already exists.");
                return Page();
            }

            // Tạo instance mới, không gán Id
            var newTag = new Tag
            {
                Name = Tag.Name.Trim(),
                Description = Tag.Description,
                Status = Tag.Status
            };

            _context.Tags.Add(newTag);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Tags/Index");
        }

    }
}
