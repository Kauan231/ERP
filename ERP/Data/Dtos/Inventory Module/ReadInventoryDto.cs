using ERP.Models;

namespace ERP.Data.Dtos
{
    public class ReadInventoryDto
    {
        public string Id { get; set; }
        public ICollection<InventoryItem> InventoryItems { get; set; }
    }
}
