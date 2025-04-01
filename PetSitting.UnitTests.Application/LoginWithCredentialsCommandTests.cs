using Firebase.Auth;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Options;
using Moq;
using PetSitting.Application.Features.UserManagement;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Application.Interfaces.Services;
using PetSitting.Domain.Entities.Security;
using PetSitting.Domain.Entities.UserManagement;
using PetSitting.Domain.Entities.Utils;

namespace PetSitting.UnitTests.Application
{
    public class LoginWithCredentialsCommandTests
    {
        private readonly Mock<IFirebaseService> _mockFirebaseServices;
        private readonly Mock<IFirebaseAuthProvider> _mockFirebaseAuthProvider;
        private readonly Mock<Firebase.Auth.FirebaseAuth> _mockFirebaseAuth;
        private readonly Mock<FirebaseAuthLink> _mockFirebaseAuthLink;

        private readonly Mock<IUserRepository> _mockUserRepository;


        public LoginWithCredentialsCommandTests()
        {
            _mockFirebaseServices = new Mock<IFirebaseService>();
            _mockFirebaseAuthProvider = new Mock<IFirebaseAuthProvider>();
            _mockFirebaseAuth = new Mock<Firebase.Auth.FirebaseAuth>();
            _mockFirebaseAuthLink = new Mock<FirebaseAuthLink>(_mockFirebaseAuthProvider.Object, _mockFirebaseAuth.Object);

            _mockUserRepository = new Mock<IUserRepository>();
        }

        [Fact]
        public async Task Handle_LogsUser_WhenCredentialsAreValid()
        {
            var loginWithCredentialsCommand = new LoginWithCredentialsCommand(email: "raducumihaicristian@gmail.com", password: "P@ssword1!");

            var _mockApplicationUser = new Mock<ApplicationUser>();
            var _mockRolesList = new Mock<IReadOnlyList<string>>();
            var _mockFirebaseToken = new Mock<FirebaseToken>();
            _mockFirebaseServices.Setup(f => f.SignInWithEmailAndPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_mockFirebaseAuthLink.Object));
            _mockFirebaseServices.Setup(f => f.VerifyTokenAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(_mockFirebaseToken.Object));
            _mockUserRepository.Setup(u => u.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(_mockApplicationUser.Object);
            _mockUserRepository.Setup(u => u.GetRoles(It.IsAny<string>()))
                .ReturnsAsync(_mockRolesList.Object);
            _mockUserRepository.Setup(u => u.StoreRefreshToken(It.IsAny<RefreshToken>()))
                .Returns(Task.CompletedTask);
            var _mockJwtSettings = new Mock<IOptions<JwtSettings>>();

            var loginWithCredentialsCommandHandler = new LoginWithCredentialsCommandHandler(_mockFirebaseServices.Object
                ,_mockUserRepository.Object, _mockJwtSettings.Object);

            //act & assert
            await loginWithCredentialsCommandHandler.Handle(loginWithCredentialsCommand, CancellationToken.None);

            _mockUserRepository.Verify(u => u.StoreRefreshToken(It.IsAny<RefreshToken>()),Times.Once);
        } 
    }
}