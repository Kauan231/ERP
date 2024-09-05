using ERP.Data.Dtos;
using ERP.Repositories;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUserRepository _userRepository;

        public UserController(IInventoryRepository inventoryRepository, IUserRepository userRepository)
        {
            _inventoryRepository = inventoryRepository;
            _userRepository = userRepository;;
        }

        // GET <UserController>/5
        [HttpGet("{id}/Inventories")]
        public List<Inventory> GetAll(string id)
        {
            List<Inventory> readInventoryDto = _inventoryRepository.ReadAllUserInventories(id);
            _inventoryRepository.SaveChanges();
            return readInventoryDto;
        }

        [HttpPost]
        public User Post(CreateUserDto createUserDto)
        {
            User user = _userRepository.Create(createUserDto);
            _userRepository.SaveChanges();
            return user;
        }
    }
}
