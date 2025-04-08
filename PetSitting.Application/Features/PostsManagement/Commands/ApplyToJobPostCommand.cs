using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.PostManagement.Validators;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.PostManagement;

namespace PetSitting.Application.Features.PostManagement.Commands
{
    public record ApplyToJobPostCommand(string jobPostId, string applicantId) : IRequest<BaseResponse>;

    public class ApplyToJobPostCommandHandler : BaseHandler<ApplyToJobPostCommand,BaseResponse,ApplyToJobPostCommandValidator>
    {
        private readonly IUserRepository _userRepository;
        private readonly IBaseRepository<Post> _postRepository;
        private readonly IJobApplicationRepository _applicationsRepository;
        public ApplyToJobPostCommandHandler(IUserRepository userRepository, IBaseRepository<Post> postRepository, IJobApplicationRepository applicationRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _applicationsRepository = applicationRepository;
        }
        protected override async Task<BaseResponse> HandleCommand(ApplyToJobPostCommand request, BaseResponse response, CancellationToken cancellationToken)
        {
            try
            {
                if(string.IsNullOrEmpty(request.applicantId) || string.IsNullOrEmpty(request.jobPostId))
                    throw new Exception("Bad request!");

                var user = await _userRepository.GetByIdAsync(request.applicantId);
                if(user == null)
                    throw new Exception("User not found!");

                var post = await _postRepository.GetByIdAsync(request.jobPostId);
                if(post == null)
                    throw new Exception("Post not found");

                if(await _applicationsRepository.Exists(request.jobPostId,request.applicantId))
                    throw new Exception("You've already applied to this job!");

                JobApplication ja = JobApplication.Create(request.jobPostId, request.applicantId);

                await _applicationsRepository.AddAsync(ja);

                return response;
            }
            catch(Exception)
            {
                throw;
            }

        }
    }
}