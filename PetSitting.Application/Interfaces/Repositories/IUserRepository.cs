using Microsoft.AspNetCore.Identity;
using PetSitting.Domain.Entities.UserManagement;

namespace PetSitting.Application.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<ApplicationUser>
    {
        public Task AddRole(IdentityUserRole<string> role);
        public Task AddUserProfile(UserProfile userProfile);
        public Task AddUserSettings(UserSettings userSettings);
        public Task<IReadOnlyList<string>> GetRoles(string UserId);
    }
}