using ERP.Data.Dtos;
using ERP.Repositories;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using ERP.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ERP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly UserManager<User> _userManager;
        private UserService _UserService;
        private TokenService _tokenService;

        public UserController(IInventoryRepository inventoryRepository, UserService userService, TokenService tokenService, UserManager<User> userManager)
        {
            _inventoryRepository = inventoryRepository;
            _UserService = userService;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser
            ([FromBody] CreateUserDto dto)
        {
            await _UserService.SignUser(dto);
            return Ok("User registered with success");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserDto dto)
        {
            User loggedUser = await _UserService.Login(dto);
            var token = await _tokenService.GenerateToken(loggedUser);
            return Ok(token);
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
        [Route("Roles")]
        public async Task<IActionResult> GetUserRoles()
        {
            var user = await _userManager.FindByIdAsync(this.User.Claims.First(i => i.Type == "id").Value);
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }
        
        [HttpGet("Inventories")]
        [Authorize]
        public List<Inventory> GetAllInventories()
        {
            List<Inventory> readInventoryDto = _inventoryRepository.ReadAllUserInventories(this.User.Claims.First(i => i.Type == "id").Value);
            _inventoryRepository.SaveChanges();
            return readInventoryDto;
        }


    }
}
