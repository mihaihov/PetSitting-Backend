using Xunit;
using Moq;
using PetSitting.Application.Features.UserManagement.Commands;
using FirebaseAdmin.Auth;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.UserManagement;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage;
using Google.Apis.Auth.OAuth2;
using PetSitting.Application.Interfaces.Services;

namespace PetSitting.UnitTests.Application
{
    public class UserManagementTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IBaseRepository<IdentityRole>> _mockRoleRepository;

        public UserManagementTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockRoleRepository = new Mock<IBaseRepository<IdentityRole>>();
        }

        [Fact]
        public async Task Handle_ShouldCreateApplicationUser_WhenValidationSucceeds()
        {
            //arrange
            RegisterCommand command = new RegisterCommand(firstName: "Raducu", lastName: "Mihai", email: "raducumihaicristian@gmail.com", password: "P@ssword1!", username: null);
            
            var newUser = new ApplicationUser {
                FirstName = command.firstName!,
                LastName = command.lastName!,
                Email = command.email,
            };

            //mocking methods/services used by IUserRepository.
            _mockUserRepository.Setup(f => f.AddAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(newUser);
            _mockUserRepository.Setup(f => f.SaveChangesAsync())
                .Returns(Task.CompletedTask);
            _mockUserRepository.Setup(f => f.CommitTransactionAsync())
                .Returns(Task.CompletedTask);
            var mockTransaction = new Mock<IDbContextTransaction>();
            _mockUserRepository.Setup(f => f.BeginTransactionAsync())
                .Returns(Task.FromResult(mockTransaction.Object));

            //mocking firebase serivces
            var _mockUserRecord = new Mock<UserRecord>();
            var _mockFirebaseService = new Mock<IFirebaseServices>();
            _mockFirebaseService.Setup(f => f.CreateUserAsync(It.IsAny<UserRecordArgs>()))
                 .ReturnsAsync(_mockUserRecord.Object);
            
            //mocking methods/services used by IBaseRepository<IdentityRole>
            _mockRoleRepository.Setup(f => f.FirstOrDefaultAsync(It.IsAny<Expression<Func<IdentityRole, bool>>>()))
                .ReturnsAsync(new IdentityRole {});
            _mockRoleRepository.Setup(f => f.SaveChangesAsync())
                .Returns(Task.CompletedTask);
            
            //act
            RegisterCommandHandler commandHandler = new RegisterCommandHandler(_mockUserRepository.Object, _mockRoleRepository.Object,
                _mockFirebaseService.Object);
            var response = await commandHandler.Handle(command, cancellationToken: CancellationToken.None);

            //assert
            _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<ApplicationUser>()), Times.Once);
            _mockUserRepository.Verify(x => x.SaveChangesAsync(),Times.Once);
            _mockUserRepository.Verify(x => x.BeginTransactionAsync(), Times.Once);
            _mockUserRepository.Verify(x => x.CommitTransactionAsync(),Times.Once);

            _mockRoleRepository.Verify(y => y.FirstOrDefaultAsync(It.IsAny<Expression<Func<IdentityRole,bool>>>()), Times.Once);
            _mockRoleRepository.Verify(y => y.SaveChangesAsync(), Times.Once);
            
            Assert.True(response.Success);
            Assert.Equal(response.ValidationErrors?.Count, 0);
        }
    }
}