using FirebaseAdmin.Auth;
using Microsoft.Extensions.DependencyInjection;
using PetSitting.Application.Interfaces.Services;

namespace PetSitting.Infrastructure.Services
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection RegisterFirebaseServices(this IServiceCollection serviceCollection)
        {
            //one instance per application lifetime.
            serviceCollection.AddSingleton<IFirebaseService>(new FirebaseService());
            return serviceCollection;
        }
    }
}