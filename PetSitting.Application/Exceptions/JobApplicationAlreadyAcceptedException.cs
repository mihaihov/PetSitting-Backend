namespace PetSitting.Application.Exceptions
{
    public class JobApplicationAlreadyAcceptedException : Exception
    {
        public JobApplicationAlreadyAcceptedException() 
            : base("You have already accepted a job offer for this job.") {}
    }
}