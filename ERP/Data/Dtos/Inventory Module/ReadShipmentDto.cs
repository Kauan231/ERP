namespace ERP.Data.Dtos
{
    public class ReadShipmentDto
    {
        public string Id { get; set; }
        public int Amount { get; set; }
        public string Type { get; set; }
        public DateTime shipmentDate { get; set; }
        public string? productId { get; set; }
    }
}
