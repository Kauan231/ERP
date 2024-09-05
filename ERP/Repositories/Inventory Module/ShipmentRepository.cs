using AutoMapper;
using ERP.Data;
using ERP.Data.Dtos;
using ERP.Models;
using Microsoft.EntityFrameworkCore;

namespace ERP.Repositories
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly ErpContext _context;
        private readonly IMapper _mapper;
        private readonly ProductRepository _productRepository;
        public ShipmentRepository(ErpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _productRepository = new ProductRepository(_context, _mapper); 
        }

        public Shipment Receive(CreateShipmentDto createDto)
        {
            Shipment shipment = _mapper.Map<Shipment>(createDto);
            _productRepository.AddAmount(shipment.productId, createDto.Amount);

            var guid = Guid.NewGuid().ToString();
            shipment.Id = guid;
            shipment.Type = "RECEIVE";
            shipment.shipmentDate = DateTime.Now;

            _context.Shipments.Add(shipment);
            return shipment;
        }

        public Shipment Send(CreateShipmentDto sendDto)
        {
            Shipment shipment = _mapper.Map<Shipment>(sendDto);
            _productRepository.SubtractAmount(sendDto.productId, sendDto.Amount);

            var shipmentGuid = Guid.NewGuid().ToString();
            shipment.Id = shipmentGuid;
            shipment.Type = "SEND";
            shipment.shipmentDate = DateTime.Now;

            _context.Shipments.Add(shipment);
            return shipment;
        }

        public Shipment TransferToAnotherInventory(TransferDto transferDto)
        {
            _productRepository.SubtractAmount(transferDto.productId, transferDto.Amount);

            //Checks if destination already has the type of Product
            Product checkInventory = _context.Products.SingleOrDefault(x => x.Id == transferDto.productId && x.inventoryId == transferDto.toInventoryId);

            var shipmentGuid = Guid.NewGuid().ToString();
            Shipment shipment = new Shipment();
            shipment.Amount = transferDto.Amount;
            shipment.Type = "TRANSFER";
            shipment.Id = shipmentGuid;
            shipment.shipmentDate = DateTime.Now;

            // If it does not have then Create
            if (checkInventory == null)
            {
                CreateProductDto newProduct = _mapper.Map<CreateProductDto>(_productRepository.Read(transferDto.productId));
                newProduct.Amount = transferDto.Amount;
                newProduct.inventoryId = transferDto.toInventoryId;
                Product createdProduct = _productRepository.Create(newProduct);
                shipment.productId = createdProduct.Id;
            } 
            else
            {
                checkInventory.Amount += transferDto.Amount;
                shipment.productId = transferDto.productId;
            }

            _context.Shipments.Add(shipment);
            return shipment;
        }

        public ReadShipmentDto Read(string id)
        {
            Shipment shipment = _context.Shipments.FirstOrDefault(x => x.Id == id);
            ReadShipmentDto dto = _mapper.Map<ReadShipmentDto>(shipment);
            return dto;
        }

        public List<Shipment> ReadAllShipments(string id)
        {
            List<Shipment> shipments = _context.Shipments.Where(x => x.productId == id).ToList();
            return shipments;
        }

        public void Delete(string id)
        {
            Shipment shipment = _context.Shipments.SingleOrDefault(x => x.Id == id);
            if(shipment != null)
            {
                _context.Shipments.Remove(shipment);
                _productRepository.SubtractAmount(shipment.productId, shipment.Amount);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
