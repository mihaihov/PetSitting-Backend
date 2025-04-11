using Google.Apis.Util;
using MediatR;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Messaging;

namespace PetSitting.Application.Features.Messaging.Queries
{
    public record GetLatestNMessagesReceivedCommand(string userId, int n) : IRequest<GetLatestNMessagesReceivedCommandResponse>;
    public record GetLatestNMessagesReceivedCommandResponse : BaseResponse
    {
        public ICollection<Message>? Messages {get;set;}
    }
    public class GetLatestNMessagesReceivedCommandHandler : BaseHandler<GetLatestNMessagesReceivedCommand, GetLatestNMessagesReceivedCommandResponse>
    {
        private readonly IMessageRepository _messageRepository;
        public GetLatestNMessagesReceivedCommandHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        protected override async Task<GetLatestNMessagesReceivedCommandResponse> HandleCommand(GetLatestNMessagesReceivedCommand request, GetLatestNMessagesReceivedCommandResponse response, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.userId) || request.n <= 0)
                throw new GenericValidationException("Invalid parameters!");

            response.Messages = await _messageRepository.GetLatestNMessagesReceivedByUser(request.userId,request.n);
            return response;
        }
    }
}