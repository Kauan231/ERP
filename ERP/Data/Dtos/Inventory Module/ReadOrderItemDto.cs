namespace ERP.Data.Dtos
{
    public class ReadOrderItemDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string InventoryFromId { get; set; }
        public string InventoryFromName { get; set; }
        public int Amount { get; set; }
    }
}
