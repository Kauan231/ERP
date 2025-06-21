using ERP.Data.Dtos;
using ERP.Models;

namespace ERP.Repositories
{
    public interface IShipmentRepository
    {
        Shipment Receive(CreateShipmentDto createDto);
        ReadShipmentDto Read(string id);
        List<ShipmentDto> Read(string businessId, int skip, int limit);
        int Count(string businessId);
        Shipment Send(CreateShipmentDto sendDto);
        List<Shipment> ReadAllShipmentsOfAProduct(string id);
        Shipment TransferToAnotherInventory(TransferDtoMany transferDto);
        void Delete(string id);
        void SaveChanges();
    }
}
