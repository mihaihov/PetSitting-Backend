using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Domain.Entities.PostManagement
{
    public class JobApplication
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? Title {get;set;}
        public required string Description {get;set;}
        public required string JobPostId { get; set; }
#pragma warning disable
        public JobPost JobPost { get; set; }
        public required string ApplicantId { get; set; }
        public ApplicationUser? Applicant { get; set; }
#pragma warning restore
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
        public JobApplicationStatus Status {get;set;} = JobApplicationStatus.Pending;

        public void UpdateStatus(JobApplicationStatus newStatus)
        {
            if(IsValidStatusTransition(Status,newStatus))
                Status = newStatus;
            else throw new Exception("Invalid state transition!");
        }

        private bool IsValidStatusTransition(JobApplicationStatus current, JobApplicationStatus newStatus)
        {
            return current switch
            {
                JobApplicationStatus.Pending => newStatus != JobApplicationStatus.Pending,
                JobApplicationStatus.Approved => false,
                JobApplicationStatus.Rejected => false,
                _ => false
            };
        }

        public static JobApplication Create(string jobPostId, string applicantId, string? title, string description)
        {
            return new JobApplication {
                    JobPostId = jobPostId,
                    ApplicantId = applicantId,
                    Title = title,
                    Description = description
                };
        }

    }

    public enum JobApplicationStatus
    {
        Pending,
        Approved,
        Rejected
    }

}