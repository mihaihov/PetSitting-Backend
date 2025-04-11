using MediatR;
using PetSitting.Application.Features.Common;
using PetSitting.Application.Features.Messaging.Validators;
using PetSitting.Application.Features.UserManagement.Entities;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Messaging;

namespace PetSitting.Application.Features.Messaging.Commands
{
    public record AddChatSessionCommand(string petOwnerId, string petSitterId, 
        string jobPostId, bool? isActive) : IRequest<BaseResponse>;
    public class AddChatSessionCommandHandler : BaseHandler<AddChatSessionCommand,BaseResponse,AddChatSessionCommandValidator>
    {
        private readonly IChatSessionRepository _chatSessionRepository;
        public AddChatSessionCommandHandler(IChatSessionRepository chatSessionRepository)
        {
            _chatSessionRepository = chatSessionRepository;
        }

        protected override async Task<BaseResponse> HandleCommand(AddChatSessionCommand request, BaseResponse response, CancellationToken cancellationToken)
        {
            var chatSession = new ChatSession{
                PetOwnerId = request.petOwnerId,
                PetSitterId = request.petSitterId,
                JobPostId = request.jobPostId,
                IsActive = request.isActive == null ? false : true
            };
            await _chatSessionRepository.AddAsync(chatSession);
            return response;
        }
    }
}