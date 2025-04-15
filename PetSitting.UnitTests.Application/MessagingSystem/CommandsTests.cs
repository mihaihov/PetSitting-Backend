using System.Net;
using Moq;
using PetSitting.Application.Features.Messaging.Commands;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Messaging;

namespace PetSitting.UnitTests.Application.MessagingSystem
{
    public class CommandsTests
    {
        [Fact]
        public async Task HandleCommand_ShouldNotAddChatSession_IfValidationFails()
        {
            //arrange
            Mock<IChatSessionRepository> mockChatSessionRepository = new Mock<IChatSessionRepository>();
            mockChatSessionRepository.Setup(c => c.AddAsync(It.IsAny<ChatSession>()))
                .Returns(Task.CompletedTask);
            var command = new AddChatSessionCommand("","","",null);
            var commandHandler = new AddChatSessionCommandHandler(mockChatSessionRepository.Object);

            //act
            var response = await commandHandler.Handle(command,CancellationToken.None);

            //assert
            mockChatSessionRepository.Verify(c => c.AddAsync(It.IsAny<ChatSession>()), Times.Never);
            Assert.NotNull(response.ValidationErrors);
            Assert.NotEmpty(response.ValidationErrors);
        }

        [Fact]
        public async Task HandleCommand_ShouldAddChatSession_IfValidationSucceeds()
        {
            //arrange
            Mock<IChatSessionRepository> mockChatSessionRepository = new Mock<IChatSessionRepository>();
            mockChatSessionRepository.Setup(c => c.AddAsync(It.IsAny<ChatSession>()))
                .Returns(Task.CompletedTask);
            var command = new AddChatSessionCommand("test","test","test",null);
            var commandHandler = new AddChatSessionCommandHandler(mockChatSessionRepository.Object);

            //act
            var response = await commandHandler.Handle(command,CancellationToken.None);

            //assert
            mockChatSessionRepository.Verify(c => c.AddAsync(It.IsAny<ChatSession>()), Times.Once);
            Assert.Null(response.ValidationErrors);
        }

        [Fact]
        public async Task HandleCommand_ShouldNotAddMessage_IfValidationFails()
        {
            //arrange
            Mock<IMessageRepository> mockMessageRepository = new Mock<IMessageRepository>();
            mockMessageRepository.Setup(c => c.AddAsync(It.IsAny<Message>()))
                .Returns(Task.CompletedTask);
            var command = new AddMessageCommand("","","","",DateTime.Now);
            var commandHandler = new AddMessageCommandHandler(mockMessageRepository.Object);

            //act
            var response = await commandHandler.Handle(command,CancellationToken.None);

            //assert
            mockMessageRepository.Verify(c => c.AddAsync(It.IsAny<Message>()), Times.Never);
            Assert.NotNull(response.ValidationErrors);
            Assert.NotEmpty(response.ValidationErrors);
        }

        [Fact]
        public async Task HandleCommand_ShouldAddMessage_IfValidationSucceeds()
        {
            //arrange
            Mock<IMessageRepository> mockMessageRepository = new Mock<IMessageRepository>();
            mockMessageRepository.Setup(c => c.AddAsync(It.IsAny<Message>()))
                .Returns(Task.CompletedTask);
            var command = new AddMessageCommand("test","test","test","test",DateTime.Now);
            var commandHandler = new AddMessageCommandHandler(mockMessageRepository.Object);

            //act
            var response = await commandHandler.Handle(command,CancellationToken.None);

            //assert
            mockMessageRepository.Verify(c => c.AddAsync(It.IsAny<Message>()), Times.Once);
            Assert.Null(response.ValidationErrors);
        }

    }
}