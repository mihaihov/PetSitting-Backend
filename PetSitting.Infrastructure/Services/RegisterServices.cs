using FirebaseAdmin.Auth;
using Microsoft.Extensions.DependencyInjection;
using PetSitting.Application.Interfaces.Services;

namespace PetSitting.Infrastructure.Services
{
    public static class RegisterServices
    {
        public static IServiceCollection RegisterFirebaseServices(this IServiceCollection serviceCollection)
        {
            //one instance per application lifetime.
            serviceCollection.AddSingleton<IFirebaseServices>(new FirebaseServices());
            return serviceCollection;
        }
    }
}