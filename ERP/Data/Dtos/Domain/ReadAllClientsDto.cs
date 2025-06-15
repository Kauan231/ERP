using ERP.Models.Domain;

namespace ERP.Data.Dtos.Domain
{
    public class ReadAllClientDto
    {
        public List<Client> clients { get; set; }
        public int totalOfItems { get; set; }
    }
}