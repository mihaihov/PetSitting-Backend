using Microsoft.AspNetCore.Mvc;
using MediatR;
using PetSitting.Application.Features.UserManagement.Commands;
using PetSitting.Application.Features.UserManagement;
using Microsoft.AspNetCore.Authorization;

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

        [HttpPost("register", Name = "Register"), Authorize(Roles = "PetOwner")]
        public async Task<ActionResult<RegisterCommandResponse>> Register([FromBody] RegisterCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
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
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new {message = ex.Message});
            }
        }
    }
}