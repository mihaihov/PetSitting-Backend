namespace PetSitting.Application.Exceptions
{
    public class InternalMessageNotFoundException : Exception
    {
        public InternalMessageNotFoundException() : base("Message was not found.")
        {
            
        }
    }
}