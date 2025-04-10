namespace PetSitting.Application.Exceptions
{
    public class InternalUserNotFoundException : Exception
    {
        public InternalUserNotFoundException() 
            : base("User was not found in the internal database.") {}
    }
}