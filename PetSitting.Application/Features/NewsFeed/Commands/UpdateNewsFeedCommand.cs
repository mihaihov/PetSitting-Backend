using MediatR;
using Microsoft.EntityFrameworkCore;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.NewsFeed.Validators;
using PetSitting.Application.Features.PostManagement.Commands;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.NewsFeed;
using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Domain.Entities.UserManagement;
using Stripe.TestHelpers.Terminal;

namespace PetSitting.Application.Features.NewsFeed.Commands
{
    public record UpdateNewsFeedCommand(
        string userId,
        string postId    
    ) : IRequest<BaseResponse>;

    public class UpdateNewsFeedCommandHandler : BaseHandler<UpdateNewsFeedCommand,BaseResponse,UpdateNewsFeedCommandValidator>
    {
        private readonly INewsFeedRepository _newsFeedRepository;
        public UpdateNewsFeedCommandHandler(INewsFeedRepository newsFeedRepository)
        {
            _newsFeedRepository = newsFeedRepository;
        }

        protected override async Task<BaseResponse> HandleCommand(UpdateNewsFeedCommand request, BaseResponse response, CancellationToken cancellationToken)
        {
            await _newsFeedRepository.AddAsync(new Feed
            {
                Id = string.Concat(request.userId, request.postId),
                UserId = request.userId,
                PostId = request.postId
            });

            return response;
        }
    }
}