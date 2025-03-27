using Firebase.Auth;
using FirebaseAdmin.Auth;

namespace PetSitting.Application.Interfaces.Services
{
    public interface IFirebaseService
    {
        public Task<FirebaseToken> VerifyTokenAsync(string idToken);
        public Task<UserRecord> CreateUserAsync (UserRecordArgs args);

        public Task<UserRecord> GetUserByEmailAsync(string email);
        public Task<FirebaseAuthLink> CreateUserWithEmailAndPasswordAsync(string email, string password);
        public Task<FirebaseAuthLink> SignInWithEmailAndPasswordAsync(string email, string password);
        public Task<UserRecord?> GetUserByIdAsync(string id);
        public Task SendEmailVerificationAsync(string firebaseToken);
        public Task SendPasswordResetEmailAsync(string firebaseToken);
        public Task<FirebaseAuthLink> ResetPasswordAsync(string firebaseToken, string newPassword);
        public Task ChangeEmailAsync(string firebaseToken, string newEmail);
    }
}