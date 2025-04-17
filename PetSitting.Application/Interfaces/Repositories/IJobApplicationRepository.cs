using PetSitting.Domain.Entities.PostManagement;

namespace PetSitting.Application.Interfaces.Repositories
{
    public interface IJobApplicationRepository : IBaseRepository<JobApplication>
    {
        public Task<bool> Exists(string jobPostId, string applicantId);
        public Task<IReadOnlyList<JobApplication>> GetAllJobApplicationsForAJobPost(string jobPostId);
    }
}