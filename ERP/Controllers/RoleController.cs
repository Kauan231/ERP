using ERP.Data.Dtos;
using ERP.Repositories;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ERP.Services.Roles;
using Microsoft.AspNetCore.Rewrite;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRolesService _roleManager;

        public RoleController(IRolesService roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.GetAllRoles();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            try
            {
                IdentityResult result = await _roleManager.CreateRole(roleName);
                if (result.Succeeded)
                {
                    return Ok("Role added");
                }
                else
                {
                    return BadRequest("Issue adding the role");
                }
            } catch (Exception ex)
            {
                if(ex.Message.Equals("Role already exists"))
                {
                    return BadRequest("Role already exists");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }  
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            try
            {
                IdentityResult result = await _roleManager.DeleteRole(roleName);
                if (result.Succeeded)
                {
                    return Ok("Role Deleted");
                }
                else
                {
                    return BadRequest("Issue deleting the role");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("Role does not exist"))
                {
                    return NotFound("Role does not exist");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string userId, string roleName)
        {
            try
            {
                IdentityResult result = await _roleManager.AddUserToRole(userId,roleName);
                if (result.Succeeded)
                {
                    return Ok("Role added to User");
                }
                else
                {
                    return BadRequest("Error while adding Role to User");
                }
            } catch (Exception ex)
            {
                if(ex.Message.Equals("User does not exist"))
                {
                    return NotFound("User does not exist");
                } 
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }

        // Remove User to role
        [HttpPost]
        [Route("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserFromRole(string userId, string roleName)
        {
            try
            {
                IdentityResult result = await _roleManager.RemoveUserFromRole(userId, roleName);
                if (result.Succeeded)
                {

                    return Ok("Role removed from user");
                }
                else
                {
                    return BadRequest("Error while removing Role from User");
                }
            } catch (Exception ex)
            {
                if (ex.Message.Equals("User does not exist"))
                {
                    return NotFound("User does not exist");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }

        // Get specific user role
        [HttpGet]
        [Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var roles = await _roleManager.GetUserRoles(userId);
            return Ok(roles);
        }
    }
}
