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
        
    }
}
