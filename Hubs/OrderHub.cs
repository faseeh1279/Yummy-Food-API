using Microsoft.AspNetCore.SignalR;
using Yummy_Food_API.Models.DTOs;

namespace Yummy_Food_API.Hubs
{
    public class OrderHub : Hub
    {
        //public async Task SendMessage(string userId, string message)
        //{
        //    await Clients.User(userId).SendAsync("ReceiveMessage", message); 
        //}
        // BroadCast Order Message
        public async Task BroadCastOrderMessageAsync(object message)
        {
            await Clients.All.SendAsync("OrderState", message); 
        }
        // Send to Specific User
        public async Task OrderNotification(string userId, OrderNotificationDTO orderNotificationDTO)
        {
            await Clients.User(userId).SendAsync("OrderAccepted", orderNotificationDTO);
        }
        // ✅ Send private message to a specific user
        public async Task SendPrivateMessage(string userId, string message)
        {
            await Clients.User(userId).SendAsync("ReceiveMessage", Context.User?.Identity?.Name, message);
        }

        // ✅ On connection, add user to a role-based group
        public override async Task OnConnectedAsync()
        {
            var user = Context.User;
            if (user != null && user.IsInRole("Customer"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Customer");
            }

            if (user != null && user.IsInRole("Admin"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
            }

            await base.OnConnectedAsync();
        }
    }
}
