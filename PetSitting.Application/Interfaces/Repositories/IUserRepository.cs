using Microsoft.AspNetCore.Identity;
using PetSitting.Domain.Entities.Security;
using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Application.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<ApplicationUser>
    {
        public Task<ApplicationUser?> GetByEmailAsync(string email);
        public Task AddRole(IdentityUserRole<string> role);
        public Task AddUserProfile(UserProfile userProfile);
        public Task AddUserSettings(UserSettings userSettings);
        public Task<IReadOnlyList<string>> GetRoles(string UserId);
        public Task StoreRefreshToken(RefreshToken token);
    }
}