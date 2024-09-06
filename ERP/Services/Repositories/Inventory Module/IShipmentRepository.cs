using ERP.Data.Dtos;
using ERP.Models;

namespace ERP.Repositories
{
    public interface IShipmentRepository
    {
        Shipment Receive(CreateShipmentDto createDto);
        ReadShipmentDto Read(string id);
        Shipment Send(CreateShipmentDto sendDto);
        List<Shipment> ReadAllShipments(string id);
        Shipment TransferToAnotherInventory(TransferDto transferDto);
        void Delete(string id);
        void SaveChanges();
    }
}
