using System.Reflection.Metadata.Ecma335;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.NewsFeed.Commands;
using PetSitting.Application.Features.NewsFeed.Queries;
using PetSitting.Application.Features.PostManagement.Commands;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;
using PetSitting.Domain.Entities.PostManagement;

namespace PetSitting.Infrastructure.Services
{
    public class FeedFanoutService : IFanoutService<JobPost,string>
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        public FeedFanoutService(IMediator mediator, IUserRepository userRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
        }
        public async Task Publish(JobPost? entity)
        {
            if (entity == null)
                throw new GenericValidationException("Invalid data.");

            await _mediator.Send(new CreateJobPostCommand(entity));     //ads post to Post table in DB

            var users = _userRepository.BaseQuery();
            var nearbyUsers = await users
                .Include(u => u.UserProfile)
                .Where(u => u.UserProfile!.Location == entity.Location)
                .Select(u => u.Id)
                .ToListAsync();

            foreach (var user in nearbyUsers)
            {
                await _mediator.Send(new UpdateNewsFeedCommand(user, entity.Id)); //creates entry in the news feed based on the post.
            }
        }

        public async Task<IReadOnlyList<JobPost>?> Retrieve(string retrieveBy)
        {
            List<JobPost>? posts = (await _mediator.Send(new QueryNewsFeedByUser(retrieveBy)))?.Posts;
            posts?.Sort((a, b) => Convert.ToInt32(a.Payment > b.Payment));
            posts?.Sort((a, b) => Convert.ToInt32(a.Applications.Count > b.Applications.Count ));
            posts?.Sort((a, b) => Convert.ToInt32(a.CreatedAt > b.CreatedAt));

            return posts;

        }
    }
}