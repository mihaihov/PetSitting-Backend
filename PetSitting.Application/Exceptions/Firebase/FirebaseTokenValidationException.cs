namespace PetSitting.Application.Exceptions.Firebase
{
    public class FirebaseTokenValidationException : Exception
    {
        public FirebaseTokenValidationException() : base ("Firebase token validation failed!") {}
    }
}