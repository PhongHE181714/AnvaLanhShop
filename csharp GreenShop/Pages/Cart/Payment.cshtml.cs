using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using GreenShop.Models;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using GreenShop.Services;

namespace GreenShop.Pages.Shop
{
    public class PaymentModel : PageModel
    {
        private readonly GreenShop.Models.GreenShopContext _context;
        private readonly MomoSettings _momo;

        public PaymentModel(GreenShop.Models.GreenShopContext context, IOptions<MomoSettings> momoOptions)
        {
            _context = context;
            _momo = momoOptions.Value;
        }

        public string url { get; set; }
        public IActionResult OnGet(double price, int method)
        {
            this.itemCart = getCart();
            Guid? userId = getIdFromToken();
            Models.User user = _context.Users.FirstOrDefault(x => x.Id == userId);
            ShipmentDetail detail = _context.ShipmentDetails.FirstOrDefault(x => x.UserId == userId);
            List<Models.Cart> carts = _context.Carts.Include(x => x.Product).Where(x => x.UserId == userId).ToList();
            Guid paymentId = Guid.NewGuid();
            //add order and delete cart
            if (user != null && detail != null && carts.Count > 0)
            {

                Order order = new Order();
                order.Id = Guid.NewGuid();
                paymentId = order.Id;
                order.TotalPrice = (float)price;
                order.Status = 0;
                order.UserId = (Guid)userId;
                order.Note = "User:" + userId + " Type:" + (method == 1 ? "Momo" : "COD");
                order.ShipmentDetailsId = detail.Id;
                order.CreateAt = DateTime.Now;
                try
                {
                    //add order
                    _context.Orders.Add(order);
                    //get cart
                    foreach (var cart in carts)
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.Id = Guid.NewGuid();
                        orderDetail.OrderId = order.Id;
                        orderDetail.ProductId = cart.ProductId;
                        orderDetail.Quantity = cart.Quantity;
                        orderDetail.Price = cart.Product.Price;
                        //add order detail
                        _context.OrderDetails.Add(orderDetail);
                    }
                    //delete cart
                    _context.Carts.RemoveRange(carts);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }

            if (method == 1)
            {
                // Build Momo payment request
                var endpoint = _momo.Endpoint;
                var partnerCode = _momo.PartnerCode;
                var accessKey = _momo.AccessKey;
                var secretKey = _momo.SecretKey;
                var returnUrl = _momo.ReturnUrl;
                var notifyUrl = _momo.NotifyUrl;
                var requestType = _momo.RequestType;

                var orderId = Guid.NewGuid().ToString();
                var requestId = Guid.NewGuid().ToString();
                var amount = ((int)Math.Ceiling(price)).ToString(); // Momo expects integer amount (VND)
                var orderInfo = $"Payment for order {paymentId}";
                var extraData = ""; // optional base64-encoded string

                // raw signature string format defined by Momo
                var rawSignature = $"accessKey={accessKey}&amount={amount}&extraData={extraData}&ipnUrl={notifyUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&returnUrl={returnUrl}&requestId={requestId}&requestType={requestType}";

                string signature;
                using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
                {
                    var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawSignature));
                    signature = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }

                var momoRequest = new
                {
                    partnerCode = partnerCode,
                    accessKey = accessKey,
                    requestId = requestId,
                    amount = amount,
                    orderId = orderId,
                    orderInfo = orderInfo,
                    returnUrl = returnUrl,
                    notifyUrl = notifyUrl,
                    extraData = extraData,
                    requestType = requestType,
                    signature = signature
                };

                var client = new RestClient(endpoint);
                var request = new RestRequest
                {
                    Method = RestSharp.Method.Post
                };
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(momoRequest);
                var response = client.Execute(request);

                if (!response.IsSuccessful)
                {
                    // fallback to page with error info
                    Console.WriteLine($"Momo request failed: {response.StatusCode} {response.Content}");
                    TempData["PaymentError"] = "Payment gateway error. Please try later.";
                    return Page();
                }

                // parse response
                var respObj = JsonConvert.DeserializeObject<dynamic>(response.Content);
                string payUrl = respObj.payUrl;
                if (!string.IsNullOrEmpty(payUrl))
                {
                    // redirect user to Momo payUrl
                    return Redirect(payUrl);
                }
                else
                {
                    Console.WriteLine("Momo returned no payUrl: " + response.Content);
                    TempData["PaymentError"] = "Payment gateway error. Please try later.";
                }
            }
            else
            {
                // COD or other method handling
            }
            return Page();
        }

        public int? itemCart { get; set; }
        public int getCart()
        {
            Guid? userId = getIdFromToken();
            if (userId != null)
            {
                List<Models.Cart> cart = new List<Models.Cart>();
                cart = _context.Carts.Where(c => c.UserId == userId).ToList();
                return cart.Count;
            }
            else
            {
                return 0;
            }

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
            // Assuming the GUID is stored in a claim named "userId"
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
    }
}