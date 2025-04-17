namespace PetSitting.Application.Exceptions
{
    public class ReviewNotFoundException : Exception
    {
        public ReviewNotFoundException() : base("Review not found.") {}
    }
}