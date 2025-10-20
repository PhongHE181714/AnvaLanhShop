using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;

namespace GreenShop.Pages.Accounts
{
    public class DeleteModel : PageModel
    {
        private readonly GreenShopContext _context;

        public DeleteModel(GreenShopContext context)
        {
            _context = context;
        }

        [BindProperty]
      public GreenShop.Models.User User { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                User = user;
                _context.Users.Remove(User);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        
    }
}
