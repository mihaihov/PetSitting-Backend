using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PetSitting.Infrastructure
{
    public static class RegisterPersistenceServices
    {
        public static IServiceCollection RegisterDbContext(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DevelopmentDb")));
            return serviceCollection;
        }
    }
}