using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MediatR;
using PetSitting.Application.Features.Messaging.Commands;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;
using PetSitting.Domain.Entities.Messaging;
using PetSitting.Domain.Entities.PostManagement;

namespace PetSitting.Infrastructure.Services
{
    public class MessagingServices : IMessagingServices
    {
        private readonly IMediator _mediator;
        private readonly IChatSessionRepository _chatSessionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBaseRepository<JobPost> _jobPostRepository;
        public MessagingServices(IMediator mediator, IChatSessionRepository chatSessionRepository, IUserRepository userRepository, IBaseRepository<JobPost> jobPostRepository)
        {
            _mediator = mediator;
            _chatSessionRepository = chatSessionRepository;
            _userRepository = userRepository;
            _jobPostRepository = jobPostRepository;
        }
        public async Task<bool> CanUsersChat(string chatSessionId)
        {
            var chatSession = await _chatSessionRepository.GetByIdAsync(chatSessionId);
            if(chatSession == null) return false;
            var firstUser = await _userRepository.GetByIdAsync(chatSession.FirstUserId);
            var secondUser = await _userRepository.GetByIdAsync(chatSession.SecondUserId);
            var jobPost = await _jobPostRepository.GetByIdAsync(chatSession.JobPostId);


            if(!firstUser!.IsPetSitter && !secondUser!.IsPetSitter)   return false;
            if(DateTime.Now > jobPost!.StartDate.AddDays(2)) return false;    //doublecheck
            if(!chatSession.IsActive) return false;
            return true;
        }

        public async Task CreateChatSession(string firstUser, string secondUser, string jobPostId)
        {
            var command = new AddChatSessionCommand(firstUser,secondUser,jobPostId, null);
            await _mediator.Send(command);
        }

        public async Task<ChatSession?> DoesChatSessionExists(string chatSessionId)
        {
            return await _chatSessionRepository.GetByIdAsync(chatSessionId);
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