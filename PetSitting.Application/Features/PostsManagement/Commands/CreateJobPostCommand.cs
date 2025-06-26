using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.PostManagement.Validators;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.PostManagement;

namespace PetSitting.Application.Features.PostManagement.Commands
{
    public record CreateJobPostCommand(JobPost JobPost) : IRequest<BaseResponse>;

    public class CreateJobPostCommandHandler : BaseHandler<CreateJobPostCommand,BaseResponse,CreateJobPostCommandValidator>
    {
        private readonly IBaseRepository<JobPost> _jobPost;
        public CreateJobPostCommandHandler(IBaseRepository<JobPost> jobPost)
        {
            _jobPost = jobPost;
        }

        protected override async Task<BaseResponse> HandleCommand(CreateJobPostCommand request, BaseResponse response, CancellationToken cancellationToken)
        {
            await _jobPost.AddAsync(request.JobPost);

            return response;
        }
    }

}   