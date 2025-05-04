using ERP.Migrations;
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
        public virtual string businessId { get; set; }
        public virtual Business Business { get; set; }
        public virtual ICollection<InventoryItem>? InventoryItems { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
