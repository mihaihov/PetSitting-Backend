using Google.Apis.Util;
using MediatR;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Messaging;

namespace PetSitting.Application.Features.Messaging.Queries
{
    public record GetLatestNMessagesSentCommand(string userId, int n) : IRequest<GetLatestNMessagesSentCommandResponse>;
    public record GetLatestNMessagesSentCommandResponse : BaseResponse
    {
        public ICollection<Message>? Messages {get;set;}
    }
    public class GetLatestNMessagesSentCommandHandler : BaseHandler<GetLatestNMessagesSentCommand, GetLatestNMessagesSentCommandResponse>
    {
        private readonly IMessageRepository _messageRepository;
        public GetLatestNMessagesSentCommandHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        protected override async Task<GetLatestNMessagesSentCommandResponse> HandleCommand(GetLatestNMessagesSentCommand request, GetLatestNMessagesSentCommandResponse response, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.userId) || request.n <= 0)
                throw new GenericValidationException("Invalid parameters!");

            response.Messages = await _messageRepository.GetLatestNMessagesSentByUser(request.userId,request.n);
            return response;
        }
    }
}