using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetSitting.Application.Interfaces.Services;
using PetSitting.Application.Services;

namespace PetSitting.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddScoped<IMessagingServices,MessagingServices>();
        services.AddHostedService<StripeWorker>();
        return services;
    }
}
