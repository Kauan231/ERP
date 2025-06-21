using AutoMapper;
using ERP.Data;
using ERP.Data.Dtos;
using ERP.Models;
using ERP.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ERP.Repositories
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly ErpContext _context;
        private readonly IMapper _mapper;
        private readonly InventoryItemRepository _inventoryItemRepository;
        public ShipmentRepository(ErpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _inventoryItemRepository = new InventoryItemRepository(_context, _mapper);
        }

        public Shipment Receive(CreateShipmentDto createDto)
        {
            Shipment shipment = new Shipment();
            shipment.Id = Guid.NewGuid().ToString();
            shipment.Type = "RECEIVE";
            shipment.Status = "FINISHED";
            shipment.shipmentDate = DateTime.Now;
            shipment.inventoryId = createDto.inventoryId;

            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (CreateOrderItemDto orderItem in createDto.OrderItems)
            {
                _inventoryItemRepository.AddAmount(orderItem.productId, createDto.inventoryId, orderItem.Amount);
                _inventoryItemRepository.SaveChanges();
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
                _inventoryItemRepository.SubtractAmount(orderItem.productId, sendDto.inventoryId, orderItem.Amount);
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

            Order orderForClient = new Order();
            orderForClient.Id = Guid.NewGuid().ToString();
            orderForClient.clientId = sendDto.clientId;
            orderForClient.shipmentId = shipment.Id;
            _context.Orders.Add(orderForClient);

            return shipment;
        }

        public Shipment TransferToAnotherInventory(TransferDtoMany transferDtos)
        {
            var shipment = new Shipment
            {
                Id = Guid.NewGuid().ToString(),
                Type = "TRANSFER",
                Status = "PLACED",
                shipmentDate = DateTime.Now,
                OrderItems = new List<OrderItem>(),
                inventoryId = transferDtos.toInventoryId
            };

            _context.Shipments.Add(shipment);


            foreach (TransferDto transferDto in transferDtos.transferDtos)
            {
                InventoryItem inventoryItem = _context.InventoryItems.FirstOrDefault(inventoryItem => inventoryItem.Id == transferDto.Id);
                if (inventoryItem == null) throw new Exception("Inventory item does not exist");
                _inventoryItemRepository.SubtractAmount(transferDto.productId, transferDto.fromInventoryId, transferDto.Amount);

                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Amount = transferDto.Amount,
                    Shipment = shipment
                };

                var checkInventory = _context.InventoryItems
                    .SingleOrDefault(inventoryItem => inventoryItem.productId == transferDto.productId && inventoryItem.inventoryId == transferDto.toInventoryId);

                if (checkInventory == null)
                {
                    CreateInventoryItemDto newInventoryItemDto = new CreateInventoryItemDto();
                    newInventoryItemDto.productId = transferDto.productId;
                    newInventoryItemDto.Amount = transferDto.Amount;
                    newInventoryItemDto.inventoryId = transferDto.toInventoryId;

                    var createdProduct = _inventoryItemRepository.Create(newInventoryItemDto);
                    orderItem.productId = transferDto.productId;
                }
                else
                {
                    checkInventory.Amount += transferDto.Amount;
                    orderItem.productId = transferDto.productId;
                }

                orderItem.inventoryItemId = transferDto.Id;
                shipment.OrderItems.Add(orderItem);
            }

            _context.SaveChanges();
            return shipment;
        }

        public List<ShipmentDto> Read(string businessId, int skip, int limit)
        {
            var query = _context.Shipments
                .Where(shipment => shipment.Inventory.businessId == businessId)
                .Skip(skip)
                .Take(limit)
                .Select(shipment => new ShipmentDto
                {
                    Id = shipment.Id,
                    InventoryToId = shipment.Inventory.Id,
                    InventoryToName = shipment.Inventory.Name,
                    Type = shipment.Type,
                    Status = shipment.Status,
                    Date = shipment.shipmentDate,
                    OrderItems = shipment.OrderItems.Select(orderItem => new ReadOrderItemDto
                    {
                        ProductId = orderItem.Product.Id,
                        ProductName = orderItem.Product.Name,
                        Amount = orderItem.Amount,
                        InventoryFromId = orderItem.InventoryItem.Inventory.Id,
                        InventoryFromName = orderItem.InventoryItem.Inventory.Name
                    }).ToList()
                })
                .ToList();

            return query;
        }


        public int Count(string businessId)
        {
            return _context.Shipments
            .Include(shipment => shipment.Inventory)
            .Where(shipment => shipment.Inventory.businessId == businessId)
            .Count();
        }

        public ReadShipmentDto Read(string id)
        {
            Shipment shipment = _context.Shipments.FirstOrDefault(x => x.Id == id);
            ReadShipmentDto dto = _mapper.Map<ReadShipmentDto>(shipment);
            return dto;
        }

        public List<Shipment> ReadAllShipmentsOfAProduct(string id)
        {
            List<OrderItem> orderItemsByProductId = _context.OrderItems.Where(orderItem => orderItem.productId == id).ToList();
            List<String> shipmentsFromOrderItemsByProductId = new List<string>();
            foreach (var item in orderItemsByProductId)
            {
                shipmentsFromOrderItemsByProductId.Add(item.shipmentId);
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
                    _inventoryItemRepository.SubtractAmount(orderItem.productId, shipment.inventoryId, orderItem.Amount);
                }
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
