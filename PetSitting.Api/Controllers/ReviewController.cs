using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.ReviewSystem.Commands;
using PetSitting.Application.Features.ReviewSystem.Queries;

namespace PetSitting.Api.Controllers
{
    [Route("api/[controller]/")]
    public class ReviewController : BaseController
    {
        public ReviewController(IMediator mediator) : base (mediator){}

        [HttpPost("leavereview")]
        public Task<ActionResult<BaseResponse>> LeaveReview([FromBody]LeaveReviewCommand command) =>
            HandleRequest<LeaveReviewCommand,BaseResponse>(command);

        [HttpPut("updatereview")]
        public Task<ActionResult<BaseResponse>> UpdateReview([FromBody]UpdateReviewCommand command) =>
            HandleRequest<UpdateReviewCommand,BaseResponse>(command);

        [HttpDelete("deletereview")]
        public Task<ActionResult<BaseResponse>> DeleteReview([FromQuery]DeleteReviewCommand command) =>
            HandleRequest<DeleteReviewCommand,BaseResponse>(command);

        [HttpGet("getreviewsbypost")]
        public Task<ActionResult<QueryReviewsByPostResponse>> GetReviewsByPost([FromQuery]QueryReviewsByPost query) =>
            HandleRequest<QueryReviewsByPost,QueryReviewsByPostResponse>(query);


        [HttpGet("getreviewsbyuser")]
        public Task<ActionResult<QueryReviewsByUserResponse>> GetReviewsByUser([FromQuery]QueryReviewsByUser query) =>
            HandleRequest<QueryReviewsByUser,QueryReviewsByUserResponse>(query);
    }
}