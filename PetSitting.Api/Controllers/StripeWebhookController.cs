using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Events;

namespace PetSitting.Api.Controllers
{
    [Route("api/[controller]/")]
    public class StripeWebhookController : Controller
    {
        private readonly StripeClient _client;
        private readonly string _webhookSecret;
        public StripeWebhookController(IConfiguration configuration)
        {
            var apiKey = StripeConfiguration.ApiKey = configuration["Stripe:TestEnvironment:SecretKey"];
            _client = new StripeClient(apiKey);
            _webhookSecret = configuration["Stripe:TestEnvironment:WebhookSignature"]!;
        }
        [HttpPost("index")]
        public async Task<IActionResult> Index()
        {
            Console.WriteLine("Webhook triggered");
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _webhookSecret);

                // Handle the event
                // If on SDK version < 46, use class Events instead of EventTypes
                if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    // Then define and call a method to handle the successful payment intent.
                    // handlePaymentIntentSucceeded(paymentIntent);
                }
                else if (stripeEvent.Type == EventTypes.PaymentMethodAttached)
                {
                    var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                    // Then define and call a method to handle the successful attachment of a PaymentMethod.
                    // handlePaymentMethodAttached(paymentMethod);
                }
                // ... handle other event types
                else
                {
                    // Unexpected event type
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
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