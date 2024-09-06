using ERP.Data.Dtos;
using ERP.Models;

namespace ERP.Repositories
{
    public interface IInventoryRepository
    {
        void SaveChanges();
        Inventory Create(CreateInventoryDto inventory);
        ReadInventoryDto Read(string id);
        void Delete(string id);
        List<Inventory> ReadAllUserInventories(string userId);
    }
}
