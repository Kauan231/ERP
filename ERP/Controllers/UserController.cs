
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ERP.Services.Authentication;
using ERP.Models.Domain;
using ERP.Data.Dtos;
using ERP.Repositories;
using ERP.Models;

namespace ERP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly UserManager<User> _userManager;
        private UserService _UserService;
        private TokenService _tokenService;
        public UserController(UserService userService, TokenService tokenService, UserManager<User> userManager, IBusinessRepository businessRepository)
        {
            _UserService = userService;
            _tokenService = tokenService;
            _userManager = userManager;
            _businessRepository = businessRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser
            ([FromBody] CreateUserDto dto)
        {
            try
            {
                await _UserService.SignUser(dto);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
            return Ok("User registered with success");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserDto dto)
        {
            User loggedUser;
            try
            {
                loggedUser = await _UserService.Login(dto);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
            var token = await _tokenService.GenerateToken(loggedUser);
            return Ok(new { token = token });
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteAsync([FromQuery] RemoveUserDto dto)
        {
            await _UserService.RemoveUser(dto);
            return Ok("User removed");
        }

        [HttpGet]
        [Authorize]
        [Route("GetUser")]
        public async Task<User> GetUser()
        {
            User user = await _userManager.FindByIdAsync(this.User.Claims.First(i => i.Type == "id").Value);
            return user;
        }

        // Get "My Roles"
        [HttpGet]
        [Authorize]
        [Route("Roles")]
        public async Task<IActionResult> GetUserRoles()
        {
            var user = await _userManager.FindByIdAsync(this.User.Claims.First(i => i.Type == "id").Value);
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        // Get "My Businesses"
        [HttpGet("Businesses")]
        [Authorize]
        public List<Business> GetAllBusinesses()
        {
            List<Business> readBusinessDto = _businessRepository.ReadAllUserBusinesses(this.User.Claims.First(i => i.Type == "id").Value);
            return readBusinessDto;
        }
    }
}
