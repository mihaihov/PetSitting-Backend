using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.PostManagement.Validators;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.PostManagement;

namespace PetSitting.Application.Features.PostManagement.Commands
{
    public record CreateJobPostCommand(string? description, string authorId, ICollection<Media>? medias,
        string? title, string location, DateTime startDate, DateTime endDate, decimal? payment) : IRequest<BaseResponse>;

    public class CreateJobPostCommandHandler : BaseHandler<CreateJobPostCommand,BaseResponse,CreateJobPostCommandValidator>
    {
        private readonly IBaseRepository<JobPost> _jobPost;
        public CreateJobPostCommandHandler(IBaseRepository<JobPost> jobPost)
        {
            _jobPost = jobPost;
        }

        protected override async Task<BaseResponse> HandleCommand(CreateJobPostCommand request, BaseResponse response, CancellationToken cancellationToken)
        {
            try
            {
                JobPost jobPost = new JobPost
                {
                    Description = request.description,
                    AuthorId = request.authorId,
                    MediaFiles = request.medias,
                    Title = request.title,
                    Location = request.location,
                    StartDate = request.startDate,
                    EndDate = request.endDate,
                    Payment = request.payment
                };
                
                await _jobPost.AddAsync(jobPost);

                return response;
            }
            catch(Exception)
            {
                throw;
            }


        }
    }

}   