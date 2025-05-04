using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Domain
{
    public class OrderItem
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public int Amount { get; set; }
        public virtual string? productId { get; set; }
        public virtual Product? Product { get; set; }
        public virtual string? inventoryItemId { get; set; }
        public virtual InventoryItem? InventoryItem { get; set; }
        public virtual string? shipmentId { get; set; }
        public virtual Shipment? Shipment { get; set; }

    }
}
