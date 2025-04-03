using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.PostManagement.Validators;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.PostManagement;

namespace PetSitting.Application.Features.PostManagement.Commands
{
    public record UpdateJobPostCommand(string id, string? description, ICollection<Media>? medias, string? title,
        string? location, DateTime? startDate, DateTime? endDate, decimal? payment, bool? isOpen) : IRequest<BaseResponse>;

    public class UpdatejobPostCommandHandler : BaseHandler<UpdateJobPostCommand, BaseResponse, UpdateJobPostCommandValidator>
    {
        private readonly IBaseRepository<JobPost> _jobPostRepository;
        public UpdatejobPostCommandHandler(IBaseRepository<JobPost> jobPostRespository)
        {
            _jobPostRepository = jobPostRespository;
        }
        protected override async Task<BaseResponse> HandleCommand(UpdateJobPostCommand request, BaseResponse response, CancellationToken cancellationToken)
        {
            try
            {
                var jobPost = await _jobPostRepository.GetByIdAsync(request.id);
                if(jobPost == null)
                    throw new Exception("Post not found!");

                if(request.description is not null) jobPost.Description = request.description;
                if(request.medias is not null) jobPost.MediaFiles = request.medias;
                if(request.title is not null) jobPost.Title = request.title;
                if(request.location is not null) jobPost.Location = request.location;
                if(request.startDate is not null) jobPost.StartDate = (DateTime)request.startDate;
                if(request.endDate is not null) jobPost.EndDate = (DateTime)request.endDate;
                if(request.payment is not null) jobPost.Payment = request.payment;
                if(request.isOpen is not null) jobPost.IsOpen = (bool)request.isOpen;

                _jobPostRepository.Update(jobPost);

                return response;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}