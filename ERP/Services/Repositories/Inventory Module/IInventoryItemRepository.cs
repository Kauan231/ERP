using ERP.Data.Dtos;
using ERP.Models;

namespace ERP.Repositories
{
    public interface IInventoryItemRepository
    {
        void SaveChanges();
        InventoryItem Create(CreateInventoryItemDto product);
        InventoryItem Read(string id);
        List<InventoryItemDto> ReadAll(List<string>? inventoryIds, int skip, int limit);
        void Delete(string id);
    }
}
