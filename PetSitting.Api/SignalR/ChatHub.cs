using Microsoft.AspNetCore.SignalR;

namespace PetSitting.Api.SignalR
{
    public class ChatHub : Hub
    {
        public async Task JoinChat(string chatSessionId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatSessionId);
        }

        public async Task SendMessage(string chatSessionId, string message)
        {
            await Clients.Group(chatSessionId).SendAsync("ReceiveMessage", message);
        }
    }
}