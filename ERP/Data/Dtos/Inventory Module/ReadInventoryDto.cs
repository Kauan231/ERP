using ERP.Models;
using System.ComponentModel.DataAnnotations;

namespace ERP.Data.Dtos
{
    public class ReadInventoryDto
    {
        public string Id { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
