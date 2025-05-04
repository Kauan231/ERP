namespace ERP.Data.Dtos
{
    public class CreateShipmentDto
    {
        public List<CreateOrderItemDto> OrderItems { get; set; }
        public string clientId { get; set; }
        public string inventoryId { get; set; }
    }
}
