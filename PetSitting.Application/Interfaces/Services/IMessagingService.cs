using PetSitting.Domain.Entities.Messaging;
using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Application.Interfaces.Services
{
    public interface IMessagingServices
    {
        public Task<bool> DoesChatSessionExists(string chatSessionId);
        public bool CanUsersChat(ApplicationUser firstUser, ApplicationUser secondUser, JobPost jobPost, ChatSession chatSession);
        public Task CreateChatsession(string firstUser, string secondUser, string jobPostId);
        public Task<ICollection<Message>?> GetRecenteMessagesAsync(string chatSessionId, int count);
        public string GenerateChatSessionId(string firstUser, string secondUser);
    }
}