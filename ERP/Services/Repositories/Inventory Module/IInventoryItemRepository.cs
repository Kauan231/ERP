using ERP.Data.Dtos;
using ERP.Models;

namespace ERP.Repositories
{
    public interface IInventoryItemRepository
    {
        void SaveChanges();
        InventoryItem Create(CreateInventoryItemDto product);
        InventoryItem Read(string id);
        List<InventoryItem> ReadAll();
        void Delete(string id);
    }
}
