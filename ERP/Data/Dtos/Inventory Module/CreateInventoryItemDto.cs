using System.ComponentModel.DataAnnotations;

namespace ERP.Data.Dtos
{
    public class CreateInventoryItemDto
    {
        [Required]
        public int Amount { get; set; }
        [Required]
        public string productId { get; set; }
        [Required]
        public string inventoryId { get; set; }
    }
}
