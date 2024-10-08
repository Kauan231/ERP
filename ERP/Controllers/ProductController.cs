﻿using Microsoft.AspNetCore.Mvc;
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

        // GET <ProductController>/5
        [HttpGet("{id}")]
        public ReadProductDto Get(string id)
        {
            ReadProductDto readProductDto = _productRepository.Read(id);
            return readProductDto;
        }

        // GET <ProductController>/5
        [HttpGet("{id}/Shipments")]
        public List<Shipment> GetShipments(string id)
        {
            List<Shipment> readShipmentDto = _shipmentRepository.ReadAllShipments(id);
            return readShipmentDto;
        }

        // POST <ProductController>
        [HttpPost]
        public Product Post([FromBody] CreateProductDto dto)
        {
            Product product = _productRepository.Create(dto);
            _productRepository.SaveChanges();
            return product;
        }

        // DELETE <ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _productRepository.Delete(id);
            _productRepository.SaveChanges();
        }
    }
}
