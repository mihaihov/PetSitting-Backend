using FirebaseAdmin.Auth;
using PetSitting.Application.Interfaces.Services;

namespace PetSitting.Infrastructure.Services
{
    public class FirebaseServices : IFirebaseServices
    {
        public async Task<UserRecord> CreateUserAsync(UserRecordArgs args)
        {
            return await FirebaseAuth.DefaultInstance.CreateUserAsync(args);
        }

        public async Task<FirebaseToken> VerifyTokenAsync(string idToken)
        {
            return await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
        }
    }
}