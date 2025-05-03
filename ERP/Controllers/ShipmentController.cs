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
        public ActionResult<Shipment> Transfer([FromBody] TransferDto dto)
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
