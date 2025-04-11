using PetSitting.Domain.Entities.Messaging;
using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Application.Interfaces.Services
{
    public interface IMessagingServices
    {
        public Task<ChatSession?> DoesChatSessionExists(string chatSessionId);
        public Task<bool> CanUsersChat(string chatSessionId);
        public Task CreateChatSession(string firstUser, string secondUser, string jobPostId);
        public Task<ICollection<Message>?> GetRecenteMessagesAsync(string chatSessionId, int count);
        public string GenerateChatSessionId(string firstUser, string secondUser);
    }
}