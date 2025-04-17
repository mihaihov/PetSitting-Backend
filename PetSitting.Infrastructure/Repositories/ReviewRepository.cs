using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.ReviewSystem;

namespace PetSitting.Infrastructure.Repositories
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<ICollection<Review>?> GetByAuthorIdAsync(string userId)
        {
            return await _dbContext.Reviews.Where(r => r.AuthorId == userId).ToListAsync();
        }

        public async Task<ICollection<Review>?> GetByPostIdAsync(string postId)
        {
            return await _dbContext.Reviews.Where(r => r.PostId == postId).ToListAsync();
        }

        public async Task<ICollection<Review>?> GetReviewsByAuthorToPost(string userId, string postId)
        {
            return await _dbContext.Reviews.Where(r => r.AuthorId == userId && r.PostId == postId).ToListAsync();
        }
    }
}