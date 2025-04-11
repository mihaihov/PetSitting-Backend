using MediatR;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Messaging;

namespace PetSitting.Application.Features.Messaging.Queries
{
    public record GetUserMessagesByDateCommand(string userId, DateTime timeStamp) : IRequest<GetUserMessagesByDateCommandResponse>;
    public record GetUserMessagesByDateCommandResponse : BaseResponse
    {
        public ICollection<Message>? Messages {get;set;}
    }

    public class GetUserMessagesByDateCommandHandler : BaseHandler<GetUserMessagesByDateCommand, GetUserMessagesByDateCommandResponse>
    {
        private readonly IMessageRepository _messageRepository;
        public GetUserMessagesByDateCommandHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        protected override async Task<GetUserMessagesByDateCommandResponse> HandleCommand(GetUserMessagesByDateCommand request, GetUserMessagesByDateCommandResponse response, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.userId))
                throw new GenericValidationException("Invalid parameters.");

            response.Messages = await _messageRepository.GetUserMessagesByDate(request.userId, request.timeStamp);
            return response;
        }
    }
}