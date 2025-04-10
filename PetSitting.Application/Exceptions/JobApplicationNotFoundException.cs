namespace PetSitting.Application.Exceptions
{
    public class JobApplicationNotFoundException : Exception
    {
        public JobApplicationNotFoundException() 
            : base("Job application was not found.") {}
    }
}