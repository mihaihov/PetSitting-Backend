using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetSitting.Application.Interfaces.Repositories;
using PetSitting.Domain.Entities.Security;
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
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddUserProfile(UserProfile userProfile)
        {
            await _dbContext.Set<UserProfile>().AddAsync(userProfile);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddUserSettings(UserSettings userSettings)
        {
            await _dbContext.Set<UserSettings>().AddAsync(userSettings);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            return await QueryByEmailAsync(email).FirstOrDefaultAsync();
        }

        public IQueryable<ApplicationUser> QueryByEmailAsync(string email)
        {
            return _dbContext.Set<ApplicationUser>().Where(u => u.Email == email);
        }

        public async Task<IReadOnlyList<string>> GetRoles(string UserId)
        {
            return await _dbContext.UserRoles.Where(ur => ur.UserId == UserId)
                .Join(_dbContext.Roles, ur => ur.RoleId, r => r.Id, (ur,r) => r.Name!)
                .ToListAsync();
        }

        public async Task StoreRefreshToken(RefreshToken token)
        {
            await _dbContext.Set<RefreshToken>().AddAsync(token);
            await _dbContext.SaveChangesAsync();
        }
    }
}