using FirebaseAdmin.Messaging;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.PostManagement.Commands;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Domain.Enums;


namespace PetSitting.Api.Controllers
{
    [Route("api/[controller]/")]
    public class PostController : BaseController
    {
        private readonly IBaseRepository<JobPost> _jobPostRepository;
        public PostController(IMediator mediator, IBaseRepository<JobPost> jobPostRepository) : base(mediator) 
        { 
            _jobPostRepository = jobPostRepository;
        }

        [HttpPost("addpost")]
        public Task<ActionResult<BaseResponse>> AddPost([FromBody] CreateJobPostCommand command) =>
            HandleRequest<CreateJobPostCommand,BaseResponse>(command);

        [HttpPut("updatepost")]
        public Task<ActionResult<BaseResponse>> UpdatePost([FromBody] UpdateJobPostCommand command) =>
            HandleRequest<UpdateJobPostCommand,BaseResponse>(command);

        [HttpDelete("deletepost")]
        public Task<ActionResult<BaseResponse>> DeletePost([FromBody] DeleteJobPostCommand command) =>
            HandleRequest<DeleteJobPostCommand,BaseResponse>(command);

        [HttpGet("getpostbyid")]
        public async Task<ActionResult<JobPost?>> GetPostById([FromQuery]string id)
        {
            try
            {
                if(id == null)
                    throw new Exception("Id cannot be null");
                var jobpost = await _jobPostRepository.GetByIdAsync(id);
                
                return jobpost;
            }
            catch(Exception ex)
            {
                return StatusCode(500, new {message = ex.Message});
            }
        }

        [HttpPost("applytojobpost"), Authorize(Roles = "PetSitter,Admin")]
        public Task<ActionResult<BaseResponse>> ApplyToJob([FromBody]ApplyToJobPostCommand command) =>
            HandleRequest<ApplyToJobPostCommand,BaseResponse>(command);
    }
}