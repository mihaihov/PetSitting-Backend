
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Domain.Entities.Stripe;
using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Infrastructure.Repositories
{
    public class StripeTransactionRepository : IStripeTransactionRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _dbContext;
        public StripeTransactionRepository(IUserRepository userRepository, IBaseRepository<StripeTransaction> stripeTransactionRepository,
            ApplicationDbContext dbContext)
        {
            _userRepository = userRepository;
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<StripeTransaction>?> GetAllByJobPost(string jobPostId)
        {
            return await _dbContext.Set<JobPost>().Where(jp => jp.Id == jobPostId).Include(jb => jb.StripeTransactions)
                .ToListAsync() as IReadOnlyList<StripeTransaction>;
        }

        public async Task<IReadOnlyList<StripeTransaction>?> GetAllByUser(string email)
        {
            IQueryable<ApplicationUser> transactions = _userRepository.QueryByEmailAsync(email).Include(u => u.StripeTransactions);
            var getTransactions = await transactions.ToListAsync() as IReadOnlyList<StripeTransaction>;
            return getTransactions;
        }

        public async Task<IReadOnlyList<StripeTransaction>?> GetAllByUser(ApplicationUser applicationUser)
        {
            return await _userRepository.QueryByIdAsync(applicationUser.Id)
                .Include(u => u.StripeTransactions).ToListAsync() as IReadOnlyList<StripeTransaction>;
        }

        public async Task<StripeTransaction?> GetByIdAsync(string transactionId)
        {
            return await _dbContext.Set<StripeTransaction>().FindAsync(transactionId);
        }

        public async Task UpdateAsync(StripeTransaction entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}