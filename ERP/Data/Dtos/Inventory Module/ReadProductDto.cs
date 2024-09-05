using ERP.Models;
using System.ComponentModel.DataAnnotations;

namespace ERP.Data.Dtos
{
    public class ReadProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public string inventoryId { get; set; }
    }
}
