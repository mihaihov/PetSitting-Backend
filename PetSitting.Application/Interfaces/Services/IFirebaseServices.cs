using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

namespace PetSitting.Application.Interfaces.Services
{
    public interface IFirebaseServices
    {
        public Task<FirebaseToken> VerifyTokenAsync(string idToken);
        public Task<UserRecord> CreateUserAsync (UserRecordArgs args);

        public Task<UserRecord> GetUserByEmailAsync(string email);
        public Task<UserCredential> SignInWithEmailAndPasswordAsync(string email, string password);
    }
}