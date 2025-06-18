using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Domain.Entities.Stripe
{
    public class StripeAccount
    {
        public string Id {get;set;} = Guid.NewGuid().ToString();
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime? UpdatedAt {get;set;}
        public bool? IsVerified {get;set;} = false;
        public bool? IsDisabled {get;set;} = false;

        //FKs
        public required string ApplicationUserId {get;set;}
        public ApplicationUser? ApplicationUser {get;set;}
        public ICollection<StripeTransaction>? StripeTransactions {get;set;}
    }
}