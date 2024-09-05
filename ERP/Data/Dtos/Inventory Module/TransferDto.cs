using ERP.Models;
using System.ComponentModel.DataAnnotations;

namespace ERP.Data.Dtos
{
    public class TransferDto
    {
        [Required]
        public int Amount { get; set; }
        [Required]
        public string productId { get; set; }
        [Required]
        public string toInventoryId { get; set; }
    }
}
