using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using GreenShop.Models;
using System.IdentityModel.Tokens.Jwt;
using System.IO;

namespace GreenShop.Pages.Cart
{
    public class EditShipmentModel : PageModel
    {
        private readonly GreenShop.Models.GreenShopContext _context;

        public EditShipmentModel(GreenShop.Models.GreenShopContext context)
        {
            _context = context;
        }
        public IActionResult OnGet(string receiver, string PhoneNumber, string city, string district, string ward, string street, string UserId)
        {
            Guid? userId = getIdFromToken();
            ShipmentDetail shipmentDetail = _context.ShipmentDetails.FirstOrDefault(shipmentDetail => shipmentDetail.UserId == userId);
            shipmentDetail.PhoneNumber = PhoneNumber;
            shipmentDetail.Address = city + "," + district + "," + ward + "," + street;
            shipmentDetail.Receiver = receiver;
            _context.ShipmentDetails.Update(shipmentDetail);
            _context.SaveChanges();
            return RedirectToPage("./CartView");
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

    }
}
