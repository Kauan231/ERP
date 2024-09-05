using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ERP.Models
{
    public class Shipment
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public int Amount { get; set; }
        public string Type { get; set; }
        public DateTime shipmentDate { get; set; }

        public string? productId { get; set; }
        public virtual Product Products { get; set; }
    }
}
