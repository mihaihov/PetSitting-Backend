namespace PetSitting.Application.Exceptions
{
    public class JobApplicationAlreadyExistsException : Exception
    {
        public JobApplicationAlreadyExistsException() 
            : base("This user already applied for this job.") {}
    }
}