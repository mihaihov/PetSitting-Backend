using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Domain.Entities.UserManagement;
using PetSitting.Domain.Enums;

namespace PetSitting.Domain.Entities.Stripe
{
    public class StripeTransaction
    {
        public string Id {get;set;} = Guid.NewGuid().ToString();
        public StripeTransactionstatus Status {get;set;}
        public decimal Amount {get;set;}
        public string Currency {get;set;} = "USD";
        public string? PaymentIntentId {get;set;}
        public DateTime? CreatedAt {get;set;} = DateTime.Now;

        //FKs
        public StripeAccount? StripeAccount {get;set;}
        public ApplicationUser? PaidBy {get;set;}
        public JobPost? JobPost {get;set;}
    }
}