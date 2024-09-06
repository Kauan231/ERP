using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Inventory
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
        public string? userId { get; set; }
        public virtual User Users { get; set; }
    }
}
