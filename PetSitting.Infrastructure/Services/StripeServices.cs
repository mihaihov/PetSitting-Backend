using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PetSitting.Application.Interfaces.Services;
using Stripe;

namespace PetSitting.Infrastructure.Services
{
    public class StripeServices : IStripeServices
    {
        public async Task<string> CreateAccount(string email)
        {
            var accountOptions = new AccountCreateOptions {
                Type = "express",
                Email = email
            };

            var service = new AccountService(); 
            var account = await service.CreateAsync(accountOptions);
            return account.Id;
        }

        public async Task<string> GenerateAccountLink(string accountId, string refreshUrl, string returnUrl)
        {
            var options = new AccountLinkCreateOptions {
                Account = accountId,
                RefreshUrl = refreshUrl,
                ReturnUrl = returnUrl,
                Type = "account_onboarding"
            };

            var service = new AccountLinkService();
            var accountLink = await service.CreateAsync(options);
            return accountLink.Url;
        }

        public async Task<PaymentIntent> CreatePaymentIntent(long amount, string currency, string destinationAccount,
            string? destinationEmail = null, Dictionary<string,string>? metadata = null)
        {
            var options = new PaymentIntentCreateOptions 
            {
                Amount = amount,
                Currency = currency,
                TransferData = new PaymentIntentTransferDataOptions {
                    Destination = destinationAccount
                },
                Metadata = metadata,
                Confirm = true
            };

            var service  = new PaymentIntentService();
            var intent = await service.CreateAsync(options);
            return intent;
        }
    }
}