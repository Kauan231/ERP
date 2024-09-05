using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class User
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Inventory>? Inventories { get; set; }
    }
}
