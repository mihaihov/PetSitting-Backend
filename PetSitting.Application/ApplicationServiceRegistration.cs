using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PetSitting.Application.Interfaces.Services;

namespace PetSitting.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        return services;
    }
}
