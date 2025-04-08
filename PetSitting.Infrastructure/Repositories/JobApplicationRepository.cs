using System.Globalization;
using Microsoft.EntityFrameworkCore;
using PetSitting.Application.Features.PostManagement.Validators;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.PostManagement;

namespace PetSitting.Infrastructure.Repositories
{
    public class JobApplicationRepository : BaseRepository<JobApplication>, IJobApplicationRepository
    {
        public JobApplicationRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<bool> Exists(string jobPostId, string applicantId)
        {
            var jobApplication = await _dbContext.JobApplications.Where(ja => ja.ApplicantId == applicantId && ja.JobPostId == jobPostId).FirstOrDefaultAsync();
            return jobApplication == null ? false : true;
        }

        public async Task<IReadOnlyList<JobApplication>> GetAllJobApplicationsForAJobPost(string jobPostId)
        {
            return await _dbContext.JobApplications.Where(ja => ja.JobPostId == jobPostId).ToListAsync();
        }
    }
}