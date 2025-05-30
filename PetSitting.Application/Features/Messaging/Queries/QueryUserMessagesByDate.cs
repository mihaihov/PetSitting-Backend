using MediatR;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Messaging;

namespace PetSitting.Application.Features.Messaging.Queries
{
    public record QueryUserMessagesByDate(string userId, DateTime timeStamp) : IRequest<QueryUserMessagesByDateResponse>;
    public record QueryUserMessagesByDateResponse : BaseResponse
    {
        public ICollection<Message>? Messages {get;set;}
    }

    public class QueryUserMessagesByDateHandler : BaseHandler<QueryUserMessagesByDate, QueryUserMessagesByDateResponse>
    {
        private readonly IMessageRepository _messageRepository;
        public QueryUserMessagesByDateHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        protected override async Task<QueryUserMessagesByDateResponse> HandleCommand(QueryUserMessagesByDate request, QueryUserMessagesByDateResponse response, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.userId))
                throw new GenericValidationException("Invalid parameters.");

            response.Messages = await _messageRepository.GetUserMessagesByDate(request.userId, request.timeStamp);
            return response;
        }
    }
}