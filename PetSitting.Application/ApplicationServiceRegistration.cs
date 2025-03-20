using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace PetSitting.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}
