using PetSitting.Domain.Entities.Messaging;

namespace PetSitting.Application.Interfaces.Repositories
{
    public interface IChatSessionRepository
    {
        public Task<ChatSession?> GetByIdAsync(string chatSessionId);
        public Task<ICollection<ChatSession>?> GetByUserAsync(string userId);
        public Task AddAsync(ChatSession chatSession);
        public Task DeleteAsync(ChatSession chatSession);
        public Task<ICollection<Message>?> GetRecentMessages(string chatId, int count);
    }
}