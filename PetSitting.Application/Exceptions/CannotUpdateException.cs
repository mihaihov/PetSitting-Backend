namespace PetSitting.Application.Exceptions
{
    public class CannotUpdateException : Exception
    {
        public CannotUpdateException() : base("You are not allowed to update the review anymore.") {}
    }
}