using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.Messaging.Validators;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Messaging;

namespace PetSitting.Application.Features.Messaging.Commands
{
    public record AddMessageCommand(string chatSessionId, string senderId,
         string recipientId, string content, DateTime? timeStamp) : IRequest<BaseResponse>;

    public class AddMessageCommandHandler : BaseHandler<AddMessageCommand, BaseResponse, AddMessageCommandValidator>
    {
        private readonly IMessageRepository _messageRepository;
        public AddMessageCommandHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        protected override async Task<BaseResponse> HandleCommand(AddMessageCommand request, BaseResponse response, CancellationToken cancellationToken)
        {
            var message = new Message {
                ChatSessionId = request.chatSessionId,
                SenderId = request.senderId,
                RecipientId = request.recipientId,
                Content = request.content,
                Timestamp = request.timeStamp == null ? DateTime.Now : (DateTime)request.timeStamp
            };
            await _messageRepository.AddAsync(message);
            return response;
        }
    }
}