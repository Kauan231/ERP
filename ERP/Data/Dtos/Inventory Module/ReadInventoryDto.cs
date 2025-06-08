using ERP.Models;

namespace ERP.Data.Dtos
{
    public class ReadInventoryDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<InventoryItem> InventoryItems { get; set; }
    }

    public class ReadInventorySimpleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
