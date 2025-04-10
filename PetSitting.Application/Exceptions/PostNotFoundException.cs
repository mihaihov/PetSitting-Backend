namespace PetSitting.Application.Exceptions
{
    public class PostNotFoundException : Exception
    {
        public PostNotFoundException() 
            : base("Post was not found.") {}
    }
}