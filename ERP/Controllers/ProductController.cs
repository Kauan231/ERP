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
        public ActionResult<ReadProductTableDto> ReadAll(
            [FromQuery] string? search = "",
            [FromQuery] int skip = 0,
            [FromQuery] int limit = 10
        )
        {
            var userIdClaim = User.Claims.FirstOrDefault(i => i.Type == "id");
            var userId = userIdClaim.Value;

            List<Product> products = new List<Product>();
            int count;
            if (search?.Length > 0)
            {
                products = _productRepository.ReadAllWithFilter(userId, search, skip, limit);
                count = _productRepository.CountWithFilter(userId, search);
            }
            else
            {
                products = _productRepository.ReadAll(userId, skip, limit);
                count = _productRepository.Count(userId);
            }

            ReadProductTableDto tableContent = new ReadProductTableDto();

            tableContent.AllProducts = products;
            tableContent.totalOfItems = count;

            return Ok(tableContent);
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
