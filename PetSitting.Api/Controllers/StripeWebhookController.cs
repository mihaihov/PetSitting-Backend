using Microsoft.AspNetCore.Mvc;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Stripe;
using PetSitting.Domain.Enums;
using Stripe;
using Stripe.Events;

namespace PetSitting.Api.Controllers
{
    [Route("api/[controller]/")]
    public class StripeWebhookController : Controller
    {
        private readonly StripeClient _client;
        private readonly string _webhookSecret;
        private readonly IStripeTransactionRepository _stripeTransactionRepository;
        private readonly IBaseRepository<StripeAccount> _stripeAccountRepository;
        
        public StripeWebhookController(IConfiguration configuration, IStripeTransactionRepository stripeTransactionRepository, 
            IBaseRepository<StripeAccount> stripeAccountRepository)
        {
            var apiKey = StripeConfiguration.ApiKey = configuration["Stripe:TestEnvironment:SecretKey"];
            _client = new StripeClient(apiKey);
            _webhookSecret = configuration["Stripe:TestEnvironment:WebhookSignature"]!;
            _stripeTransactionRepository = stripeTransactionRepository;
            _stripeAccountRepository = stripeAccountRepository;
        }
        [HttpPost("index")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _webhookSecret);

                switch (stripeEvent.Type) {
                    case EventTypes.AccountUpdated:
                        if(stripeEvent is null) break;
                        var stripePayload = stripeEvent.Data.Object as Stripe.Account;
                        if(stripePayload is null)   break;

                        var stripeAccount = await _stripeAccountRepository.GetByIdAsync(stripeEvent.Account);
                        if(stripeAccount is null)  
                            break;
                        
                        stripeAccount.IsVerified = stripePayload?.Requirements.PendingVerification?.Count > 0 ? false : true;
                        stripeAccount.IsDisabled = !string.IsNullOrEmpty(stripePayload?.Requirements.DisabledReason);
                        stripeAccount.UpdatedAt = DateTime.Now;
                        await _stripeAccountRepository.UpdateAsync(stripeAccount);
                    break;
                    case EventTypes.PaymentIntentCreated:
                        var paymentIntentCreated = stripeEvent.Data.Object as Stripe.PaymentIntent;
                        if(paymentIntentCreated is null) break;

                        //THIS CAN NEVER BE NULL. TRANSACTION IS CREATED FIRST IN THE BACKEND AND THEN BY STRIPE
                        var stripeTransaction = await _stripeTransactionRepository.GetByIdAsync(paymentIntentCreated.Id);

                        stripeTransaction!.PaymentIntentId = paymentIntentCreated.Id;
                        stripeTransaction!.Status = paymentIntentCreated.Status;
                        stripeTransaction!.Amount = paymentIntentCreated.Amount;
                        stripeTransaction!.Currency = paymentIntentCreated.Currency;
                        stripeTransaction!.CreatedAt = paymentIntentCreated.Created;
                        
                        await _stripeTransactionRepository.UpdateAsync(stripeTransaction);
                    break;
                    case EventTypes.PaymentIntentSucceeded:
                        break;
                    case EventTypes.PaymentIntentPaymentFailed:
                        break;
                    case EventTypes.TransferCreated:
                        break;
                    case EventTypes.PayoutFailed:
                        break;
                    default:
                        throw new StripeWebhookNotHandledException();
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}