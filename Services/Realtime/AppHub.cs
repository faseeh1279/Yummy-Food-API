using Microsoft.AspNetCore.SignalR;

namespace Yummy_Food_API.Services.Realtime
{
    public class AppHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var email = httpContext?.Request.Query["email"];
            var role = httpContext?.Request.Query["role"];

            if (!string.IsNullOrEmpty(email))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, email!);
            }

            if (!string.IsNullOrEmpty(role))
            {
                // Add user to their role group
                await Groups.AddToGroupAsync(Context.ConnectionId, role!);
            }
            await base.OnConnectedAsync();
        }
        // Send message to a specific user
        public async Task SendMessageToUser(string receiverEmail, string message)
        {
            var sender = Context.GetHttpContext()?.Request.Query["email"];
            await Clients.Group(receiverEmail).SendAsync("ReceiveMessage", sender, message);
        }

        // Send message to a role (e.g., all riders)
        public async Task SendMessageToRole(string role, string message)
        {
            var sender = Context.GetHttpContext()?.Request.Query["email"];
            await Clients.Group(role).SendAsync("ReceiveMessage", sender, message);
        }

        // Send message to a ride-specific group
        public async Task SendMessageToRide(string rideId, string message)
        {
            var sender = Context.GetHttpContext()?.Request.Query["email"];
            await Clients.Group($"Ride_{rideId}").SendAsync("ReceiveMessage", sender, message);
        }

        // Allow user to join a ride chat
        public async Task JoinRideGroup(string rideId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Ride_{rideId}");
        }
    }
}
