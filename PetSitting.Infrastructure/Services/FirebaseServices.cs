using FirebaseAuth = FirebaseAdmin.Auth.FirebaseAuth;
using UserRecord = FirebaseAdmin.Auth.UserRecord;
using UserRecordArgs = FirebaseAdmin.Auth.UserRecordArgs;
using FirebaseToken = FirebaseAdmin.Auth.FirebaseToken;
using Google.Apis.Auth.OAuth2;
using PetSitting.Application.Interfaces.Services;
using Firebase.Auth;

namespace PetSitting.Infrastructure.Services
{
    public class FirebaseServices : IFirebaseServices
    {
        private readonly FirebaseAuthProvider _firebaseProvider;
        public FirebaseServices()
        {
            _firebaseProvider = new FirebaseAuthProvider(new FirebaseConfig("AIzaSyA-oK7GEDK6S-UYdLEBm2DhrSix1k-zQAM"));
        }
        public async Task<UserRecord> CreateUserAsync(UserRecordArgs args)
        {
            return await FirebaseAuth.DefaultInstance.CreateUserAsync(args);
        }

        public async Task<FirebaseToken> VerifyTokenAsync(string idToken)
        {
            return await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
        }

        public async Task<UserRecord> GetUserByEmailAsync(string email)
        {
            return await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);
        }

        //test 
        public async Task<FirebaseAuthLink> CreateUserWithEmailAndPasswordAsync(string email, string password)
        {
            return await _firebaseProvider.CreateUserWithEmailAndPasswordAsync(email,password);
        }

        public async Task<FirebaseAuthLink> SignInWithEmailAndPasswordAsync(string email, string password)
        {
            return await _firebaseProvider.SignInWithEmailAndPasswordAsync(email,password);
        }

        public async Task<UserRecord?> GetUserByIdAsync(string id)
        {
            return await FirebaseAuth.DefaultInstance.GetUserAsync(id);
        }

        public async Task SendEmailVerificationAsync(string firebaseToken)
        {
            await _firebaseProvider.SendEmailVerificationAsync(firebaseToken);
        }

        public async Task SendPasswordResetEmailAsync(string firebaseToken)
        {
            await _firebaseProvider.SendPasswordResetEmailAsync(firebaseToken);
        }

        public async Task<FirebaseAuthLink> ResetPasswordAsync(string firebaseToken, string newPassword)
        {
            return await _firebaseProvider.ChangeUserPassword(firebaseToken,newPassword);
        }

        public async Task ChangeEmailAsync(string firebaseToken, string newEmail)
        {
            await _firebaseProvider.ChangeUserEmail(firebaseToken,newEmail);
        }
    }
}