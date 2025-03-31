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
using Firebase.Auth;

namespace PetSitting.UnitTests.Application
{
    public class RegisterCommandTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IBaseRepository<IdentityRole>> _mockRoleRepository;
        private readonly Mock<IFirebaseService> _mockFirebaseServices;
        private readonly Mock<IFirebaseAuthProvider> _mockFirebaseAuthProvider;
        private readonly Mock<Firebase.Auth.FirebaseAuth> _mockFirebaseAuth;
        private readonly Mock<FirebaseAuthLink> _mockFirebaseAuthLink;
        private readonly Mock<UserManager<ApplicationUser>> _userManager;

        public RegisterCommandTests()
        {
            //generic arrange for _userRepository.
            _mockUserRepository = new Mock<IUserRepository>();
            _mockUserRepository.Setup(user => user.SaveChangesAsync())
                .Returns(Task.CompletedTask);
            _mockUserRepository.Setup(user => user.CommitTransactionAsync())
                .Returns(Task.CompletedTask);
            var mockTransaction = new Mock<IDbContextTransaction>();
            _mockUserRepository.Setup(f => f.BeginTransactionAsync())
                .Returns(Task.FromResult(mockTransaction.Object));

            //generic arrange for _mockRoleRepository.
            _mockRoleRepository = new Mock<IBaseRepository<IdentityRole>>();
            _mockRoleRepository.Setup(f => f.FirstOrDefaultAsync(It.IsAny<Expression<Func<IdentityRole, bool>>>()))
                .ReturnsAsync(new IdentityRole { });
            _mockRoleRepository.Setup(f => f.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            //generic arrange for firebase related services.
            _mockFirebaseServices = new Mock<IFirebaseService>();
            _mockFirebaseAuthProvider = new Mock<IFirebaseAuthProvider>();
            _mockFirebaseAuth = new Mock<Firebase.Auth.FirebaseAuth>();
            _mockFirebaseAuthLink = new Mock<FirebaseAuthLink>(_mockFirebaseAuthProvider.Object, _mockFirebaseAuth.Object);
            _mockFirebaseAuthLink.Object.User = new User() { LocalId = "FirebaseId" };
            _mockFirebaseServices.Setup(f => f.CreateUserWithEmailAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_mockFirebaseAuthLink.Object));

            //generic arrange for UserManager<ApplicationUser>
            var _mockPasswordHasher = new Mock<IPasswordHasher<ApplicationUser>>();
            _mockPasswordHasher.Setup(f => f.HashPassword(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Returns("hashedPassword");
            var _mockUserStore = new Mock<IUserStore<ApplicationUser>>();
#pragma warning disable
            _userManager = new Mock<UserManager<ApplicationUser>>(_mockUserStore.Object, null, _mockPasswordHasher.Object, null, null, null, null, null, null);
#pragma warning restore
        }


        [Fact]
        public async Task Handle_ShouldCreateApplicationUser_WhenValidationSucceeds()
        {
            //arrange
            RegisterCommand command = new RegisterCommand(firstName: "Raducu", lastName: "Mihai",
                email: "raducumihaicristian@gmail.com",password: "P@ssword1!", username: null);
            
            var newUser = new ApplicationUser {
                FirstName = command.firstName!,
                LastName = command.lastName!,
                Email = command.email,
            };

            _mockUserRepository.Setup(user => user.AddAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(newUser);



            //act
            RegisterCommandHandler commandHandler = new RegisterCommandHandler(_mockUserRepository.Object,
                _mockRoleRepository.Object, _mockFirebaseServices.Object, _userManager.Object);
            var response = await commandHandler.Handle(command, cancellationToken: CancellationToken.None);


            //assert
            _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<ApplicationUser>()), Times.Once);
            _mockUserRepository.Verify(x => x.SaveChangesAsync(),Times.Once);
            _mockUserRepository.Verify(x => x.BeginTransactionAsync(), Times.Once);
            _mockUserRepository.Verify(x => x.CommitTransactionAsync(),Times.Once);

            _mockRoleRepository.Verify(y => y.FirstOrDefaultAsync(It.IsAny<Expression<Func<IdentityRole,bool>>>()), Times.Once);
            _mockRoleRepository.Verify(y => y.SaveChangesAsync(), Times.Once);
            
            Assert.True(response.Success);
            Assert.Null(response.ValidationErrors);
        }

        [Fact]
        public async Task Handle_ShouldThrowError_WhenFirebaseUserCreationFails()
        {
            //arrange
            var command = new RegisterCommand(firstName: "Raducu", lastName: "Mihai",
                email: "raducumihaicristian@gmail.com",password: "P@ssword1!", username: null);
            
#pragma warning disable
            _mockFirebaseServices.Setup(f => f.CreateUserWithEmailAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((FirebaseAuthLink)null);
#pragma warning restore

            var commandHandler = new RegisterCommandHandler(_mockUserRepository.Object,
                _mockRoleRepository.Object, _mockFirebaseServices.Object, _userManager.Object);

            //act and assert
            await Assert.ThrowsAsync<Exception>(() => commandHandler.Handle(command, CancellationToken.None));

            _mockFirebaseServices.Verify(f => f.CreateUserWithEmailAndPasswordAsync(command.email,command.password), Times.Once);
            _mockRoleRepository.VerifyNoOtherCalls();
            //ASSERT THAT THE TRANSACTION IS ROLLED BACK.
        }
    }
}