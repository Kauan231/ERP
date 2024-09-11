using AutoMapper;
using ERP.Data;
using ERP.Data.Dtos;
using ERP.Data.Dtos.Domain;
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
            Inventory inventory = _context.Inventories.Include(x => x.Products).SingleOrDefault(x => x.Id == id);
            ReadInventoryDto dto = _mapper.Map<ReadInventoryDto>(inventory);
            return dto;
        }

        public List<Inventory> ReadAllBusinessInventories(string businessId) {
            List<Inventory> readInventoryDtos = _context.Inventories.Where(x => x.businessId == businessId).ToList();
            return readInventoryDtos;
        }

        public void Delete(string id)
        {
            Inventory inventory = _context.Inventories.SingleOrDefault(x => x.Id == id);
            if(inventory != null)
            {
                _context.Inventories.Remove(inventory);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
