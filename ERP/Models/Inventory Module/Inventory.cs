using System.ComponentModel.DataAnnotations;
using ERP.Models.Domain;

namespace ERP.Models
{
    public class Inventory
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
        public string? businessId { get; set; }
        public virtual Business Businesses { get; set; }
    }
}
