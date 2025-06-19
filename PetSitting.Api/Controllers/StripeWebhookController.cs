using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Stripe.Commands;
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
        private readonly IMediator _mediator;
        
        public StripeWebhookController(IConfiguration configuration, IStripeTransactionRepository stripeTransactionRepository, 
            IBaseRepository<StripeAccount> stripeAccountRepository, IMediator mediator)
        {
            var apiKey = StripeConfiguration.ApiKey = configuration["Stripe:TestEnvironment:SecretKey"];
            _client = new StripeClient(apiKey);
            _webhookSecret = configuration["Stripe:TestEnvironment:WebhookSignature"]!;
            _stripeTransactionRepository = stripeTransactionRepository;
            _stripeAccountRepository = stripeAccountRepository;
            _mediator = mediator;
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

                        await _mediator.Send(new CreateTransactionCommand(paymentIntentCreated.Metadata["internalTransactionId"],
                            paymentIntentCreated.Id,paymentIntentCreated.Status,paymentIntentCreated.Amount,paymentIntentCreated.Currency,
                            paymentIntentCreated.Created,paymentIntentCreated.TransferData?.DestinationId,paymentIntentCreated.CustomerId));
                    break;
                    case EventTypes.PaymentIntentSucceeded:
                        var paymentIntentSucceeded = stripeEvent.Data.Object as Stripe.PaymentIntent;
                        if(paymentIntentSucceeded is null) break;

                        var stripeTransaction = await _stripeTransactionRepository.GetByPaymentIntentId(paymentIntentSucceeded.Id);
                        if (stripeTransaction is null) break;
                        
                        stripeTransaction.Status = paymentIntentSucceeded.Status;
                        
                        await _stripeTransactionRepository.UpdateAsync(stripeTransaction);
                    break;
                    case EventTypes.PaymentIntentPaymentFailed:
                        var paymentIntentFailed = stripeEvent.Data.Object as Stripe.PaymentIntent;
                        if(paymentIntentFailed is null) break;

                        var failedTransaction = await _stripeTransactionRepository.GetByPaymentIntentId(paymentIntentFailed.Id);
                        if (failedTransaction is null) break;

                        failedTransaction.Status = paymentIntentFailed.Status;
                        await _stripeTransactionRepository.UpdateAsync(failedTransaction);
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