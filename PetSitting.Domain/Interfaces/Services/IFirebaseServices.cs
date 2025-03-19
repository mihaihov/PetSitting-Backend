using FirebaseAdmin.Auth;

namespace PetSitting.Domain.Interfaces.Services
{
    public interface IFirebaseServices
    {
        public Task<FirebaseToken> VerifyTokenAsync(string idToken);
    }
}