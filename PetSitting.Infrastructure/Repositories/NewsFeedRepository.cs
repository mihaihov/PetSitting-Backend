using Microsoft.EntityFrameworkCore;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.NewsFeed;

namespace PetSitting.Infrastructure.Repositories
{
    public class NewsFeedRepository : BaseRepository<Feed>, INewsFeedRepository
    {
        public NewsFeedRepository(ApplicationDbContext _dbContext) : base(_dbContext) { }
        public async Task<IReadOnlyList<Feed>?> GetFeedByPostAsync(string postId)
        {
            return await QueryFeedByPost(postId).ToListAsync();
        }

        public async Task<IReadOnlyList<Feed>?> GetFeedByUserAsync(string userId)
        {
            return await QueryFeedByUser(userId).ToListAsync();
        }

        public IQueryable<Feed> QueryFeedByPost(string postId)
        {
            return _dbContext.Set<Feed>().Where(f => f.PostId == postId);
        }

        public IQueryable<Feed> QueryFeedByUser(string userId)
        {
            return _dbContext.Set<Feed>().Where(f => f.UserId == userId);
        }
    }
}