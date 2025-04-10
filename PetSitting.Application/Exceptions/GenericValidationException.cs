namespace PetSitting.Application.Exceptions
{
    public class GenericValidationException : Exception
    {
        public GenericValidationException(string message) : base (message) { }
    }
}