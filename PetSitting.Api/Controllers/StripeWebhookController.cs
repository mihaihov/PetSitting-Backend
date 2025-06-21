using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Stripe.Commands;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;
using PetSitting.Domain.Entities.Stripe;
using PetSitting.Domain.Enums;
using Stripe;
using Stripe.Events;

namespace PetSitting.Api.Controllers
{
    [Route("api/[controller]/")]
    public class StripeWebhookController : Controller
    {
        private readonly string _webhookSecret;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IServiceProvider _serviceProvider;
        public StripeWebhookController(IConfiguration configuration, IBackgroundTaskQueue backgroundTaskQueue,
            IServiceProvider serviceProvider)
        {
            _webhookSecret = configuration["Stripe:TestEnvironment:WebhookSignature"]!;
            _backgroundTaskQueue = backgroundTaskQueue ?? 
                throw new ArgumentNullException(nameof(backgroundTaskQueue));
            _serviceProvider = serviceProvider ?? 
                throw new ArgumentNullException(nameof(serviceProvider));
        }

        [HttpPost("index")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], _webhookSecret);

            _backgroundTaskQueue.Enqueue(async token => {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var stripeServices = scope.ServiceProvider.GetRequiredService<IStripeServices>();
                    await stripeServices.HandleWebHooks(stripeEvent);
                }
                catch(Exception ex)
                {
                    // Log the exception (you can use a logging framework here)
                    Console.WriteLine($"Error processing Stripe webhook: {ex.Message}");
                }
            });
            
            return Ok();
        }
    }
}