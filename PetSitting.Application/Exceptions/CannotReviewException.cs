namespace PetSitting.Application.Exceptions
{
    public class CannotReviewException : Exception
    {
        public CannotReviewException(string message) : base(message) {}
    }
}