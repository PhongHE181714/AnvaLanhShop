using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Project_PRN221.Hubs;
using Project_PRN221.Models;

namespace Project_PRN221.Pages.Categories
{
    public class CreateModel : PageModel
    {
        private readonly Project_PRN221.Models.GreenShopContext _context;
        private readonly IHubContext<CategoryHubs> _hubContext;

        public CreateModel(GreenShopContext context, IHubContext<CategoryHubs> hubContext)
        {
            this._context = context;
            this._hubContext = hubContext;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.Category Category { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Categories == null || Category == null)
            {
                return Page();
            }
            Category.Id = Guid.NewGuid();
            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveCategoryUpdate", Category.Id, "Created");
            return RedirectToPage("./Index");
        }
    }
}
