using PetSitting.Domain.Entities.ReviewSystem;

namespace PetSitting.Application.Interfaces.Repositories
{
    public interface IReviewRepository : IBaseRepository<Review>
    {
        public Task<ICollection<Review>?> GetByPostIdAsync(string postId);
        public Task<ICollection<Review>?> GetByAuthorIdAsync(string userId);
        public Task<ICollection<Review>?> GetReviewsByAuthorToPost(string userId, string postId);
    }
}