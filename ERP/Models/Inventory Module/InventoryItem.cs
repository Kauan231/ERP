using ERP.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class InventoryItem
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public int Amount { get; set; }
        public string? productId { get; set; }
        public virtual Product Product { get; set; }
        public string? inventoryId { get; set; }
        public virtual Inventory Inventory { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
