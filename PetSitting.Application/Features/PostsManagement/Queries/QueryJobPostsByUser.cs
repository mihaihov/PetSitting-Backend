using MediatR;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.PostManagement;

namespace PetSitting.Application.Features.PostManagement.Queries
{
    public record QueryJobPostsByUser(string userId) : IRequest<QueryJobPostsByUserResponse>;

    public record QueryJobPostsByUserResponse : BaseResponse
    {
        public IEnumerable<JobPost>? PostsByUser {get;set;}
    }

    public class QueryJobPostsByUserHandler : BaseHandler<QueryJobPostsByUser,QueryJobPostsByUserResponse>
    {
        private readonly IBaseRepository<JobPost> _baseJobPostRepository;
        public QueryJobPostsByUserHandler(IBaseRepository<JobPost> baseJobPostRepository)
        {
            _baseJobPostRepository = baseJobPostRepository;
        }

        protected override async Task<QueryJobPostsByUserResponse> HandleCommand(QueryJobPostsByUser request, QueryJobPostsByUserResponse response, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.userId))
                throw new GenericValidationException("User id cannot is not valid!");

            var jobPosts = await _baseJobPostRepository.GetAllAsync();
            var jobPostsByUser = jobPosts.Where(jp => jp.AuthorId == request.userId);
            response.PostsByUser = jobPostsByUser;

            return response;
        }
    }
}