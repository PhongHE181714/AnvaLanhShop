using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GreenShop.Models;

namespace GreenShop.Pages.Orders
{
    public class EditModel : PageModel
    {
        private readonly GreenShop.Models.GreenShopContext _context;
        private readonly GreenShop.Service.EmailService _emailService;

        public EditModel(GreenShop.Models.GreenShopContext context, GreenShop.Service.EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        public string userId;
        public List<GreenShop.Models.User> user;
        public List<GreenShop.Models.ShipmentDetail> shipmentDetails;
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
            
            var existingOrder = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == Order.Id);

            if (existingOrder == null)
            {
                return NotFound();
            }

            bool statusChangedToShipped = existingOrder.Status != 1 && Order.Status == 1;

            existingOrder.Status = Order.Status;
            existingOrder.Note = Order.Note;
            existingOrder.TotalPrice = Order.TotalPrice;

            _context.Orders.Update(existingOrder);

            if (statusChangedToShipped)
            {
                // Deduct stock
                foreach (var detail in existingOrder.OrderDetails)
                {
                    var matchingSize = await _context.ProductSizes.FirstOrDefaultAsync(ps => ps.ProductId == detail.ProductId && ps.Price == detail.Price);
                    if (matchingSize != null)
                    {
                        matchingSize.Quantity -= detail.Quantity;
                        _context.ProductSizes.Update(matchingSize);
                    }
                }

                // Send email
                var user = existingOrder.User;
                if (user != null && !string.IsNullOrEmpty(user.Email))
                {
                    string subject = "AnvaLanhShop - Đơn hàng đã được duyệt và đang đóng gói";
                    string adminMessage = "Chuyển khoản ngân hàng";
                    if (!string.IsNullOrEmpty(existingOrder.Note))
                    {
                        var noteParts = existingOrder.Note.Split(new[] { "Type:" }, StringSplitOptions.None);
                        adminMessage = noteParts.Length > 1 ? noteParts[1].Trim() : existingOrder.Note;
                    }
                    
                    // Build product table HTML
                    string productRowsHtml = "";
                    foreach (var detail in existingOrder.OrderDetails)
                    {
                        var product = detail.Product;
                        string imageUrl = product != null ? product.Image : "";
                        string productName = product != null ? product.Name : "Sản phẩm";
                        var matchingSize = await _context.ProductSizes.FirstOrDefaultAsync(ps => ps.ProductId == detail.ProductId && ps.Price == detail.Price);
                        string sizeName = matchingSize != null && !string.IsNullOrEmpty(matchingSize.SizeName) ? matchingSize.SizeName : "";
                        
                        productRowsHtml += $@"
                            <tr>
                                <td style='padding: 15px; text-align: center; border-bottom: 1px solid #e9ecef;'>
                                    <img src='{imageUrl}' alt='Product Image' style='width: 100px; height: auto; border-radius: 5px;' />
                                </td>
                                <td style='padding: 15px; text-align: left; border-bottom: 1px solid #e9ecef;'>
                                    <strong style='color: #495057;'>{productName}</strong><br/>
                                    <span style='color: #6c757d; font-size: 12px;'>Size: {sizeName}</span>
                                </td>
                                <td style='padding: 15px; text-align: center; border-bottom: 1px solid #e9ecef; color: #495057;'>
                                    {detail.Quantity}
                                </td>
                                <td style='padding: 15px; text-align: center; border-bottom: 1px solid #e9ecef; font-weight: bold; color: #495057;'>
                                    {detail.Price.ToString("N0")} VNĐ
                                </td>
                            </tr>";
                    }

                    string htmlBody = $@"
                        <html>
                            <body style='font-family: Arial, sans-serif; background-color: #f8f9fa; margin: 0; padding: 20px;'>
                                <div style='max-width: 800px; margin: 0 auto; background-color: #ffffff; padding: 30px; border-radius: 8px; box-shadow: 0 4px 6px rgba(0,0,0,0.1);'>
                                    <h2 style='color: #28a745; text-align: center; margin-bottom: 20px;'>Đơn hàng đã được duyệt và đang đóng gói</h2>
                                    <p style='color: #495057;'>Kính gửi <strong>{user.Username}</strong>,</p>
                                    <p style='color: #495057; line-height: 1.6;'>Đơn hàng của bạn (Mã đơn: <strong>{existingOrder.Id}</strong>) đã được xác nhận thanh toán thành công, duyệt và hiện đang trong quá trình đóng gói để chuẩn bị giao đến bạn.</p>
                                    
                                    <div style='background-color: #f1f3f5; padding: 15px; border-radius: 5px; margin: 20px 0;'>
                                        <p style='margin: 0 0 10px 0; color: #495057;'><strong>Thông tin thanh toán:</strong></p>
                                        <p style='margin: 0 0 5px 0; color: #495057;'>Người dùng: {user.Id}</p>
                                        <p style='margin: 0; color: #495057;'>Hình thức thanh toán: {adminMessage}</p>
                                    </div>
                                    
                                    <h3 style='color: #343a40; margin-top: 30px; padding-bottom: 10px; border-bottom: 2px solid #e9ecef;'>Chi tiết đơn hàng</h3>
                                    <table style='width: 100%; border-collapse: collapse; margin-bottom: 30px; background-color: #f8f9fa; border-radius: 5px; overflow: hidden;'>
                                        <thead style='background-color: #e9ecef;'>
                                            <tr>
                                                <th style='padding: 15px; text-align: center; color: #495057;'>SẢN PHẨM</th>
                                                <th style='padding: 15px; text-align: left; color: #495057;'>TÊN SẢN PHẨM</th>
                                                <th style='padding: 15px; text-align: center; color: #495057;'>SỐ LƯỢNG</th>
                                                <th style='padding: 15px; text-align: center; color: #495057;'>GIÁ</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {productRowsHtml}
                                        </tbody>
                                    </table>
                                    
                                    <div style='text-align: right; margin-bottom: 30px;'>
                                        <p style='font-size: 18px; color: #343a40; margin: 0;'><strong>Tổng giá trị hóa đơn: <span style='color: #e53e3e;'>{existingOrder.TotalPrice.ToString("N0")} VNĐ</span></strong></p>
                                    </div>
                                    
                                    <p style='color: #495057; line-height: 1.6;'>Cảm ơn bạn đã tin tưởng và mua sắm tại <strong>An va Lành</strong>. Chúng tôi rất trân trọng sự ủng hộ của bạn và sẽ cố gắng giao hàng đến bạn trong thời gian sớm nhất.</p>
                                    <p style='color: #495057; margin-top: 30px;'>Trân trọng cảm ơn!</p>
                                </div>
                            </body>
                        </html>";

                    try
                    {
                        await _emailService.SendEmailAsync(user.Email, subject, htmlBody);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to send email: " + ex.Message);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private bool OrderExists(Guid id)
        {
          return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
