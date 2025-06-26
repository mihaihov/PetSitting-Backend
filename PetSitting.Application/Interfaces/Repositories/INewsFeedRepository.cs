using PetSitting.Domain.Entities.NewsFeed;

namespace PetSitting.Application.Interfaces.Repositories
{
    public interface INewsFeedRepository
    {
        public IQueryable<Feed>? QueryFeedByUser(string userId);
        public IQueryable<Feed>? QueryFeedByPost(string postId);
        public Task<IReadOnlyList<Feed>?> GetFeedByUserAsync(string userId);
        public Task<IReadOnlyList<Feed>?> GetFeedByPostAsync(string postId);
    }
}