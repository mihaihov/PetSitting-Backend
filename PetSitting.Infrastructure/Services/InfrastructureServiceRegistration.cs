using FirebaseAdmin.Auth;
using Microsoft.Extensions.DependencyInjection;
using PetSitting.Application.Interfaces.Services;

namespace PetSitting.Infrastructure.Services
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            //one instance per application lifetime.
            serviceCollection.AddScoped<IFirebaseService,FirebaseService>();
            serviceCollection.AddScoped<IStripeServices,StripeServices>();
            serviceCollection.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            return serviceCollection;
        }
    }
}