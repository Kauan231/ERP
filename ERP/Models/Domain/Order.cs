using System.ComponentModel.DataAnnotations;

namespace ERP.Models.Domain
{
    public class Order
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string? Status { get; set; }
        public string clientId { get; set; }
        public virtual Client client { get; set; }
        public string? shipmentId { get; set; }
        public virtual Shipment Shipment { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
