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

        [HttpPost("register", Name = "Register"), AllowAnonymous]
        public async Task<ActionResult<BaseResponse>> Register([FromBody] RegisterCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                
                if(result.ValidationErrors != null && result.ValidationErrors.Count != 0)
                {
                    return StatusCode(500, result.ValidationErrors);
                }
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new {message = ex.Message});
            }
        }

        [HttpPost("login", Name = "Login")]
        public async Task<ActionResult<LoginWithCredentialsCommandResponse>> Login([FromBody] LoginWithCredentialsCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                
                if(result.ValidationErrors != null && result.ValidationErrors.Count != 0)
                {
                    return StatusCode(500, result.ValidationErrors);
                }
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new {message = ex.Message});
            }
        }

        [HttpPost("sendverifiactionemail", Name ="SendVerificationEmail"), Authorize(Roles = "PetOwner, PetSitter, Admin")]
        public async Task<ActionResult<BaseResponse>> SendVerficationEmail([FromBody] UserManagementBaseCommand<BaseResponse> command)
        {
            try
            {
                var result = await _mediator.Send(command);
                if(result.ValidationErrors != null && result.ValidationErrors.Count !=0)
                {
                    return StatusCode(500, result.ValidationErrors);
                }
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new {message = ex.Message});
            }
        }

        [HttpPost("sendresetpasswordemail", Name = "SendResetPasswordEmail")]
        public async Task<ActionResult<BaseResponse>> SendResetPasswordEmail([FromBody]UserManagementBaseCommand<BaseResponse> command)
        {
            try
            {
                var result = await _mediator.Send(command);
                if(result.ValidationErrors != null && result.ValidationErrors.Count != 0)
                {
                    return StatusCode(500, result.ValidationErrors);
                }
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new {message = ex.Message});
            }
        }

        [HttpPost("resetpassword", Name = "ResetPassword")]
        public async Task<ActionResult<BaseResponse>> ResetPassword([FromBody]ResetPasswordCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                if (result.ValidationErrors != null && result.ValidationErrors.Count != 0)
                {
                    return StatusCode(500, result.ValidationErrors);
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