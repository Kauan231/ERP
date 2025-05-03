using ERP.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Product
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }

        public string? inventoryId { get; set; }
        public virtual Inventory Inventories { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
