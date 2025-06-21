
using Stripe;

namespace PetSitting.Application.Interfaces.Services
{
    public interface IStripeServices
    {
        public Task<string> CreateAccount(string email);
        public Task<string> GenerateAccountLink(string accountId, string refreshUrl, string returnUrl);
        public Task<PaymentIntent> CreatePaymentIntent(long ammunt, string currency, string destinationAccount,
            string? destinationEmail);
        public Task HandleWebHooks(Event stripeEvent);
    }
}