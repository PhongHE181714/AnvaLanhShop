using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Project_PRN221.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly Project_PRN221.Models.GreenShopContext _context;

        public IndexModel(Project_PRN221.Models.GreenShopContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(int? month, int? day, int? week)
        {
            if (month != null)
            {
                // Assuming you have a way to get the year, you may set it to a default value or get from the request.  
                int year = DateTime.Now.Year;

                if (week != null)
                {
                    //day
                    if (day != null)
                    {
                        var date = new DateTime(year, month.Value, day.Value);
                        var OrderCount = await _context.Orders
                            .Where(o => o.CreateAt.Date == date)
                            .CountAsync();
                        var MoneyCount = await _context.Orders
                            .Where(o => o.CreateAt.Date == date)
                            .SumAsync(o => o.TotalPrice);
                        var user = await _context.Orders
                            .Include(o => o.User)
                            .Where(o => o.CreateAt.Date == date) // Filter orders by the specified date  
                            .GroupBy(o => o.UserId) // Group by UserId  
                            .Select(g => new
                            {
                                UserId = g.Key, // The user's ID  
                                UserName = g.FirstOrDefault().User.Username,
                                Email = g.FirstOrDefault().User.Email,
                                TotalPrice = g.Sum(o => o.TotalPrice) // The sum of TotalPrice for that user  
                            })
                            .OrderByDescending(u => u.TotalPrice) // Sort by TotalPrice in descending order  
                            .ToListAsync(); // Execute the query asynchronously;

                        var productRankings = await _context.Orders
                            .Include(o => o.OrderDetails)
                                .ThenInclude(od => od.Product) // Include related products in order details  
                            .Where(o => o.CreateAt.Date == date) // Filter orders by the specified date  
                            .SelectMany(o => o.OrderDetails) // Flatten the OrderDetails from each Order  
                            .GroupBy(od => od.ProductId) // Group by ProductId  
                            .Select(g => new
                            {
                                ProductId = g.Key, // The ProductId  
                                ProductName = g.Select(od => od.Product.Name).FirstOrDefault(), // Get product name from one of the records  
                                TotalQuantitySold = g.Sum(od => od.Quantity), // Sum of quantities sold for each product  
                            })
                            .OrderByDescending(p => p.TotalQuantitySold) // Order by total quantity sold  
                            .ToListAsync(); // Execute the query asynchronously  

                        // Now you can use productRankings to get the ranked product list.
                        ViewData["MoneyCount"] = MoneyCount;
                        ViewData["OrderCount"] = OrderCount;
                        ViewData["ViewCount"] = OrderCount * 10 + (int)OrderCount * 9 / 10;
                        ViewData["UserCount"] = user;
                        ViewData["ProductCount"] = productRankings;


                    }
                    //week
                    else
                    {
                        var startOfWeek = GetStartDateOfWeek(year, month.Value, week.Value);
                        var endOfWeek = startOfWeek.AddDays(7);

                        var OrderCount = await _context.Orders
                            .Where(o => o.CreateAt >= startOfWeek && o.CreateAt < endOfWeek)
                            .CountAsync();
                        var MoneyCount = await _context.Orders
                           .Where(o => o.CreateAt >= startOfWeek && o.CreateAt < endOfWeek)
                           .SumAsync(o => o.TotalPrice);
                        ;
                        var user = await _context.Orders
                        .Include(o => o.User)
                           .Where(o => o.CreateAt >= startOfWeek && o.CreateAt < endOfWeek)
                            .GroupBy(o => o.UserId) // Group by UserId  
                            .Select(g => new
                            {
                                UserId = g.Key, // The user's ID  
                                UserName = g.FirstOrDefault().User.Username,
                                Email = g.FirstOrDefault().User.Email,
                                TotalPrice = g.Sum(o => o.TotalPrice) // The sum of TotalPrice for that user  
                            })
                            .OrderByDescending(u => u.TotalPrice) // Sort by TotalPrice in descending order  
                            .ToListAsync(); // Execute the query asynchronously;

                        var productRankings = await _context.Orders
                            .Include(o => o.OrderDetails)
                                .ThenInclude(od => od.Product) // Include related products in order details  
                           .Where(o => o.CreateAt >= startOfWeek && o.CreateAt < endOfWeek)
                            .SelectMany(o => o.OrderDetails) // Flatten the OrderDetails from each Order  
                            .GroupBy(od => od.ProductId) // Group by ProductId  
                            .Select(g => new
                            {
                                ProductId = g.Key, // The ProductId  
                                ProductName = g.Select(od => od.Product.Name).FirstOrDefault(), // Get product name from one of the records  
                                TotalQuantitySold = g.Sum(od => od.Quantity), // Sum of quantities sold for each product  
                            })
                            .OrderByDescending(p => p.TotalQuantitySold) // Order by total quantity sold  
                            .ToListAsync(); // Execute the query
                        ViewData["MoneyCount"] = MoneyCount;
                        ViewData["OrderCount"] = OrderCount;
                        ViewData["ViewCount"] = OrderCount * 10 + (int)OrderCount * 9 / 10;
                        ViewData["UserCount"] = user;
                        ViewData["ProductCount"] = productRankings;
                    }

                }
                else
                {
                    //month
                    var startOfMonth = new DateTime(year, month.Value, 1);
                    var endOfMonth = startOfMonth.AddMonths(1);

                    var OrderCount = await _context.Orders
                        .Where(o => o.CreateAt >= startOfMonth && o.CreateAt < endOfMonth)
                        .CountAsync();
                    var MoneyCount = await _context.Orders
                        .Where(o => o.CreateAt >= startOfMonth && o.CreateAt < endOfMonth)
                        .SumAsync(o => o.TotalPrice);
                    var user = await _context.Orders
                            .Include(o => o.User)
                        .Where(o => o.CreateAt >= startOfMonth && o.CreateAt < endOfMonth)
                            .GroupBy(o => o.UserId) // Group by UserId  
                            .Select(g => new
                            {
                                UserId = g.Key, // The user's ID  
                                UserName = g.FirstOrDefault().User.Username,
                                Email = g.FirstOrDefault().User.Email,
                                TotalPrice = g.Sum(o => o.TotalPrice) // The sum of TotalPrice for that user  
                            })
                            .OrderByDescending(u => u.TotalPrice) // Sort by TotalPrice in descending order  
                            .Take(8)
                            .ToListAsync(); // Execute the query asynchronously;

                    var productRankings = await _context.Orders
                        .Include(o => o.OrderDetails)
                            .ThenInclude(od => od.Product) // Include related products in order details  
                        .Where(o => o.CreateAt >= startOfMonth && o.CreateAt < endOfMonth)
                        .SelectMany(o => o.OrderDetails) // Flatten the OrderDetails from each Order  
                        .GroupBy(od => od.ProductId) // Group by ProductId  
                        .Select(g => new
                        {
                            ProductId = g.Key, // The ProductId  
                            ProductName = g.Select(od => od.Product.Name).FirstOrDefault(), // Get product name from one of the records  
                            TotalQuantitySold = g.Sum(od => od.Quantity), // Sum of quantities sold for each product  
                        })
                        .OrderByDescending(p => p.TotalQuantitySold) // Order by total quantity sold  
                        .ToListAsync(); // Execute t
                    ViewData["MoneyCount"] = MoneyCount;
                    ViewData["OrderCount"] = OrderCount;
                    ViewData["ViewCount"] = OrderCount * 10 + (int)OrderCount * 9 / 10;
                    ViewData["UserCount"] = user;
                    ViewData["ProductCount"] = productRankings;

                }
            }
            else
            {
                var OrderCount = await _context.Orders
                    .CountAsync();
                var MoneyCount = await _context.Orders
                    .SumAsync(o => o.TotalPrice);
                var user = await _context.Orders
                        .Include(o => o.User)
                        .GroupBy(o => o.UserId) // Group by UserId  
                        .Select(g => new
                        {
                            UserId = g.Key, // The user's ID  
                            UserName = g.FirstOrDefault().User.Username,
                            Email = g.FirstOrDefault().User.Email,
                            TotalPrice = g.Sum(o => o.TotalPrice) // The sum of TotalPrice for that user  
                        })
                        .OrderByDescending(u => u.TotalPrice) // Sort by TotalPrice in descending order  
                        .ToListAsync(); // Execute the query asynchronously;

                var productRankings = await _context.Orders
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product) // Include related products in order details  
                    .SelectMany(o => o.OrderDetails) // Flatten the OrderDetails from each Order  
                    .GroupBy(od => od.ProductId) // Group by ProductId  
                    .Select(g => new
                    {
                        ProductId = g.Key, // The ProductId  
                        ProductName = g.Select(od => od.Product.Name).FirstOrDefault(), // Get product name from one of the records  
                        TotalQuantitySold = g.Sum(od => od.Quantity), // Sum of quantities sold for each product  
                    })
                    .OrderByDescending(p => p.TotalQuantitySold) // Order by total quantity sold  
                    .ToListAsync(); // Execute t
                ViewData["MoneyCount"] = MoneyCount;
                ViewData["OrderCount"] = OrderCount;
                ViewData["ViewCount"] = OrderCount * 10 + (int)OrderCount * 9 / 10;
                ViewData["UserCount"] = user;
                ViewData["ProductCount"] = productRankings;
            }

            return Page();
        }

        private DateTime GetStartDateOfWeek(int year, int month, int week)
        {
            // This method calculates the start date of the specified week in the given month and year.  
            var firstDayOfMonth = new DateTime(year, month, 1);
            var dayOfWeek = firstDayOfMonth.DayOfWeek;
            var daysToFirstMonday = (dayOfWeek == DayOfWeek.Sunday) ? 0 : 7 - (int)dayOfWeek;
            var firstMonday = firstDayOfMonth.AddDays(daysToFirstMonday);

            // Each week starts from first Monday  
            return firstMonday.AddDays((week - 1) * 7);
        }
    }
}
