using PetSitting.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetSitting.Domain.Interfaces.Repositories;

namespace PetSitting.Infrastructure
{
    public static class RegisterPersistenceServices
    {
        public static IServiceCollection RegisterDbContext(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DevelopmentDb")));
            return serviceCollection;
        }

        public static IServiceCollection RegisterRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            return serviceCollection;
        }
    }
}