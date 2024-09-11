using ERP.Models;
using System.ComponentModel.DataAnnotations;

namespace ERP.Data.Dtos.Domain
{
    public class ReadBusinessDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Inventory>? Inventories { get; set; }
    }
}
