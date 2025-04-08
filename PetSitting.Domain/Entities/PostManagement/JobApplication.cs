using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Domain.Entities.PostManagement
{
    public class JobApplication
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string JobPostId { get; set; }
#pragma warning disable
        public JobPost JobPost { get; set; }
        public required string ApplicantId { get; set; }
        public ApplicationUser? Applicant { get; set; }
#pragma warning restore
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    }

}