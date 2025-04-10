using MediatR;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;

namespace PetSitting.Application.Features.PostManagement.Commands
{
    public record UpdateJobApplicationCommand(string jobApplicationId, string description) : IRequest<BaseResponse>;

    public class UpdateJobApplicationCommandHandler : BaseHandler<UpdateJobApplicationCommand, BaseResponse>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        public UpdateJobApplicationCommandHandler(IJobApplicationRepository jobApplicationRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
        }
        protected override async Task<BaseResponse> HandleCommand(UpdateJobApplicationCommand request, BaseResponse response, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.jobApplicationId))
                throw new GenericValidationException("Invalid jobpost application!");
            if (string.IsNullOrEmpty(request.description))
                throw new GenericValidationException("Invalid description!");

            var jobApplication = await _jobApplicationRepository.GetByIdAsync(request.jobApplicationId);
            if (jobApplication == null)
                throw new JobApplicationNotFoundException();

            jobApplication.Description = request.description;
            await _jobApplicationRepository.Update(jobApplication);
            return response;
        }
    }
}