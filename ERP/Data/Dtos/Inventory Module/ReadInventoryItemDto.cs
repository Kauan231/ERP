namespace ERP.Data.Dtos
{
    public class ReadInventoryItemDto
    {
        public string Id { get; set; }
        public int Amount { get; set; }
        public string? productId { get; set; }
        public string? inventoryId { get; set; }
    }
}
