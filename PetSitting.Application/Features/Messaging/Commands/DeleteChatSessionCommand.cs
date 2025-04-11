using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Messaging;

namespace PetSitting.Application.Features.Messaging.Commands
{
    public record DeleteChatSessionCommand(ChatSession chatSession) : IRequest<BaseResponse>;
    public class DeleteChatSessionCommandHandler : BaseHandler<DeleteChatSessionCommand, BaseResponse>
    {
        private readonly IChatSessionRepository _chatSessionRepository;
        public DeleteChatSessionCommandHandler(IChatSessionRepository chatSessionRepository)
        {
            _chatSessionRepository = chatSessionRepository;
        }
        protected override async Task<BaseResponse> HandleCommand(DeleteChatSessionCommand request, BaseResponse response, CancellationToken cancellationToken)
        {
            await _chatSessionRepository.DeleteAsync(request.chatSession);
            return response;
        }
    }
}