using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ERP.Models.Domain;

namespace ERP.Models
{
    public class Shipment
    {
        [Key]
        [Required]
        public string Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime shipmentDate { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
