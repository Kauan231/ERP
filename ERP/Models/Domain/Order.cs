using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Domain
{
    public class Order
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Status { get; set; }
        public string? shipmentId { get; set; }
        public virtual Shipment Shipment { get; set; }
    }
}
