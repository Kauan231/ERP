namespace ERP.Data.Dtos
{
    public class CreateOrderItemDto
    {
        public string? Id { get; set; }
        public int Amount { get; set; }
        public string productId { get; set; }
    }
}
