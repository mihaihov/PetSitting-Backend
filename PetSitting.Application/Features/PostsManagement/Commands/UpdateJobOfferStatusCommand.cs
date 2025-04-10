using MediatR;
using PetSitting.Application.Exceptions;
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
            var jobApplication = await _jobApplicationRepository.GetByIdAsync(request.jobApplicationId);
            if (jobApplication == null)
                throw new JobApplicationNotFoundException();

            if (request.status == JobApplicationStatus.Approved)
            {
                var allJobOffers = await _jobApplicationRepository.GetAllJobApplicationsForAJobPost(jobApplication.JobPostId);
                var approvedJobOffers = allJobOffers.Where(jo => jo.Status == JobApplicationStatus.Approved).FirstOrDefault();
                if (approvedJobOffers != null)
                    throw new JobApplicationAlreadyAcceptedException();
            }

            jobApplication.UpdateStatus(request.status);
            await _jobApplicationRepository.Update(jobApplication);

            return response;
        }
    }
}