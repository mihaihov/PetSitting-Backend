namespace PetSitting.Application.Exceptions
{
    public class StripeWebhookNotHandledException : Exception
    {
        public StripeWebhookNotHandledException() : base("Webhook not handled") { }
    }
}