using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.PostManagement.Validators;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.PostManagement;

namespace PetSitting.Application.Features.PostManagement.Commands
{
    public record UpdateJobOfferStatusCommand(string id, JobApplicationStatus status) : IRequest<BaseResponse>;

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
                var jobApplication = await _jobApplicationRepository.GetByIdAsync(request.id);
                if(jobApplication == null)
                    throw new Exception("Job application does not exists!");
                
                jobApplication.Status = request.status;
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