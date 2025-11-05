using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GreenShop.Models;

namespace GreenShop.Pages.Orders
{
    public class CreateModel : PageModel
    {
        private readonly GreenShop.Models.GreenShopContext _context;

        public CreateModel(GreenShop.Models.GreenShopContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(Guid? id)
        {

            user = _context.Users.ToList();
            if (id != null && !id.Equals("0"))
            {
                shipmentDetails = _context.ShipmentDetails.Where(x => x.UserId == (Guid)id).ToList();
                userId = id.ToString();
            }
            return Page();
        }
        public string userId;
        public List<GreenShop.Models.User> user;
        public List<GreenShop.Models.ShipmentDetail> shipmentDetails;

        [BindProperty]
        public Order Order { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (_context.Orders == null || Order == null)
            {
                return Page();
            }
            Order.Id = Guid.NewGuid();
            Order.CreateAt = DateTime.Now;
            Order.Status = 0;
            _context.Orders.Add(Order);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}


