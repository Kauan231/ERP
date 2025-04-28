using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Domain
{
    public class OrderItem
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public int Amount { get; set; }
        public string productId { get; set; }
        public virtual Product Product { get; set; }
        public string shipmentId { get; set; }
        public virtual Shipment Shipment { get; set; }

    }
}
