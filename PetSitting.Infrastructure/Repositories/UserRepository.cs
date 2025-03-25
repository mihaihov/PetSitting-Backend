using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IReadOnlyList<string>> GetRoles(string UserId)
        {
            return await _dbContext.UserRoles.Where(ur => ur.UserId == UserId)
                .Join(_dbContext.Roles, ur => ur.RoleId, r => r.Id, (ur,r) => r.Name!)
                .ToListAsync();
        }
    }
}