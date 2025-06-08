using ERP.Data.Dtos;
using ERP.Data.Dtos.Domain;
using ERP.Models;

namespace ERP.Repositories
{
    public interface IInventoryRepository
    {
        void SaveChanges();
        Inventory Create(CreateInventoryDto inventory);
        ReadInventoryDto Read(string id);
        List<ReadInventoryDto> Read(string userId, string search, int skip, int limit);
        List<ReadInventorySimpleDto> ReadAll(string userId);
        void Delete(string id);
        List<Inventory> ReadAllBusinessInventories(string businessId);
    }
}
