namespace PetSitting.Application.Exceptions.Firebase
{
    public class FirebaseLoginFailedException : Exception
    {
        public FirebaseLoginFailedException() : base ("Firebase login failed!") {}
    }
}