using Microsoft.AspNetCore.SignalR.Client;

namespace Yummy_Food_API.Services.Realtime
{
    public class OrderNotificationService
    {
        private readonly HubConnection _hubConnection;
        private readonly List<object> _messages = new();
        public IReadOnlyList<object> ReceivedData => _messages;


        public OrderNotificationService()
        {
            _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7284/chatHub")
            .WithAutomaticReconnect()
            .Build();

            // Listen for messages
            _hubConnection.On<object>("OrderState", (message) =>
            {
                _messages.Add(message);
            });

            Task.Run(() => _hubConnection.StartAsync());
        }

        public async Task SendMessageAsync(string user, object message)
        {
            await _hubConnection.InvokeAsync("BroadCastOrderMessageAsync", user, message);
        }

        public async Task BroadCastMessageAsync(object message)
        {
            await _hubConnection.InvokeAsync("BroadCastOrderMessageAsync", message); 
        }
    }
}
