using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace GreenShop.Hubs
{
    public class OrderHubs : Hub
    {
        // Khách hàng s? g?i JoinGroup(userId) sau khi k?t n?i ?? nh?n thông báo riêng
        public async Task JoinGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
        }

        public async Task LeaveGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user-{userId}");
        }
    }
}