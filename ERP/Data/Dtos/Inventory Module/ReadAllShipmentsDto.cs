using ERP.Models;
using ERP.Models.Domain;
namespace ERP.Data.Dtos
{
    public class ReadAllShipmentsDto
    {
        public List<ShipmentDto> shipments { get; set; }
        public int totalOfItems { get; set; }
    }
}
