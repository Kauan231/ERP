using Microsoft.AspNetCore.Identity;

namespace ERP.Services.Roles
{
    public interface IRolesService
    {
        List<IdentityRole> GetAllRoles();
        Task<IdentityResult> CreateRole(string roleName);
        Task<IdentityResult> DeleteRole(string roleName);
        Task<IdentityResult> AddUserToRole(string userId, string roleName);
        Task<IdentityResult> RemoveUserFromRole(string userId, string roleName);
        Task<IList<string>> GetUserRoles(string userId);
        void Startup();
    }
}
