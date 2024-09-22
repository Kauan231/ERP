using ERP.Models.Domain;

namespace ERP.Data.Dtos.Domain
{
    public class ReadClientDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}