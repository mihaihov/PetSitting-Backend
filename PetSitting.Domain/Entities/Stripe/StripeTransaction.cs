using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Domain.Entities.UserManagement;
using PetSitting.Domain.Enums;

namespace PetSitting.Domain.Entities.Stripe
{
    public class StripeTransaction
    {
        public string Id {get;set;} = Guid.NewGuid().ToString();
        public string? Status {get;set;} = string.Empty;
        public decimal? Amount {get;set;}
        public string? Currency {get;set;} = "USD";
        public string? PaymentIntentId {get;set;}
        public DateTime? CreatedAt {get;set;} = DateTime.Now;

        //FKs
        public StripeAccount? StripeAccount {get;set;}
        public string? PaidById {get;set;}
        public StripeAccount? PaidBy {get;set;}
        public string? PaidToId {get;set;}
        public StripeAccount? PaidTo {get;set;}
        public string? JobPostId {get;set;}
        public JobPost? JobPost {get;set;}
    }
}