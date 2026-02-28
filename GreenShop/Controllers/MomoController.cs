using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using GreenShop.Service;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using GreenShop.Models;

namespace GreenShop.Controllers
{
    [ApiController]
    [Route("api/momo")]
    public class MomoController : ControllerBase
    {
        private readonly MomoSettings _momo;
        private readonly GreenShopContext _context;

        public MomoController(IOptions<MomoSettings> momoOptions, GreenShopContext context)
        {
            _momo = momoOptions.Value;
            _context = context;
        }

        [HttpPost("notify")]
        public IActionResult Notify([FromBody] JObject body)
        {
            // L?y các tr??ng c?n thi?t t? body
            var partnerCode = (string)body["partnerCode"];
            var orderId = (string)body["orderId"];
            var requestId = (string)body["requestId"];
            var amount = (string)body["amount"];
            var orderInfo = (string)body["orderInfo"];
            var orderType = (string)body["orderType"];
            var transId = (string)body["transId"];
            var resultCode = (string)body["resultCode"];
            var message = (string)body["message"];
            var payType = (string)body["payType"];
            var responseTime = (string)body["responseTime"];
            var extraData = (string)body["extraData"];
            var signature = (string)body["signature"];

            // T?o raw signature string theo tài li?u Momo
            var rawSignature = $"amount={amount}&extraData={extraData}&message={message}&orderId={orderId}&orderInfo={orderInfo}&orderType={orderType}&partnerCode={partnerCode}&payType={payType}&requestId={requestId}&responseTime={responseTime}&resultCode={resultCode}&transId={transId}";

            string mySignature;
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_momo.SecretKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawSignature));
                mySignature = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }

            if (mySignature != signature)
            {
                return BadRequest(new { message = "Invalid signature" });
            }

            // N?u thanh toán thành công (resultCode == "0"), c?p nh?t tr?ng thái ??n hàng
            if (resultCode == "0")
            {
                // orderId b?n truy?n vào lúc t?o payment, nên l?u orderId vào DB khi t?o ??n
                var order = _context.Orders.FirstOrDefault(o => o.Id.ToString() == orderId);
                if (order != null)
                {
                    order.Status = 1; // ?ã thanh toán
                    _context.SaveChanges();
                }
            }

            // Tr? v? response cho Momo
            return Ok(new
            {
                resultCode = 0,
                message = "Confirm Success"
            });
        }
    }
}