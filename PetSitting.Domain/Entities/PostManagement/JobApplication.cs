using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Domain.Entities.PostManagement
{
    public class JobApplication
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string JobPostId { get; set; }
        public required JobPost JobPost { get; set; }
        public required string ApplicantId { get; set; }
        public required ApplicationUser Applicant { get; set; }
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    }

}