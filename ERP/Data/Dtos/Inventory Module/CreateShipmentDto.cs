using ERP.Models;
using System.ComponentModel.DataAnnotations;

namespace ERP.Data.Dtos
{
    public class CreateShipmentDto
    {
        [Required]
        public int Amount { get; set; }
        [Required]
        public string productId { get; set; }
    }
}
