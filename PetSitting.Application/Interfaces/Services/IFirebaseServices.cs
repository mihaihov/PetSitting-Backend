using Firebase.Auth;
using FirebaseAdmin.Auth;

namespace PetSitting.Application.Interfaces.Services
{
    public interface IFirebaseServices
    {
        public Task<FirebaseToken> VerifyTokenAsync(string idToken);
        public Task<UserRecord> CreateUserAsync (UserRecordArgs args);

        public Task<UserRecord> GetUserByEmailAsync(string email);
        public Task<FirebaseAuthLink> CreateUserWithEmailAndPasswordAsync(string email, string password);
        public Task<FirebaseAuthLink> SignInWithEmailAndPasswordAsync(string email, string password);
        public Task<UserRecord?> GetUserByIdAsync(string id);
    }
}