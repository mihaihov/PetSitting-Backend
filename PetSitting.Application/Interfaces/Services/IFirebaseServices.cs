using FirebaseAdmin.Auth;

namespace PetSitting.Application.Interfaces.Services
{
    public interface IFirebaseServices
    {
        public Task<FirebaseToken> VerifyTokenAsync(string idToken);
    }
}