using ERP.Data.Dtos;
using ERP.Repositories;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public ReadInventoryDto Get(string id)
        {
            ReadInventoryDto readInventoryDto = _inventoryRepository.Read(id);
            _inventoryRepository.SaveChanges();
            return readInventoryDto;
        }

        // POST <InventoryController>
        [HttpPost]
        public Inventory Post([FromBody] CreateInventoryDto dto)
        {
            Inventory inventory = _inventoryRepository.Create(dto);
            _inventoryRepository.SaveChanges();
            return inventory;
        }

        // DELETE <InventoryController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _inventoryRepository.Delete(id);
            _inventoryRepository.SaveChanges();
        }
    }
}
