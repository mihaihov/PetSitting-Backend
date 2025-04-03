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
    public class UserController : BaseController
    {        
        public UserController(IMediator mediator) : base(mediator) { }

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

    }
}