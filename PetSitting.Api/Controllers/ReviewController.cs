using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.ReviewSystem.Commands;

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
    }
}