namespace ERP.Data.Dtos
{
    public class ReadInventoryItemDto
    {
        public string Id { get; set; }
        public int Amount { get; set; }
        public string? productId { get; set; }
        public string? inventoryId { get; set; }
    }

    public class InventoryItemDto
    {
        public string Id { get; set; }
        public int Amount { get; set; }

        public ReadProductDto Product { get; set; }
        public ReadInventorySimpleDto Inventory { get; set; }
    }

    public class ReadInventoryTableDto
    {
        public List<InventoryItemDto> AllInventoryItems { get; set; }
        public List<ReadInventorySimpleDto> AllInventories { get; set; }
        public int totalOfItems { get; set; }
    }
}
