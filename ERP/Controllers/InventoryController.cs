using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ERP.Data.Dtos;
using ERP.Repositories;
using ERP.Models;

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
            return readInventoryDto;
        }

        // POST <InventoryController>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public Inventory Post(CreateInventoryDto createInventoryDto)
        {
            Inventory inventory = _inventoryRepository.Create(createInventoryDto);
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
