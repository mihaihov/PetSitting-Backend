using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.Messaging.Queries;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Messaging;

namespace PetSitting.Api.Controllers
{
    [Route("api/[controller]/")]
    public class MessagingController : BaseController
    {
        private IMessageRepository _messageRepository;
        public MessagingController(IMediator mediator, IMessageRepository messageRepository) : base(mediator) 
        {
            _messageRepository = messageRepository;
        }

        [HttpGet("getbyid")]
        public async Task<ActionResult<Message>> GetByIdAsync([FromQuery]string id)
        {
            if(string.IsNullOrEmpty(id))    throw new GenericValidationException("Invalid parameters.");

            var response = await _messageRepository.GetByIdAsync(id);
            if(response == null) throw new InternalMessageNotFoundException();

            return response;
        }

        [HttpGet("getlatestnmessagessentbyuser")]
        public Task<ActionResult<GetLatestNMessagesSentCommandResponse>> GetLatestNMessagesSentByUser(
            [FromBody]GetLatestNMessagesSentCommand command) => HandleRequest<GetLatestNMessagesSentCommand,GetLatestNMessagesSentCommandResponse>(command);
        
        [HttpGet("getlatestnmessagesreceivedbyuser")]
        public Task<ActionResult<GetLatestNMessagesReceivedCommandResponse>> GetLatestNMessagesReceivedByUser(
            [FromBody]GetLatestNMessagesReceivedCommand command) => HandleRequest<GetLatestNMessagesReceivedCommand,GetLatestNMessagesReceivedCommandResponse>(command);

        [HttpGet("getusermessagesbydate")]
        public Task<ActionResult<GetUserMessagesByDateCommandResponse>> GetUserMessagesByDate(
            [FromBody]GetUserMessagesByDateCommand command) => HandleRequest<GetUserMessagesByDateCommand,GetUserMessagesByDateCommandResponse>(command);
    }
}