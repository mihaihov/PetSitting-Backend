using Moq;
using PetSitting.Application.Exceptions;
using PetSitting.Application.Features.Messaging.Queries;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Messaging;

namespace PetSitting.UnitTests.Application.MessagingSystem
{
    public class QueriesTests
    {
        [Fact]
        public async Task HandleCommand_ShouldNotGetMessages_IfValidationFails()
        {
            //arrange
            var mockMessageRepository = new Mock<IMessageRepository>();
            var mockMessages = new Mock<ICollection<Message>>();
            mockMessageRepository.Setup(m => m.GetLatestNMessagesReceivedByUser(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(mockMessages.Object)!);
            var command = new QueryLatestNMessagesReceived("",0);
            var commandHandler = new QueryLatestNMessagesReceivedHandler(mockMessageRepository.Object);

            //act & assert
            var respons = await Assert.ThrowsAsync<GenericValidationException>(() => commandHandler.Handle(command,CancellationToken.None));
            mockMessageRepository.Verify(m => m.GetLatestNMessagesReceivedByUser(It.IsAny<string>(),It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task HandleCommand_ShouldGetMessages_IfValidationSucceeds()
        {
            //arrange
            var mockMessageRepository = new Mock<IMessageRepository>();
            var mockMessages = new Mock<ICollection<Message>>();
            mockMessageRepository.Setup(m => m.GetLatestNMessagesReceivedByUser(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(mockMessages.Object)!);
            var command = new QueryLatestNMessagesReceived("test",3);
            var commandHandler = new QueryLatestNMessagesReceivedHandler(mockMessageRepository.Object);

            //act & assert
            var respons = await commandHandler.Handle(command,CancellationToken.None);
            mockMessageRepository.Verify(m => m.GetLatestNMessagesReceivedByUser(It.IsAny<string>(),It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task HandleCommand_ShouldNotGetMessagesSent_IfValidationFails()
        {
            //arrange
            var mockMessageRepository = new Mock<IMessageRepository>();
            var mockMessages = new Mock<ICollection<Message>>();
            mockMessageRepository.Setup(m => m.GetLatestNMessagesSentByUser(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(mockMessages.Object)!);
            var command = new QueryLatestNMessagesSent("",0);
            var commandHandler = new QueryLatestNMessagesSentHandler(mockMessageRepository.Object);

            //act & assert
            var respons = await Assert.ThrowsAsync<GenericValidationException>(() => commandHandler.Handle(command,CancellationToken.None));
            mockMessageRepository.Verify(m => m.GetLatestNMessagesSentByUser(It.IsAny<string>(),It.IsAny<int>()), Times.Never);
        }


        [Fact]
        public async Task HandleCommand_ShouldGetMessagesSent_IfValidationSucceeds()
        {
            //arrange
            var mockMessageRepository = new Mock<IMessageRepository>();
            var mockMessages = new Mock<ICollection<Message>>();
            mockMessageRepository.Setup(m => m.GetLatestNMessagesSentByUser(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(mockMessages.Object)!);
            var command = new QueryLatestNMessagesSent("test",3);
            var commandHandler = new QueryLatestNMessagesSentHandler(mockMessageRepository.Object);

            //act & assert
            var respons = await commandHandler.Handle(command,CancellationToken.None);
            mockMessageRepository.Verify(m => m.GetLatestNMessagesSentByUser(It.IsAny<string>(),It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task HandleCommand_ShouldNotGetMessagesByDate_IfValidationFails()
        {
            //arrange
            var mockMessageRepository = new Mock<IMessageRepository>();
            var mockMessages = new Mock<ICollection<Message>>();
            mockMessageRepository.Setup(m => m.GetUserMessagesByDate(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(mockMessages.Object)!);
            var command = new QueryUserMessagesByDate("", DateTime.Now);
            var commandHandler = new QueryUserMessagesByDateHandler(mockMessageRepository.Object);

            //act & assert
            var respons = await Assert.ThrowsAsync<GenericValidationException>(() => commandHandler.Handle(command,CancellationToken.None));
            mockMessageRepository.Verify(m => m.GetUserMessagesByDate(It.IsAny<string>(),It.IsAny<DateTime>()), Times.Never);
        }

        [Fact]
        public async Task HandleCommand_ShouldGetMessagesByDate_IfValidationSucceeds()
        {
            //arrange
            var mockMessageRepository = new Mock<IMessageRepository>();
            var mockMessages = new Mock<ICollection<Message>>();
            mockMessageRepository.Setup(m => m.GetUserMessagesByDate(It.IsAny<string>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(mockMessages.Object)!);
            var command = new QueryUserMessagesByDate("test", DateTime.Now);
            var commandHandler = new QueryUserMessagesByDateHandler(mockMessageRepository.Object);

            //act & assert
            var respons = await commandHandler.Handle(command,CancellationToken.None);
            mockMessageRepository.Verify(m => m.GetUserMessagesByDate(It.IsAny<string>(),It.IsAny<DateTime>()), Times.Once);
        }
    }
}