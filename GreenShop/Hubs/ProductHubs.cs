using Microsoft.AspNetCore.SignalR;

namespace GreenShop.Hubs
{
    public class ProductHubs:Hub
    {
        public async Task SendStudentUpdate(Guid productId, string action)
        {
            await Clients.All.SendAsync("ReceiveStudentUpdate", productId, action);
        }
    }
}
