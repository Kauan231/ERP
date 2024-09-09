using ERP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace ERP.Services.Roles
{
    public class RolesService : IRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public List<string> Roles = new List<string>
        {
            "admin", "hr"
        };

        public RolesService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async void Startup()
        {
            foreach(string role in Roles) { 
                try
                {
                    await CreateRole(role);
                }
                catch (Exception ex) { 
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public List<IdentityRole> GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return roles;
        }

        public async Task<IdentityResult> CreateRole(string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                return roleResult;
            }
            else
            {
                throw new Exception("Role already exists");
            }
        }

        public async Task<IdentityResult> DeleteRole(string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                throw new Exception("Role does not exist");
            }
            else
            {

                var roleResult = await _roleManager.DeleteAsync(await _roleManager.FindByNameAsync(roleName));
                return roleResult;
            }
        }

        public async Task<IdentityResult> AddUserToRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                return result;
            }
            throw new Exception("User does not exist");
        }

        public async Task<IdentityResult> RemoveUserFromRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                return result;
            }

            // User doesn’t exist
            throw new Exception("User does not exist");
        }

        // Get specific user role
        public async Task<IList<string>> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }
    }
}
