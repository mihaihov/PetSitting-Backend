using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using PetSitting.Application.Interfaces.Services;

namespace PetSitting.Infrastructure.Services
{
    public class FirebaseServices : IFirebaseServices
    {
        public FirebaseServices()
        {
            
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
        public async Task<UserCredential> SignInWithEmailAndPasswordAsync(string Email, string Password)
        {
            throw new NotImplementedException();
        }
    }
}