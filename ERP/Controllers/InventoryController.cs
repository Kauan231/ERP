using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ERP.Data.Dtos;
using ERP.Repositories;
using ERP.Models;
using ERP.Models.Domain;

namespace ERP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IInventoryItemRepository _inventoryItemRepository;

        public InventoryController(IInventoryRepository inventoryRepository, IInventoryItemRepository inventoryItemRepository)
        {
            _inventoryRepository = inventoryRepository;
            _inventoryItemRepository = inventoryItemRepository;
        }

        // GET <InventoryController>/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<ReadInventoryDto> Get(string id)
        {
            ReadInventoryDto readInventoryDto = _inventoryRepository.Read(id);
            return readInventoryDto;
        }

        // POST <InventoryController>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult<Inventory> Post(CreateInventoryDto createInventoryDto)
        {
            Inventory inventory = _inventoryRepository.Create(createInventoryDto);
            _inventoryRepository.SaveChanges();
            return Ok(inventory);
        }

        // POST <InventoryController>/InventoryItem/AddProduct
        [HttpPost]
        [Route("InventoryItem")]
        [Authorize(Roles = "admin")]
        public ActionResult<InventoryItem> AddInventoryItem(CreateInventoryItemDto createInventoryItemDto)
        {
            InventoryItem inventoryItem = _inventoryItemRepository.Create(createInventoryItemDto);
            _inventoryItemRepository.SaveChanges();
            return Ok(inventoryItem);
        }

        // GET <InventoryController>/InventoryItem/ReadAll
        [HttpGet]
        [Route("InventoryItem/ReadAll")]
        [Authorize(Roles = "admin")]
        public ActionResult<ReadInventoryTableDto> ReadAllInventoryItems(
            [FromQuery] List<string>? inventoryIds = null,
            [FromQuery] int skip = 0,
            [FromQuery] int limit = 10
        )
        {
            ReadInventoryTableDto inventoryItems = _inventoryItemRepository.ReadAll(inventoryIds, skip, limit);
            List<ReadInventorySimpleDto> inventories = _inventoryRepository.ReadAll();
            ReadInventoryTableDto tableContent = new ReadInventoryTableDto();
            tableContent.AllInventories = inventories;
            tableContent.AllInventoryItems = inventoryItems.AllInventoryItems;
            tableContent.totalOfItems = inventoryItems.totalOfItems;
            return Ok(tableContent);
        }

        // DELETE <InventoryController>/InventoryItem/5
        [HttpDelete]
        [Route("InventoryItem/{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult RemoveInventoryItem(string id)
        {
            _inventoryItemRepository.Delete(id);
            _inventoryItemRepository.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("InventoryItem/{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult<InventoryItem> ReadInventoryItem(string id)
        {
            InventoryItem inventoryItem = _inventoryItemRepository.Read(id);
            return Ok(inventoryItem);
        }

        // DELETE <InventoryController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(string id)
        {
            _inventoryRepository.Delete(id);
            _inventoryRepository.SaveChanges();
            return Ok();
        }
    }
}
