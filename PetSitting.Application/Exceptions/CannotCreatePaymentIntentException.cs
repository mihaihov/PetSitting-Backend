namespace PetSitting.Application.Exceptions
{
    public class CannotCreatePaymentIntentException : Exception
    {
        public CannotCreatePaymentIntentException() 
            : base("An error occured while trying to create Stripe Payment Intent.")
        {
            
        }
    }
}