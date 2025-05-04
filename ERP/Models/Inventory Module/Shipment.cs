using System.ComponentModel.DataAnnotations;
using ERP.Models.Domain;

namespace ERP.Models
{
    public class Shipment
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime shipmentDate { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual string inventoryId { get; set; }
        public virtual Inventory Inventories { get; set; }
    }
}
