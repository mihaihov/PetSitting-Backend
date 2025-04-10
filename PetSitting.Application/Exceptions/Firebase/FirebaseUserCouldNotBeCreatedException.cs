namespace PetSitting.Application.Exceptions.Firebase
{
    public class FirebaseUserCannotBeCreatedException : Exception
    {
        public FirebaseUserCannotBeCreatedException() : base ("Firebase user could not be created.") {}
    }
}