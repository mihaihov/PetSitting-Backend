using Microsoft.AspNetCore.SignalR;
using PetSitting.Application.Interfaces.Services;
using PetSitting.Domain.Entities.Messaging;
using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Api.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMessagingServices _messagingServices;
        public ChatHub(IMessagingServices messagingServices)
        {
            _messagingServices = messagingServices;
        }

        //called by the client when a chat is initiated (a petowner accepted a job offer from a petsitter).
        public async Task InitiateChat(string firstUserId, string secondUserId, string jobPostId)
        {
            var chatSessionId = _messagingServices.GenerateChatSessionId(firstUserId,secondUserId);
            await _messagingServices.CreateChatSession(firstUserId, secondUserId, jobPostId);

            await Groups.AddToGroupAsync(Context.ConnectionId, chatSessionId);
        }

        //called by the client when a chat already existed (the user reopened the app, reopened the chat window form the browser, etc)
        public async Task JoinChat(string chatSessionId)
        {
            if (! await _messagingServices.CanUsersChat(chatSessionId))
                throw new HubException("Chat is restricted!");

            await Groups.AddToGroupAsync(Context.ConnectionId, chatSessionId);
            var messages = await _messagingServices.GetRecenteMessagesAsync(chatSessionId,20);
            if(messages != null)
                await Clients.Group(chatSessionId).SendAsync("ReceiveMessages", messages);
        }

        public async Task SendMessage(string chatSessionId, string message)
        {
            if (! await _messagingServices.CanUsersChat(chatSessionId))
                throw new HubException("Chat is restricted!");
            
            await Clients.Group(chatSessionId).SendAsync("ReceiveMessage", message);
        }
    }
}