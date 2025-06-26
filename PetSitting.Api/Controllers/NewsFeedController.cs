using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSitting.Application.Interfaces.Services;
using PetSitting.Domain.Entities.PostManagement;

namespace PetSitting.Api.Controllers
{
    [Route("api/[controller]/")]
    public class NewsFeedController : Controller
    {
        private readonly IFanoutService<Post, string> _fanoutService;
        public NewsFeedController(IFanoutService<Post, string> fanoutService)
        {
            _fanoutService = fanoutService;
        }

        [HttpPost("posttonewsfeed"), Authorize(Roles = "PetOwner,Admin")]
        public async Task<ActionResult> PostToNewsFeed(Post post)
        {
            try
            {
                await _fanoutService.Publish(post);
                return Ok();
            }
            catch
            {
                throw;
            }
        }

        [HttpPost("retrievenewsfeed"), Authorize(Roles = "PetOwner,PetSitter,Admin")]
        public async Task<ActionResult<IReadOnlyList<Post>>> RetrieveNewsFeed(string userId)
        {
            try
            {
                return Ok(await _fanoutService.Retrieve(userId));
            }
            catch
            {
                throw;
            }
        }

    }
}