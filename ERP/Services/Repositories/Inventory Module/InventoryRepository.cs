using AutoMapper;
using ERP.Data;
using ERP.Data.Dtos;
using ERP.Data.Dtos.Domain;
using ERP.Migrations;
using ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ErpContext _context;
        private readonly IMapper _mapper;
        public InventoryRepository(ErpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Inventory Create(CreateInventoryDto createDto)
        {
            Inventory inventory = _mapper.Map<Inventory>(createDto);
            var guid = Guid.NewGuid().ToString();
            inventory.Id = guid;
            _context.Inventories.Add(inventory);
            return inventory;
        }

        public ReadInventoryDto Read(string id)
        {
            Inventory inventory = _context.Inventories.Include(inventory => inventory.InventoryItems).SingleOrDefault(inventory => inventory.Id == id);
            ReadInventoryDto dto = _mapper.Map<ReadInventoryDto>(inventory);
            return dto;
        }

        public List<ReadInventoryDto> Read(string userId, string search, int skip, int limit)
        {
            var userBusinessIds = _context.Users
            .Include(u => u.Businesses)
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Businesses.Select(b => b.Id))
            .ToList();

            List<Inventory> inventories = _context.Inventories
                .Include(inv => inv.Businesses)
                .Where(i => userBusinessIds.Contains(i.Businesses.Id))
                .Where(inventory => EF.Functions.Like(inventory.Name, $"%{search}%"))
                .Skip(skip)
                .Take(limit)
                .ToList();
            List<ReadInventoryDto> dtoList = _mapper.Map<List<ReadInventoryDto>>(inventories);
            return dtoList;
        }

        public List<ReadInventorySimpleDto> ReadAll(string userId)
        {
            var userBusinessIds = _context.Users
            .Include(u => u.Businesses)
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Businesses.Select(b => b.Id))
            .ToList();

            List<Inventory> inventories = _context.Inventories
            .Include(inv => inv.Businesses)
            .Where(i => userBusinessIds.Contains(i.Businesses.Id))
            .ToList();

            List<ReadInventorySimpleDto> readInventories = new List<ReadInventorySimpleDto>();
            foreach (Inventory inventory in inventories)
            {
                ReadInventorySimpleDto readDto = new ReadInventorySimpleDto();
                readDto.Id = inventory.Id;
                readDto.Name = inventory.Name;
                readInventories.Add(readDto);
            }

            return readInventories;
        }

        public List<Inventory> ReadAllBusinessInventories(string businessId)
        {
            List<Inventory> readInventoryDtos = _context.Inventories.Where(inventory => inventory.businessId == businessId).ToList();
            return readInventoryDtos;
        }

        public void Delete(string id)
        {
            var inventory = _context.Inventories
                .Include(i => i.InventoryItems)
                .Include(i => i.Shipments)
                    .ThenInclude(s => s.Orders)
                .Include(i => i.Shipments)
                    .ThenInclude(s => s.OrderItems)
                .SingleOrDefault(x => x.Id == id);

            if (inventory != null)
            {
                inventory.businessId = null;

                if (inventory.InventoryItems != null && inventory.InventoryItems.Any())
                {
                    _context.InventoryItems.RemoveRange(inventory.InventoryItems);
                }

                if (inventory.Shipments != null && inventory.Shipments.Any())
                {
                    // Remove OrderItems relacionados
                    foreach (var shipment in inventory.Shipments)
                    {
                        if (shipment.OrderItems != null && shipment.OrderItems.Any())
                        {
                            _context.OrderItems.RemoveRange(shipment.OrderItems);
                        }

                        // Remove Orders relacionados
                        if (shipment.Orders != null && shipment.Orders.Any())
                        {
                            _context.Orders.RemoveRange(shipment.Orders);
                        }
                    }

                    _context.Shipments.RemoveRange(inventory.Shipments);
                }

                _context.Inventories.Remove(inventory);
                _context.SaveChanges();
            }
        }



        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
