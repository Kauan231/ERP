using ERP.Data.Dtos;
using ERP.Repositories;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ERP.Services.Authentication;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;
        private UserService _UserService;
        private TokenService _tokenService;

        public UserController(IInventoryRepository inventoryRepository, UserService userService, TokenService tokenService)
        {
            _inventoryRepository = inventoryRepository;
            _UserService = userService;
            _tokenService = tokenService;
        }

        // GET <UserController>/5
        [Authorize]
        [HttpGet("Inventories")]
        public List<Inventory> GetAll()
        {
            List<Inventory> readInventoryDto = _inventoryRepository.ReadAllUserInventories(this.User.Claims.First(i => i.Type == "id").Value);
            _inventoryRepository.SaveChanges();
            return readInventoryDto;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser
            ([FromBody] CreateUserDto dto)
        {
            await _UserService.SignUser(dto);
            return Ok("User registered with success");

        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserDto dto)
        {
            User loggedUser = await _UserService.Login(dto);
            var token = _tokenService.GenerateToken(loggedUser);
            return Ok(token);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromQuery] RemoveUserDto dto)
        {
            await _UserService.RemoveUser(dto);
            return Ok("User removed");
        }
    }
}
