using ERP.Data.Dtos;
using ERP.Repositories;
using ERP.Models;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentRepository _shipmentRepository;

        public ShipmentController(IShipmentRepository shipmentRepository)
        {
            _shipmentRepository = shipmentRepository;
        }

        // GET <ShipmentController>/5
        [HttpGet("{id}")]
        public ReadShipmentDto Get(string id)
        {
            ReadShipmentDto readShipmentDto = _shipmentRepository.Read(id);
            _shipmentRepository.SaveChanges();
            return readShipmentDto;
        }

        // POST <ShipmentController>/Receive
        [HttpPost("Receive")]
        public Shipment Receive([FromBody] CreateShipmentDto dto)
        {
            Shipment shipment = _shipmentRepository.Receive(dto);
            _shipmentRepository.SaveChanges();
            return shipment;
        }

        // POST <ShipmentController>/Send
        [HttpPost("Send")]
        public Shipment Send([FromBody] CreateShipmentDto dto)
        {
            Shipment shipment = _shipmentRepository.Send(dto);
            _shipmentRepository.SaveChanges();
            return shipment;
        }

        // POST <ShipmentController>/Transfer
        [HttpPost("Transfer")]
        public Shipment Transfer([FromBody] TransferDto dto)
        {
            Shipment shipment = _shipmentRepository.TransferToAnotherInventory(dto);
            _shipmentRepository.SaveChanges();
            return shipment;
        }

        // DELETE <ShipmentController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _shipmentRepository.Delete(id);
            _shipmentRepository.SaveChanges();
        }
    }
}
