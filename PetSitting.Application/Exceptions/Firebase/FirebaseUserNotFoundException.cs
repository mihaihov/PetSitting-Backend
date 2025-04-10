namespace PetSitting.Application.Exceptions.Firebase
{
    public class FirebaseUserNotFoundException : Exception
    {
        public FirebaseUserNotFoundException() : base ("Firebase user not found.") {}
    }
}