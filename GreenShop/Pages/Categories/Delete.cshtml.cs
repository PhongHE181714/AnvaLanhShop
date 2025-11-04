using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Hubs;
using Project_PRN221.Models;

namespace Project_PRN221.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly Project_PRN221.Models.GreenShopContext _context;

        private readonly IHubContext<CategoryHubs> _hubContext;

        public DeleteModel(GreenShopContext context, IHubContext<CategoryHubs> hubContext)
        {
            this._context = context;
            this._hubContext = hubContext;
        }

        [BindProperty]
      public Models.Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id);

            if (category != null)
            {
                Category = category;
                _context.Categories.Remove(Category);
                await _context.SaveChangesAsync();
            }
            await _hubContext.Clients.All.SendAsync("ReceiveCategoryUpdate", Category.Id, "Deleted");

            return RedirectToPage("./Index");

        }
    }
}
