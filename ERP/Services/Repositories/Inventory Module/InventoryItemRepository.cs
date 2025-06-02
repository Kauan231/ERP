using AutoMapper;
using ERP.Data;
using ERP.Data.Dtos;
using ERP.Migrations;
using ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Repositories
{
    public class InventoryItemRepository : IInventoryItemRepository
    {
        private readonly ErpContext _context;
        private readonly IMapper _mapper;
        public InventoryItemRepository(ErpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public InventoryItem Create(CreateInventoryItemDto createDto)
        {
            InventoryItem inventoryItemSearch = _context.InventoryItems.SingleOrDefault(inventoryItem => inventoryItem.productId == createDto.productId && inventoryItem.inventoryId == createDto.inventoryId);
            if (inventoryItemSearch != null) throw new Exception("Inventory Item already exists");
            InventoryItem inventoryItem = _mapper.Map<InventoryItem>(createDto);
            var guid = Guid.NewGuid().ToString();
            inventoryItem.Id = guid;
            _context.InventoryItems.Add(inventoryItem);
            return inventoryItem;
        }

        public void SubtractAmount(string productId, string inventoryId, int amount)
        {
            InventoryItem inventoryItem = _context.InventoryItems.FirstOrDefault(inventoryItem => inventoryItem.productId == productId && inventoryItem.inventoryId == inventoryId);
            if (inventoryItem == null) throw new Exception("Inventory item does not exist");
            if (inventoryItem.Amount - amount < 0) throw new Exception("Greater than avaliable amount");
            inventoryItem.Amount -= amount;
        }

        public void AddAmount(string productId, string inventoryId, int amount)
        {
            InventoryItem inventoryItem = _context.InventoryItems.FirstOrDefault(inventoryItem => inventoryItem.productId == productId && inventoryItem.inventoryId == inventoryId);
            if (inventoryItem == null)
            {
                InventoryItem inventoryItemToAdd = new InventoryItem();
                var guid = Guid.NewGuid().ToString();
                inventoryItemToAdd.Id = guid;
                inventoryItemToAdd.productId = productId;
                inventoryItemToAdd.Amount = amount;
                inventoryItemToAdd.inventoryId = inventoryId;
                _context.InventoryItems.Add(inventoryItemToAdd);
                return;

            }
            inventoryItem.Amount += amount;
        }

        public InventoryItem ModifyItem(InventoryItem inventoryItemDto)
        {
            InventoryItem inventoryItemInInventory = _context.InventoryItems.SingleOrDefault(inventoryItem => inventoryItem.Id == inventoryItemDto.Id);
            if (inventoryItemInInventory == null) throw new Exception("Inventory item does not exist");
            inventoryItemInInventory = inventoryItemDto;
            return inventoryItemInInventory;
        }

        public InventoryItem Read(string id)
        {
            InventoryItem inventoryItem = _context.InventoryItems.FirstOrDefault(x => x.Id == id);
            return inventoryItem;
        }

        public ReadInventoryTableDto ReadAll(string productName = "", List<string>? inventoryIds = null, int skip = 0, int limit = 10)
        {
            IQueryable<InventoryItem> query = _context.InventoryItems
                                                .Include(i => i.Products)
                                                .Include(i => i.Inventories);

            if (!string.IsNullOrEmpty(productName))
            {
                query = _context.InventoryItems
                .Include(i => i.Products)
                .Include(i => i.Inventories)
                .Where(invItem => EF.Functions.Like(invItem.Products.Name, $"%{productName}%"));
            }

            // Filtro por lista de IDs de inventário
            if (inventoryIds != null && inventoryIds.Any())
            {
                query = query.Where(i => inventoryIds.Contains(i.Inventories.Id));
            }

            // Paginação
            List<InventoryItem> inventoryItems = query
                .Skip(skip)
                .Take(limit)
                .ToList();

            // Mapeamento para DTOs
            List<InventoryItemDto> dtos = inventoryItems.Select(item => new InventoryItemDto
            {
                Id = item.Id,
                Amount = item.Amount,
                Product = new ReadProductDto
                {
                    Id = item.Products?.Id,
                    Name = item.Products?.Name,
                    Description = item.Products?.Description
                },
                Inventory = new ReadInventorySimpleDto
                {
                    Id = item.Inventories?.Id,
                    Name = item.Inventories?.Name
                }
            }).ToList();
            ReadInventoryTableDto readInventoryTableDto = new ReadInventoryTableDto();
            readInventoryTableDto.totalOfItems = query.Where(inventoryItem => inventoryItem.Amount > 0).Count();
            readInventoryTableDto.AllInventoryItems = dtos;

            return readInventoryTableDto;
        }


        public void Delete(string id)
        {
            InventoryItem inventoryItem = _context.InventoryItems.FirstOrDefault(x => x.Id == id);
            if (inventoryItem != null)
            {
                _context.InventoryItems.Remove(inventoryItem);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
