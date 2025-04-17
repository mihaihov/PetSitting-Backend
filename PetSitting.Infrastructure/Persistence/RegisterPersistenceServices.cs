using PetSitting.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetSitting.Application.Interfaces.Repositories;

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
            //one instance per http request.
            serviceCollection.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            serviceCollection.AddScoped<IUserRepository, UserRepository>();
            serviceCollection.AddTransient<IJobApplicationRepository, JobApplicationRepository>();
            serviceCollection.AddTransient<IMessageRepository, MessageRepository>();
            serviceCollection.AddTransient<IChatSessionRepository, ChatSessionRepository>();
            serviceCollection.AddTransient<IReviewRepository,ReviewRepository>();
            return serviceCollection;
        }
    }
}