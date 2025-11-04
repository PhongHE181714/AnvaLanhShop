using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project_PRN221.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Project_PRN221.Pages.Cart
{
    public class AddNewShipmentModel : PageModel
    {
        private readonly Project_PRN221.Models.GreenShopContext _context;

        public AddNewShipmentModel(Project_PRN221.Models.GreenShopContext context)
        {
            _context = context;
        }
        public IActionResult OnGet(string receiver,string PhoneNumber,string city,string district,string ward,string street,string UserId)
        {
            Guid? userId= getIdFromToken();
            ShipmentDetail shipmentDetail = new ShipmentDetail();
            shipmentDetail.PhoneNumber = PhoneNumber;
            shipmentDetail.Address = city+","+district+","+ward+","+street;
            shipmentDetail.Id=Guid.NewGuid();
            shipmentDetail.UserId = (Guid)userId;
            shipmentDetail.Status = 1;
            shipmentDetail.Receiver = receiver;
            _context.ShipmentDetails.Add(shipmentDetail);
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
