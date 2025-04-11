using PetSitting.Domain.Entities.Messaging;

namespace PetSitting.Application.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        public Task<Message?> GetByIdAsync(string messageId);
        public Task AddAsync(Message message);
        public Task<ICollection<Message>?> GetLatestNMessagesSentByUser(string userId, int n);
        public Task<ICollection<Message>?> GetLatestNMessagesReceivedByUser(string userId, int n);
        public Task<ICollection<Message>?> GetUserMessagesByDate(string userId, DateTime date);
    }
}