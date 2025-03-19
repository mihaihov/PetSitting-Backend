using Microsoft.Extensions.DependencyInjection;
using PetSitting.Domain.Interfaces.Services;

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