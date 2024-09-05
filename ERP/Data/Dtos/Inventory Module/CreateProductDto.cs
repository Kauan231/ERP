using System.ComponentModel.DataAnnotations;

namespace ERP.Data.Dtos
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public string inventoryId { get; set; }
    }
}
