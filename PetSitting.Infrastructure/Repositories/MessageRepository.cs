using Microsoft.EntityFrameworkCore;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Messaging;

namespace PetSitting.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public MessageRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;   
        }
        public async Task AddAsync(Message message)
        {
            await _dbContext.Set<Message>().AddAsync(message);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Message?> GetByIdAsync(string messageId)
        {
            return await _dbContext.Set<Message>().FindAsync(messageId);
        }

        public async Task<ICollection<Message>?> GetLatestNMessagesSentByUser(string userId, int n)
        {
            return await _dbContext.Set<Message>().Where(m => m.SenderId == userId).Take(n).ToListAsync();
        }

        public async Task<ICollection<Message>?> GetLatestNMessagesRecievedByUser(string userId, int n)
        {
            return await _dbContext.Set<Message>().Where(m => m.RecipientId == userId).Take(n).ToListAsync();
        }


        public async Task<ICollection<Message>?> GetMessagesByDate(string userId, DateTime date)
        {
            return await _dbContext.Set<Message>().Where(m => m.Timestamp.Date == date.Date).ToListAsync();
        }
    }
}