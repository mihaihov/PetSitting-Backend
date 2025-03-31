using Microsoft.AspNetCore.Mvc;
using MediatR;
using PetSitting.Application.Features.UserManagement.Commands;
using PetSitting.Application.Features.UserManagement;
using Microsoft.AspNetCore.Authorization;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;

namespace PetSitting.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register"), AllowAnonymous]
        public Task<ActionResult<BaseResponse>> Register([FromBody] RegisterCommand command) => 
            HandleRequest<RegisterCommand,BaseResponse>(command);

        [HttpPost("login")]
        public Task<ActionResult<LoginWithCredentialsCommandResponse>> Login([FromBody] LoginWithCredentialsCommand command) => 
            HandleRequest<LoginWithCredentialsCommand,LoginWithCredentialsCommandResponse>(command);

        [HttpPost("sendverifiactionemail"), Authorize(Roles = "PetOwner, PetSitter, Admin")]
        public Task<ActionResult<BaseResponse>> SendVerficationEmail([FromBody] UserManagementBaseCommand<BaseResponse> command) =>
            HandleRequest<UserManagementBaseCommand<BaseResponse>,BaseResponse>(command);


        [HttpPost("sendresetpasswordemail", Name = "SendResetPasswordEmail")]
        public Task<ActionResult<BaseResponse>> SendResetPasswordEmail([FromBody]UserManagementBaseCommand<BaseResponse> command) =>
            HandleRequest<UserManagementBaseCommand<BaseResponse>, BaseResponse>(command);

        [HttpPost("resetpassword", Name = "ResetPassword")]
        public Task<ActionResult<BaseResponse>> ResetPassword([FromBody]ResetPasswordCommand command) => 
            HandleRequest<ResetPasswordCommand,BaseResponse>(command);


        private async Task<ActionResult<TResponse>> HandleRequest<TRequest, TResponse>(TRequest command)
    where TRequest : IRequest<TResponse>
        {
            try
            {
                var result = await _mediator.Send(command);

                if (result is BaseResponse baseResponse && baseResponse.ValidationErrors?.Count > 0)
                {
                    return StatusCode(500, baseResponse.ValidationErrors);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

    }
}