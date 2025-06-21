using Stripe;
using PetSitting.Application.Interfaces.Services;
using MediatR;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Stripe;
using Microsoft.Extensions.Configuration;
using PetSitting.Application.Features.Stripe.Commands;
using PetSitting.Application.Exceptions;

namespace PetSitting.Infrastructure.Services
{
    public class StripeServices : IStripeServices
    {
        private readonly StripeClient _client;
        private readonly IStripeTransactionRepository _stripeTransactionRepository;
        private readonly IBaseRepository<StripeAccount> _stripeAccountRepository;
        private readonly IMediator _mediator;

        public StripeServices(IConfiguration configuration,
            IStripeTransactionRepository stripeTransactionRepository, 
            IBaseRepository<StripeAccount> stripeAccountRepository, 
            IMediator mediator)
        {
            var apiKey = StripeConfiguration.ApiKey = configuration["Stripe:TestEnvironment:SecretKey"];
            _client = new StripeClient(apiKey);
            _stripeTransactionRepository = stripeTransactionRepository;
            _stripeAccountRepository = stripeAccountRepository;
            _mediator = mediator;
        }

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
            string? destinationEmail = null)
        {
            var options = new PaymentIntentCreateOptions 
            {
                Amount = amount,
                Currency = currency,
                TransferData = new PaymentIntentTransferDataOptions {
                    Destination = destinationAccount
                },
                Confirm = true
            };

            var service  = new PaymentIntentService();
            var intent = await service.CreateAsync(options);
            return intent;
        }

        public async Task HandleWebHooks(Event stripeEvent)
        {
            switch (stripeEvent.Type)
            {
                case EventTypes.AccountUpdated:
                    var stripePayload = stripeEvent.Data.Object as Stripe.Account;
                    if (stripePayload is null) break;

                    var stripeAccount = await _stripeAccountRepository.GetByIdAsync(stripeEvent.Account);
                    if (stripeAccount is null)
                        break;

                    stripeAccount.IsVerified = stripePayload?.Requirements.PendingVerification?.Count > 0 ? false : true;
                    stripeAccount.IsDisabled = !string.IsNullOrEmpty(stripePayload?.Requirements.DisabledReason);
                    stripeAccount.UpdatedAt = DateTime.Now;
                    await _stripeAccountRepository.UpdateAsync(stripeAccount);
                    break;
                case EventTypes.PaymentIntentCreated:
                    var paymentIntentCreated = stripeEvent.Data.Object as Stripe.PaymentIntent;
                    if (paymentIntentCreated is null) break;

                    await _mediator.Send(new CreateTransactionCommand(paymentIntentCreated.Id,
                        paymentIntentCreated.Status, paymentIntentCreated.Amount, paymentIntentCreated.Currency,
                        paymentIntentCreated.Created, paymentIntentCreated.TransferData?.DestinationId, paymentIntentCreated.CustomerId));
                    break;
                case EventTypes.PaymentIntentSucceeded:
                    var paymentIntentSucceeded = stripeEvent.Data.Object as Stripe.PaymentIntent;
                    if (paymentIntentSucceeded is null) break;

                    var stripeTransaction = await _stripeTransactionRepository.GetByPaymentIntentId(paymentIntentSucceeded.Id);
                    if (stripeTransaction is null) break;

                    stripeTransaction.Status = paymentIntentSucceeded.Status;

                    await _stripeTransactionRepository.UpdateAsync(stripeTransaction);
                    break;
                case EventTypes.PaymentIntentPaymentFailed:
                    var paymentIntentFailed = stripeEvent.Data.Object as Stripe.PaymentIntent;
                    if (paymentIntentFailed is null) break;

                    var failedTransaction = await _stripeTransactionRepository.GetByPaymentIntentId(paymentIntentFailed.Id);
                    if (failedTransaction is null) break;

                    failedTransaction.Status = paymentIntentFailed.Status;
                    await _stripeTransactionRepository.UpdateAsync(failedTransaction);
                    break;
                default:
                    throw new StripeWebhookNotHandledException();
            }
        }
    }
}