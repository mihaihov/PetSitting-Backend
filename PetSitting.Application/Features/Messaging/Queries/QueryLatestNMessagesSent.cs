using Google.Apis.Util;
using MediatR;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Messaging;

namespace PetSitting.Application.Features.Messaging.Queries
{
    public record QueryLatestNMessagesSent(string userId, int n) : IRequest<QueryLatestNMessagesSentResponse>;
    public record QueryLatestNMessagesSentResponse : BaseResponse
    {
        public ICollection<Message>? Messages {get;set;}
    }
    public class QueryLatestNMessagesSentHandler : BaseHandler<QueryLatestNMessagesSent, QueryLatestNMessagesSentResponse>
    {
        private readonly IMessageRepository _messageRepository;
        public QueryLatestNMessagesSentHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        protected override async Task<QueryLatestNMessagesSentResponse> HandleCommand(QueryLatestNMessagesSent request, QueryLatestNMessagesSentResponse response, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.userId) || request.n <= 0)
                throw new GenericValidationException("Invalid parameters!");

            response.Messages = await _messageRepository.GetLatestNMessagesSentByUser(request.userId,request.n);
            return response;
        }
    }
}