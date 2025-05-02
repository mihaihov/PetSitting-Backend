using PetSitting.Domain.Entities.Stripe;

namespace PetSitting.Domain.Entities.PostManagement
{
    public class JobPost : Post
    {
        public string? Title { get; set; }
        public required string Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Payment { get; set; }
        public bool IsOpen { get; set; } = true;

        //FKs
        public ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();
        //one JobPost can have multiple transactions. For example if the payment is paid 50-50 in advanced and after.
        public ICollection<StripeTransaction>? StripeTransactions {get;set;}
    }

}