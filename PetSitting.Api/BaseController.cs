using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetSitting.Application.Features.Common;

namespace PetSitting.Api
{
    public abstract class BaseController : Controller
    {
        private readonly IMediator _mediator;
        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected async Task<ActionResult<TResponse>> HandleRequest<TRequest, TResponse>(TRequest command)
    where TRequest : IRequest<TResponse>
        {
            var result = await _mediator.Send(command);

            if (result is BaseResponse baseResponse && baseResponse.ValidationErrors?.Count > 0)
            {
                return StatusCode(400, baseResponse.ValidationErrors);
            }

            return Ok(result);
        }
    }
}