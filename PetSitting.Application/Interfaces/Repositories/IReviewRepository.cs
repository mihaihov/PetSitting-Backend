using PetSitting.Domain.Entities.ReviewSystem;

namespace PetSitting.Application.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        public Task<ICollection<Review>?> GetByPostIdAsync(string postId);
        public Task<ICollection<Review>?> GetByAuthorIdAsync(string userId);
    }
}