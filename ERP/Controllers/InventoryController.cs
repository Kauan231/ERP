using ERP.Data.Dtos;
using ERP.Repositories;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ERP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryController(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        // GET <InventoryController>/5
        [HttpGet("{id}")]
        [Authorize]
        public ReadInventoryDto Get(string id)
        {
            ReadInventoryDto readInventoryDto = _inventoryRepository.Read(id);
            _inventoryRepository.SaveChanges();
            return readInventoryDto;
        }

        // POST <InventoryController>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public Inventory Post(string InventoryName)
        {
            CreateInventoryDto createdInventoryDto = new CreateInventoryDto();
            createdInventoryDto.userId = this.User.Claims.First(i => i.Type == "id").Value;
            createdInventoryDto.Name = InventoryName;
            Inventory inventory = _inventoryRepository.Create(createdInventoryDto);
            _inventoryRepository.SaveChanges();
            return inventory;
        }

        // DELETE <InventoryController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public void Delete(string id)
        {
            _inventoryRepository.Delete(id);
            _inventoryRepository.SaveChanges();
        }
    }
}
