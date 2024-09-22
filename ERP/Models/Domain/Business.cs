using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Domain
{
    public class Business
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        public string? userId { get; set; }
        public virtual User Users { get; set; }
        public virtual ICollection<Inventory>? Inventories { get; set; }
        public virtual ICollection<Client>? Clients { get; set; }
    }
}
