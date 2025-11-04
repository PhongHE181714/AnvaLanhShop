using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Project_PRN221.Pages.Orders
{
    public class changeStatusModel : PageModel
    {
        private readonly Project_PRN221.Models.GreenShopContext _context;

        public changeStatusModel(Project_PRN221.Models.GreenShopContext context)
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

