using Microsoft.AspNetCore.Mvc;
using ERP.Data.Dtos;
using ERP.Repositories;
using ERP.Models;

namespace ERP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IShipmentRepository _shipmentRepository;

        public ProductController(IProductRepository productRepository, IShipmentRepository shipmentRepository)
        {
            _productRepository = productRepository;
            _shipmentRepository = shipmentRepository;
        }

        // GET <ProductController>
        [HttpGet]
        public ActionResult<List<Product>> ReadAll()
        {
            List<Product> products = _productRepository.ReadAll();
            return Ok(products);
        }

        // GET <ProductController>/5
        [HttpGet("{id}")]
        public ActionResult<ReadProductDto> Get(string id)
        {
            ReadProductDto readProductDto = _productRepository.Read(id);
            return Ok(readProductDto);
        }

        // GET <ProductController>/5/Shipments
        [HttpGet("{id}/Shipments")]
        public ActionResult<List<Shipment>> GetShipments(string id)
        {
            List<Shipment> readShipmentDto = _shipmentRepository.ReadAllShipmentsOfAProduct(id);
            return Ok(readShipmentDto);
        }

        // POST <ProductController>
        [HttpPost]
        public ActionResult<Product> Post([FromBody] CreateProductDto dto)
        {
            Product product = _productRepository.Create(dto);
            _productRepository.SaveChanges();
            return Ok(product);
        }

        // DELETE <ProductController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            _productRepository.Delete(id);
            _productRepository.SaveChanges();
            return Ok(id);
        }
    }
}
