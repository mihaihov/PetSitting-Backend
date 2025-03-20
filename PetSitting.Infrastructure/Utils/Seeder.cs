using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PetSitting.Domain.Enums;

namespace PetSitting.Infrastructure.Utils
{
    public sealed class Seeder
    {
        public static void SeedRoles(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (!dbContext.Roles.Any())
                {
                    dbContext.Roles.Add(new IdentityRole { Name = Roles.Admin.ToString() });
                    dbContext.Roles.Add(new IdentityRole { Name = Roles.PetSitter.ToString() });
                    dbContext.Roles.Add(new IdentityRole { Name = Roles.PetOwner.ToString() });
                    dbContext.SaveChanges();
                }
            }
        }
    }
}