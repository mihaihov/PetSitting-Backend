using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.PostManagement;

namespace PetSitting.Application.Features.PostManagement.Queries
{
    public record GetJobPostsByUserQuery(string userId) : IRequest<GetJobPostsByUserQueryResponse>;

    public record GetJobPostsByUserQueryResponse : BaseResponse
    {
        public IEnumerable<JobPost>? PostsByUser {get;set;}
    }

    public class GetJobPostsByUserQueryHandler : BaseHandler<GetJobPostsByUserQuery,GetJobPostsByUserQueryResponse>
    {
        private readonly IBaseRepository<JobPost> _baseJobPostRepository;
        public GetJobPostsByUserQueryHandler(IBaseRepository<JobPost> baseJobPostRepository)
        {
            _baseJobPostRepository = baseJobPostRepository;
        }

        protected override async Task<GetJobPostsByUserQueryResponse> HandleCommand(GetJobPostsByUserQuery request, GetJobPostsByUserQueryResponse response, CancellationToken cancellationToken)
        {
            try
            {
                if(string.IsNullOrEmpty(request.userId))
                    throw new Exception("User id cannot be null or empty!");

                var jobPosts = await _baseJobPostRepository.GetAllAsync();
                var jobPostsByUser = jobPosts.Where(jp => jp.AuthorId == request.userId);
                response.PostsByUser = jobPostsByUser;

                return response;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}