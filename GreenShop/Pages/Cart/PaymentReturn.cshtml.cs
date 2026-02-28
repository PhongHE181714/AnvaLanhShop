using Microsoft.AspNetCore.Mvc.RazorPages;
using GreenShop.Models;

namespace GreenShop.Pages.Cart
{
    public class PaymentReturnModel : PageModel
    {
        private readonly GreenShopContext _context;
        public string Message { get; set; } = "";

        public PaymentReturnModel(GreenShopContext context)
        {
            _context = context;
        }

        public void OnGet(string orderId, string resultCode)
        {
            if (string.IsNullOrEmpty(orderId))
            {
                Message = "Không tìm th?y mã ??n hàng.";
                return;
            }

            var order = _context.Orders.FirstOrDefault(o => o.Id.ToString() == orderId);
            if (order == null)
            {
                Message = "??n hàng không t?n t?i.";
                return;
            }

            if (resultCode == "0" && order.Status == 1)
            {
                Message = "Thanh toán thành công! C?m ?n b?n ?ã mua hàng.";
            }
            else if (resultCode == "0")
            {
                Message = "Thanh toán thành công, ?ang ch? xác nh?n t? h? th?ng.";
            }
            else
            {
                Message = "Thanh toán th?t b?i ho?c b? h?y.";
            }
        }
    }
}