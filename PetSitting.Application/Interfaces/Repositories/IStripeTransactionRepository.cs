using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Domain.Entities.Stripe;
using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Application.Interfaces.Repositories
{
    public interface IStripeTransactionRepository : IBaseRepository<StripeTransaction>
    {
        public Task<IReadOnlyList<StripeTransaction>?> GetAllByUser(string email);
        public Task<IReadOnlyList<StripeTransaction>?> GetAllByUser(ApplicationUser applicationUser);
        public Task<IReadOnlyList<StripeTransaction>?> GetAllByJobPost(string jobPostId);

    }
}