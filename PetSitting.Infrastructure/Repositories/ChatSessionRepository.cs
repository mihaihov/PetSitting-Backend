using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Messaging;

namespace PetSitting.Infrastructure.Repositories
{
    public class ChatSessionRepository : IChatSessionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ChatSessionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(ChatSession chatSession)
        {
            await _dbContext.Set<ChatSession>().AddAsync(chatSession);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(ChatSession chatSession)
        {
            _dbContext.Set<ChatSession>().Remove(chatSession);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ChatSession?> GetByIdAsync(string chatSessionId)
        {
            return await _dbContext.Set<ChatSession>().FindAsync(chatSessionId);
        }

        public async Task<ICollection<ChatSession>?> GetByUserAsync(string userId)
        {
            return await _dbContext.Set<ChatSession>().Where(cs => cs.FirstUserId == userId || cs.SecondUserId == userId).ToListAsync();
        }

        public async Task<ICollection<Message>?> GetRecentMessages(string chatId, int count)
        {
            return await _dbContext.ChatSessions
                .Where(cs => cs.ChatSessionId == chatId)
                .SelectMany(cs => cs.Messages)
                .OrderByDescending(m => m.Timestamp)
                .Take(count)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }
    }
}