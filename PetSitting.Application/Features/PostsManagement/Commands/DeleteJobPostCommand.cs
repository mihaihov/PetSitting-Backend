using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.PostManagement;

namespace PetSitting.Application.Features.PostManagement.Commands
{
    public record DeleteJobPostCommand(string id) : IRequest<BaseResponse>;

    public class DeleteJobPostCommandHandler : BaseHandler<DeleteJobPostCommand, BaseResponse>
    {
        private readonly IBaseRepository<JobPost> _jobPostRepository;
        public DeleteJobPostCommandHandler(IBaseRepository<JobPost> jobPostRepository)
        {
            _jobPostRepository = jobPostRepository;
        }
        protected override async Task<BaseResponse> HandleCommand(DeleteJobPostCommand request, BaseResponse response, CancellationToken cancellationToken)
        {
            try
            {
                if(string.IsNullOrEmpty(request.id))
                    throw new Exception("Id is expected!");
                var jobPost = await _jobPostRepository.GetByIdAsync(request.id);
                if(jobPost == null)
                    throw new Exception("Job post not found!");
                
                await _jobPostRepository.Delete(jobPost);
                return response;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}