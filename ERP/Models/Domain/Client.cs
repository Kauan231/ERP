using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Domain
{
    public class Client
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        public string? businessId { get; set; }
        public virtual Business Business { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
