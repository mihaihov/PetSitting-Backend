using Microsoft.AspNetCore.Identity;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext _dbContext) : base(_dbContext)
        {
        }

        public async Task AddRole(IdentityUserRole<string> role)
        {
            await _dbContext.UserRoles.AddAsync(role);
        }

        public async Task AddUserProfile(UserProfile userProfile)
        {
            await _dbContext.UserProfiles!.AddAsync(userProfile);
        }

        public async Task AddUserSettings(UserSettings userSettings)
        {
            await _dbContext.UserSettings!.AddAsync(userSettings);
        }
    }
}