using ERP.Data.Dtos;
using ERP.Models;

namespace ERP.Repositories
{
    public interface IInventoryItemRepository
    {
        void SaveChanges();
        InventoryItem Create(CreateInventoryItemDto product);
        InventoryItem Read(string id);
        ReadInventoryTableDto ReadAll(List<string>? inventoryIds = null, int skip = 0, int limit = 10);
        void Delete(string id);
    }
}
