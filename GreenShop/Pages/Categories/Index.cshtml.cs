using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;

namespace Project_PRN221.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly Project_PRN221.Models.GreenShopContext _context;

        public IndexModel(Project_PRN221.Models.GreenShopContext context)
        {
            _context = context;
        }

        public IList<Models.Category> Category { get; set; } = default!;
        public int pageLength { get; set; }
        public int pageSize { get; set; }
        public int? page { get; set; }
        public async Task<IActionResult> OnGetAsync(int? Pag)
        {
            if (_context.Categories != null)
            {
                Category = await _context.Categories.ToListAsync();
            }
            if (Pag == null)
            {
                Pag = 1;
            }
            int pageSize = 3;
            pageLength = (Category.Count - 1) / pageSize + 1;
            Category = Category.Skip((int)((Pag - 1) * pageSize)).Take(pageSize).ToList();
            /* ViewBag.toDate = toDate;
             ViewBag.fromDate = fromDate;*/
            this.page = Pag;
            return Page();
        }
    }
}
