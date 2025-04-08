using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.PostManagement.Validators;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.PostManagement;

namespace PetSitting.Application.Features.PostManagement.Commands
{
    public record UpdateJobOfferStatusCommand(string jobApplicationId, JobApplicationStatus status) : IRequest<BaseResponse>;

    public class UpdateJobOfferStatusCommandHandler : BaseHandler<UpdateJobOfferStatusCommand,BaseResponse,UpdateJobOfferStatusCommandValidator>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        public UpdateJobOfferStatusCommandHandler(IJobApplicationRepository jobApplicationRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
        }

        protected override async Task<BaseResponse> HandleCommand(UpdateJobOfferStatusCommand request, BaseResponse response, CancellationToken cancellationToken)
        {
            try
            {
                var jobApplication = await _jobApplicationRepository.GetByIdAsync(request.jobApplicationId);
                if(jobApplication == null)
                    throw new Exception("Job application does not exists!");

                var allJobOffers = await _jobApplicationRepository.GetAllJobApplicationsForAJobPost(jobApplication.JobPostId);
                var approvedJobOffers = allJobOffers.Where(jo => jo.Status == JobApplicationStatus.Approved).FirstOrDefault();
                if(approvedJobOffers != null)
                    throw new Exception("You've already approved an offer for this Job!");
                
                jobApplication.UpdateStatus(request.status);
                _jobApplicationRepository.Update(jobApplication);

                return response;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}