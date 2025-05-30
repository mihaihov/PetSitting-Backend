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
        private IChatSessionRepository _chatSessionRepository;
        public MessagingController(IMediator mediator, IMessageRepository messageRepository, IChatSessionRepository chatSessionRepository) : base(mediator) 
        {
            _messageRepository = messageRepository;
            _chatSessionRepository = chatSessionRepository;
        }

        [HttpGet("getmessagebyid")]
        public async Task<ActionResult<Message>> GetMessage([FromQuery]string id)
        {
            if(string.IsNullOrEmpty(id))    throw new GenericValidationException("Invalid parameters.");

            var response = await _messageRepository.GetByIdAsync(id);
            if(response == null) throw new InternalMessageNotFoundException();

            return response;
        }

        [HttpGet("getlatestnmessagessentbyuser")]
        public Task<ActionResult<QueryLatestNMessagesSentResponse>> GetLatestNMessagesSentByUser(
            [FromBody]QueryLatestNMessagesSent command) => HandleRequest<QueryLatestNMessagesSent,QueryLatestNMessagesSentResponse>(command);
        
        [HttpGet("getlatestnmessagesreceivedbyuser")]
        public Task<ActionResult<QueryLatestNMessagesReceivedResponse>> GetLatestNMessagesReceivedByUser(
            [FromBody]QueryLatestNMessagesReceived command) => HandleRequest<QueryLatestNMessagesReceived,QueryLatestNMessagesReceivedResponse>(command);

        [HttpGet("getusermessagesbydate")]
        public Task<ActionResult<QueryUserMessagesByDateResponse>> GetUserMessagesByDate(
            [FromBody]QueryUserMessagesByDate command) => HandleRequest<QueryUserMessagesByDate,QueryUserMessagesByDateResponse>(command);

        [HttpGet("getchatsessionbyid")]
        public async Task<ActionResult<ChatSession>> GetChatSession([FromQuery]string id)
        {
            if(string.IsNullOrEmpty(id))    throw new GenericValidationException("Invalid parameters.");

            var response = await _chatSessionRepository.GetByIdAsync(id);
            if(response == null) throw new InternalMessageNotFoundException();

            return response;
        }

        [HttpGet("getchatsessionbyuserid")]
        public async Task<ICollection<ChatSession>?> GetChatSessionByUser([FromQuery]string userId)
        {
            if(string.IsNullOrEmpty(userId))    throw new GenericValidationException("Invalid parameters.");

            var response = await _chatSessionRepository.GetByUserAsync(userId);
            if(response == null) throw new InternalMessageNotFoundException();

            return response;
        }
    }
}