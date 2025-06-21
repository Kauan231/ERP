using Microsoft.AspNetCore.Mvc;
using ERP.Data.Dtos;
using ERP.Repositories;
using ERP.Models;

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
        public ActionResult<ReadShipmentDto> Get(string id)
        {
            ReadShipmentDto readShipmentDto = _shipmentRepository.Read(id);
            return Ok(readShipmentDto);
        }

        // GET <ShipmentController>
        [HttpGet]
        public ActionResult<ReadAllShipmentsDto> ReadAll(
            [FromQuery] string? businessId = "",
            [FromQuery] int skip = 0,
            [FromQuery] int limit = 10,
            [FromQuery] string? search = ""
        )
        {
            List<ShipmentDto> shipments = _shipmentRepository.Read(businessId, skip, limit);
            int totalOfItems = _shipmentRepository.Count(businessId);
            ReadAllShipmentsDto readAllShipmentsDto = new ReadAllShipmentsDto();
            readAllShipmentsDto.shipments = shipments;
            readAllShipmentsDto.totalOfItems = totalOfItems;
            return Ok(readAllShipmentsDto);
        }

        // POST <ShipmentController>/Receive
        [HttpPost("Receive")]
        public ActionResult<Shipment> Receive([FromBody] CreateShipmentDto dto)
        {
            Shipment shipment = _shipmentRepository.Receive(dto);
            _shipmentRepository.SaveChanges();
            return Ok(shipment);
        }

        // POST <ShipmentController>/Send
        [HttpPost("Send")]
        public ActionResult<Shipment> Send([FromBody] CreateShipmentDto dto)
        {
            Shipment shipment = _shipmentRepository.Send(dto);
            try
            {
                _shipmentRepository.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(shipment);
        }

        // POST <ShipmentController>/Transfer
        [HttpPost("Transfer")]
        public ActionResult<Shipment> Transfer([FromBody] TransferDtoMany dto)
        {
            Shipment shipment = _shipmentRepository.TransferToAnotherInventory(dto);
            _shipmentRepository.SaveChanges();
            return Ok(shipment);
        }

        // DELETE <ShipmentController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            _shipmentRepository.Delete(id);
            _shipmentRepository.SaveChanges();
            return Ok(id);
        }
    }
}
