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
        public string? StripeAccountid {get;set;}
        public StripeAccount? StripeAccount {get;set;}
        public string? PaidById {get;set;}
        public string? PaidToId {get;set;}
        public string? JobPostId {get;set;}
        public JobPost? JobPost {get;set;}
    }
}