using Microsoft.AspNetCore.Identity;
using PetSitting.Domain.Entities.NewsFeed;
using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Domain.Entities.ReviewSystem;
using PetSitting.Domain.Entities.Security;
using PetSitting.Domain.Entities.Stripe;
using NewsFeed = PetSitting.Domain.Entities.NewsFeed;

namespace PetSitting.Domain.Entities.UserManagement
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateJoined { get; set; }
        public bool IsVerified { get; set; } = false;
        public bool IsPetSitter { get; set; } = false;

        //FKs
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public string? StripeAccountId { get; set; }
        public StripeAccount? StripeAccount { get; set; }
        //one user can pay for multiple petsitting jobs. The FK from StripeTransaction would be PaidBy.
        public virtual ICollection<StripeTransaction>? StripeTransactions { get; set; }
        public UserProfile? UserProfile { get; set; }
        public UserSettings? UserSettings { get; set; }
    }
}