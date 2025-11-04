using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_PRN221.Models;

namespace Project_PRN221.Pages.Orders
{
    public class EditModel : PageModel
    {
        private readonly Project_PRN221.Models.GreenShopContext _context;

        public EditModel(Project_PRN221.Models.GreenShopContext context)
        {
            _context = context;
        }
        public string userId;
        public List<Project_PRN221.Models.User> user;
        public List<Project_PRN221.Models.ShipmentDetail> shipmentDetails;
        [BindProperty]
        public Order Order { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order =  await _context.Orders.FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            Order = order;
            userId = order.UserId.ToString();
            user = _context.Users.ToList();
            if (id != null && !id.Equals("0"))
            {
                shipmentDetails = _context.ShipmentDetails.Where(x => x.UserId == order.UserId).ToList();
                userId = id.ToString();
            }
            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine(Order.ToString());
            _context.Orders.Update(Order);
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }

        private bool OrderExists(Guid id)
        {
          return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
