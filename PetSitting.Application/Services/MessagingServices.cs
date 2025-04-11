using MediatR;
using PetSitting.Application.Features.Messaging.Commands;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;
using PetSitting.Domain.Entities.Messaging;
using PetSitting.Domain.Entities.PostManagement;
using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Application.Services
{
    public class MessagingServices : IMessagingServices
    {
        private readonly IMediator _mediator;
        private readonly IChatSessionRepository _chatSessionRepository;
        public MessagingServices(IMediator mediator, IChatSessionRepository chatSessionRepository)
        {
            _mediator = mediator;
            _chatSessionRepository = chatSessionRepository;
        }
        public bool CanUsersChat(ApplicationUser firstUser, ApplicationUser secondUser, JobPost jobPost, ChatSession chatSession)
        {
            if(!firstUser.IsPetSitter && !secondUser.IsPetSitter)   return false;
            if(DateTime.Now < jobPost.StartDate.AddDays(-2) || DateTime.Now > jobPost.StartDate.AddDays(2)) return false;    //doublecheck
            if(!chatSession.IsActive) return false;
            return true;
        }

        public async Task CreateChatsession(string firstUser, string secondUser, string jobPostId)
        {
            var command = new AddChatSessionCommand(firstUser,secondUser,jobPostId, null);
            await _mediator.Send(command);
        }

        public async Task<bool> DoesChatSessionExists(string chatSessionId)
        {
            var chatSession = await _chatSessionRepository.GetByIdAsync(chatSessionId);
            if(chatSession != null) return true;
            return false;
        }

        public string GenerateChatSessionId(string firstUser, string secondUser)
        {
            //makes sure it is consistent regardless of user order.
            var users = new[] { firstUser, secondUser };
            Array.Sort(users);
            return $"{users[0]}_{users[1]}";
        }

        public async Task<ICollection<Message>?> GetRecenteMessagesAsync(string chatSessionId, int count)
        {
            return await _chatSessionRepository.GetRecentMessages(chatSessionId,count);
        }
    }
}