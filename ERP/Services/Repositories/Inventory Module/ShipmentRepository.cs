using AutoMapper;
using ERP.Data;
using ERP.Data.Dtos;
using ERP.Models;
using ERP.Models.Domain;

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
            Shipment shipment = new Shipment();
            shipment.Id = Guid.NewGuid().ToString();
            shipment.Type = "RECEIVE";
            shipment.Status = "FINISHED";
            shipment.shipmentDate = DateTime.Now;

            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (CreateOrderItemDto orderItem in createDto.OrderItems)
            {
                _productRepository.AddAmount(orderItem.productId, orderItem.Amount);
                OrderItem orderItemToAdd = new OrderItem();
                orderItemToAdd.Id = Guid.NewGuid().ToString();
                orderItemToAdd.Amount = orderItem.Amount;
                orderItemToAdd.productId = orderItem.productId;
                orderItemToAdd.shipmentId = shipment.Id;
                _context.OrderItems.Add(orderItemToAdd);
                orderItems.Add(orderItemToAdd);
            }

            shipment.OrderItems = orderItems;

            _context.Shipments.Add(shipment);
            return shipment;
        }

        public Shipment Send(CreateShipmentDto sendDto)
        {
            Shipment shipment = new Shipment();
            var shipmentGuid = Guid.NewGuid().ToString();
            shipment.Id = shipmentGuid;
            shipment.Type = "SEND";
            shipment.Status = "Finished";
            shipment.shipmentDate = DateTime.Now;

            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (CreateOrderItemDto orderItem in sendDto.OrderItems)
            {
                _productRepository.SubtractAmount(orderItem.productId, orderItem.Amount);
                OrderItem orderItemToAdd = new OrderItem();
                orderItemToAdd.Id = Guid.NewGuid().ToString();
                orderItemToAdd.Amount = orderItem.Amount;
                orderItemToAdd.productId = orderItem.productId;
                orderItemToAdd.shipmentId = shipment.Id;
                _context.OrderItems.Add(orderItemToAdd);
                orderItems.Add(orderItemToAdd);
            }
            shipment.OrderItems = orderItems;
            _context.Shipments.Add(shipment);

            List<string> productIds = sendDto.OrderItems.Select(item => item.productId).ToList();
            List<Product> products = _context.Products
                                            .Where(p => productIds.Contains(p.Id))
                                            .ToList();

            Order orderForClient = new Order();
            orderForClient.Id = Guid.NewGuid().ToString();
            orderForClient.clientId = sendDto.clientId;
            orderForClient.Products = products;
            orderForClient.shipmentId = shipment.Id;
            _context.Orders.Add(orderForClient);

            return shipment;
        }

        public Shipment TransferToAnotherInventory(TransferDto transferDto)
        {
            _productRepository.SubtractAmount(transferDto.productId, transferDto.Amount);

            var shipment = new Shipment
            {
                Id = Guid.NewGuid().ToString(),
                Type = "TRANSFER",
                Status = "PLACED",
                shipmentDate = DateTime.Now,
                OrderItems = new List<OrderItem>()
            };

            _context.Shipments.Add(shipment);
            _context.SaveChanges();

            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid().ToString(),
                Amount = transferDto.Amount,
                Shipment = shipment
            };

            var checkInventory = _context.Products
                .SingleOrDefault(x => x.Id == transferDto.productId && x.inventoryId == transferDto.toInventoryId);

            if (checkInventory == null)
            {
                var originalProduct = _productRepository.Read(transferDto.productId);
                if (originalProduct == null)
                    throw new Exception("Cannot find original product");

                CreateProductDto newProductDto = new CreateProductDto();
                newProductDto.Name = originalProduct.Name;
                newProductDto.Description = originalProduct.Description;
                newProductDto.Amount = transferDto.Amount;
                newProductDto.inventoryId = transferDto.toInventoryId;

                var createdProduct = _productRepository.Create(newProductDto);
                orderItem.productId = createdProduct.Id;
            }
            else
            {
                checkInventory.Amount += transferDto.Amount;
                orderItem.productId = transferDto.productId;
            }

            shipment.OrderItems.Add(orderItem);
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
            List<OrderItem> orderItemsByProductId = _context.OrderItems.Where(orderItem => orderItem.productId == id).ToList();
            List<String> shipmentsFromOrderItemsByProductId = new List<string>();
            foreach (var item in orderItemsByProductId)
            {
                shipmentsFromOrderItemsByProductId.Add(item.productId);
            }
            List<Shipment> shipments = _context.Shipments.Where(x => shipmentsFromOrderItemsByProductId.Contains(x.Id)).ToList();
            return shipments;
        }

        public void Delete(string id)
        {
            Shipment shipment = _context.Shipments.SingleOrDefault(x => x.Id == id);
            if (shipment != null)
            {
                foreach (OrderItem orderItem in shipment.OrderItems)
                {
                    _productRepository.SubtractAmount(orderItem.productId, orderItem.Amount);
                }
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
