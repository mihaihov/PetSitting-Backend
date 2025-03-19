using Microsoft.Extensions.DependencyInjection;
using Petsitting.Application.Interfaces.Services;
using Petsitting.Infrastructure.Services;

namespace PetSitting.Infrastructure.Services
{
    public static class RegisterServices
    {
        public static IServiceCollection RegisterFirebaseServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IFirebaseServices, FirebaseServices>();
            return serviceCollection;
        }
    }
}