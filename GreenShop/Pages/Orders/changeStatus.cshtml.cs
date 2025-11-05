using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GreenShop.Pages.Orders
{
    public class changeStatusModel : PageModel
    {
        private readonly GreenShop.Models.GreenShopContext _context;

        public changeStatusModel(GreenShop.Models.GreenShopContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(Guid id, int orderStatus)
        {
            Models.Order order = _context.Orders.FirstOrDefault(o => o.Id == id);
            order.Status= orderStatus;
            if (order != null)
            {
                _context.Orders.Update(order);
                _context.SaveChanges();
            }
            return RedirectToPage("./Index");

        }
    }


}

