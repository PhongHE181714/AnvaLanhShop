using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Project_PRN221.Models;
using static NuGet.Packaging.PackagingConstants;

namespace Project_PRN221.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly Project_PRN221.Models.GreenShopContext _context;

        public IndexModel(Project_PRN221.Models.GreenShopContext context)
        {
            _context = context;
        }

        public int pageLength { get; set; }
        
        public int pageSize { get; set; }
        
        public int? page { get; set; }


        [BindProperty(SupportsGet = true)]
        public int? StatusOrder { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? DateFrom { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? DateTo { get; set; }

        public IList<Order> Order { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync(int Pag)
        {
            Guid? userId = getIdFromToken();
            if (userId == null)
            {
                return RedirectToPage("../Shop/Product");
            }
            else
            {
                Models.User u= await _context.Users.Include(c=>c.Role).FirstOrDefaultAsync(c=>c.Id==userId);
                if(!u.Role.Name.Equals("Admin"))
                {
                    return RedirectToPage("../Shop/Product");
                }
            }
            if (_context.Orders != null)
            {
                Order = BuildQuery().ToList();
            }
            if (Pag == null)
            {
                Pag = 1;
            }
            int pageSize = 9;
            Order = Order.OrderByDescending(o => o.CreateAt).ToList();
            pageLength = (Order.Count - 1) / pageSize + 1;
            Order = Order.Skip((int)((Pag - 1) * pageSize)).Take(pageSize).ToList();

            /*ViewBag.toDate = toDate;
            ViewBag.fromDate = fromDate;*/
            this.page = Pag;
            return Page();
        }
        public Guid? getIdFromToken()
        {

            var token = Request.Cookies["AuthToken"];

            if (token == null)
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            if (jwtToken == null)
                return null;
            // Assuming the GUID is stored in a claim named "id"
            var idClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (Guid.TryParse(idClaim, out Guid guidId))
            {
                return guidId;
            }
            else
            {
                return null;
            }
        }
        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (_context.Orders != null)
            {
                Order order = await _context.Orders.FirstOrDefaultAsync(x=>x.Id==id);
                if (order != null)
                {
                    if(order.Status==1) order.Status=0;
                    else order.Status=1;
                }
            }
            return RedirectToPage("./Index");
        }
        private IQueryable<Order> BuildQuery()
        {
            var query =  _context.Orders
                .Include(o => o.ShipmentDetails)
                .Include(o => o.User).OrderByDescending(x => x.CreateAt).AsQueryable();

           if(StatusOrder.HasValue)
            {
                query = query.Where(s => s.Status == StatusOrder.Value);
            }

            if (DateFrom.HasValue)
            {
                query = query.Where(s => s.CreateAt >= DateFrom.Value);
            }

            if (DateTo.HasValue)
            {
                query = query.Where(s => s.CreateAt <= DateTo.Value);
            }

            return query;
        }

        public async Task<IActionResult> OnPostExportToExcel()
        {
            // Tạo file Excel từ danh sách Orders
            byte[] excelFile = await ConvertListToExel();

            // Đặt tên cho file
            string fileName = "OrdersExport.xlsx";

            // Gửi file về cho client
            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        public async Task<IActionResult> OnPostExportToExcel2()
        {
            // Tạo file Excel từ danh sách Orders
            byte[] excelFile = await ConvertListToExel2();

            // Đặt tên cho file
            string fileName = "ProductsExport.xlsx";

            // Gửi file về cho client
            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }


        private async Task<byte[]> ConvertListToExel()
        {
            // Đảm bảo giấy phép EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Tạo package Excel
            using (var package = new ExcelPackage())
            {
                // Tạo worksheet mới
                var worksheet = package.Workbook.Worksheets.Add("Orders");

                // Thiết lập header với style đặc biệt
                string[] headers = { "Order ID", "Receiver", "Address", "Phone", "Created Date", "Total Price", "Status" };

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true; // In đậm chữ
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid; // Đặt nền
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue); // Màu nền
                    worksheet.Cells[1, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); // Viền xung quanh
                    worksheet.Cells[1, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; // Căn giữa
                }

                // Định dạng cột để kích thước tự động
                worksheet.Cells[1, 1, 1, headers.Length].AutoFitColumns();

                // Lấy danh sách đơn hàng
                IList<Order> Orders = BuildQuery().ToList();

                // Điền dữ liệu vào các ô
                for (int i = 0; i < Orders.Count(); i++)
                {
                    worksheet.Cells[i + 2, 1].Value = Orders[i].Id;
                    worksheet.Cells[i + 2, 2].Value = Orders[i].ShipmentDetails.Receiver;
                    worksheet.Cells[i + 2, 3].Value = Orders[i].ShipmentDetails.Address;
                    worksheet.Cells[i + 2, 4].Value = Orders[i].ShipmentDetails.PhoneNumber;
                    worksheet.Cells[i + 2, 5].Value = Orders[i].CreateAt.ToString("yyyy-MM-dd");
                    worksheet.Cells[i + 2, 5].Style.Numberformat.Format = "yyyy-MM-dd";
                    worksheet.Cells[i + 2, 6].Value = Orders[i].TotalPrice;

                    string statusOption = Orders[i].Status switch
                    {
                        0 => "Process",
                        1 => "Shipped",
                        2 => "In Route",
                        3 => "Complete",
                        _ => "Unknown"
                    };

                    worksheet.Cells[i + 2, 7].Value = statusOption;

                    // Thiết lập border cho từng ô trong hàng này
                    for (int j = 1; j <= headers.Length; j++)
                    {
                        worksheet.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); // Viền mỏng
                    }

                    // Căn lề cho các ô chứa text
                    worksheet.Cells[i + 2, 2, i + 2, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left; // Căn lề trái
                    worksheet.Cells[i + 2, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; // Ngày tháng căn giữa
                    worksheet.Cells[i + 2, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right; // Tổng giá căn phải
                }

                // Định dạng cột để kích thước tự động
                worksheet.Cells[2, 1, Orders.Count() + 1, headers.Length].AutoFitColumns();

                // Lưu dữ liệu vào MemoryStream
                var memoryStream = new MemoryStream();
                await package.SaveAsAsync(memoryStream);

                // Trả về dữ liệu Excel dưới dạng mảng byte
                return memoryStream.ToArray();
            }
        }

        private async Task<byte[]> ConvertListToExel2()
        {
            // Đảm bảo giấy phép EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Tạo package Excel
            using (var package = new ExcelPackage())
            {
                // Tạo worksheet mới
                var worksheet = package.Workbook.Worksheets.Add("Orders");

                // Thiết lập header với style đặc biệt
                string[] headers = { "Order ID", "Product", "Quantity", "Price", "Total Price" };

                for (int k = 0; k < headers.Length; k++)
                {
                    worksheet.Cells[1, k + 1].Value = headers[k];
                    worksheet.Cells[1, k + 1].Style.Font.Bold = true; // In đậm chữ
                    worksheet.Cells[1, k + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid; // Đặt nền
                    worksheet.Cells[1, k + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue); // Màu nền
                    worksheet.Cells[1, k + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); // Viền xung quanh
                    worksheet.Cells[1, k + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center; // Căn giữa
                }

                // Định dạng cột để kích thước tự động
                worksheet.Cells[1, 1, 1, headers.Length].AutoFitColumns();



                int i = 0;
               
                    List<OrderDetail> orderDetails = new List<OrderDetail>();
                    orderDetails = await _context.OrderDetails
                        .Include(x => x.Product).Include(x => x.Order).OrderByDescending(x => x.Order.CreateAt).ThenByDescending(x => x.OrderId).ToListAsync();
                    foreach(var item in orderDetails)
                    {
                        worksheet.Cells[i + 2, 1].Value = item.OrderId;
                        worksheet.Cells[i + 2, 2].Value = item.Product.Name;
                        worksheet.Cells[i + 2, 3].Value = item.Quantity;
                        worksheet.Cells[i + 2, 4].Value = item.Price;
                        worksheet.Cells[i + 2, 5].Value = item.Quantity* item.Price;

                        // Thiết lập border cho từng ô trong hàng này
                        for (int j = 1; j <= headers.Length; j++)
                        {
                            worksheet.Cells[i + 2, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin); // Viền mỏng
                        }

                        // Căn lề cho các ô chứa text
                        worksheet.Cells[i + 2, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left; // Căn lề trái
                        worksheet.Cells[i + 2, 3, i + 2, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right; // Căn phải
                    i++;    
                }
              
                

                // Định dạng cột để kích thước tự động
                worksheet.Cells[2, 1, orderDetails.Count() + 1, headers.Length].AutoFitColumns();

                // Lưu dữ liệu vào MemoryStream
                var memoryStream = new MemoryStream();
                await package.SaveAsAsync(memoryStream);

                // Trả về dữ liệu Excel dưới dạng mảng byte
                return memoryStream.ToArray();
            }
        }


    }
}
