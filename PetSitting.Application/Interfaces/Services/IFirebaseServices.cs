using FirebaseAdmin.Auth;

namespace Petsitting.Application.Interfaces.Services
{
    public interface IFirebaseServices
    {
        public Task<FirebaseToken> VerifyTokenAsync(string idToken);
    }
}