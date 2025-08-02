using AutoMapper;
using ERP.Data;
using ERP.Data.Dtos;
using ERP.Profiles;
using ERP.Models;
using ERP.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ERP.Data.Dtos.Domain;
using ERP.Models.Domain;

namespace ErpTests
{
    public class ShipmentTests : IDisposable
    {
        private readonly ErpContext _context;
        private readonly InventoryRepository _inventoryRepository;
        private readonly ShipmentRepository _shipmentRepository;
        private readonly InventoryItemRepository _inventoryItemRepository;
        private readonly Mapper _mapper;

        public ShipmentTests()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "ErpDevShipment.db" };
            var connectionString = connectionStringBuilder.ToString();

            var options = new DbContextOptionsBuilder<ErpContext>()
                .UseSqlite("DataSource=ErpDevShipment.db")
                .Options;

            _context = new ErpContext(options);
            _context.Database.Migrate();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new InventoryProfile());
                cfg.AddProfile(new ProductProfile());
                cfg.AddProfile(new ShipmentProfile());
                cfg.AddProfile(new InventoryItemProfile());
            });
            _mapper = new Mapper(config);

            _inventoryRepository = new InventoryRepository(_context, _mapper);
            _shipmentRepository = new ShipmentRepository(_context, _mapper);
            _inventoryItemRepository = new InventoryItemRepository(_context, _mapper);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        public bool CheckAmount(Product product, string inventoryID, int amountExpected)
        {
            ReadInventoryDto readInventoryDto = _inventoryRepository.Read(inventoryID);
            bool isAmount = false;

            foreach (InventoryItem inventoryItem in readInventoryDto.InventoryItems)
            {

                if (inventoryItem.productId == product.Id)
                {
                    isAmount = (inventoryItem.Amount == amountExpected);
                }
            }

            return isAmount;
        }

        [Fact]
        public void Integrate()
        {
            //Arrange
            User user = Mocks.TestUser();
            User user1 = Mocks.TestUser();

            _context.Users.Add(user);
            _context.Users.Add(user1);
            _context.SaveChanges();

            Business business = Mocks.TestBusiness(user.Id);
            _context.Businesses.Add(business);
            _context.SaveChanges();

            Business business1 = Mocks.TestBusiness(user1.Id);
            _context.Businesses.Add(business1);
            _context.SaveChanges();

            Inventory createdInventory = Mocks.TestInventory(business.Id);
            Inventory createdInventory1 = Mocks.TestInventory(business1.Id);
            _context.Inventories.Add(createdInventory);
            _context.Inventories.Add(createdInventory1);
            _context.SaveChanges();

            Product createdProduct = Mocks.TestProduct(business.Id);
            _context.Products.Add(createdProduct);
            _context.SaveChanges();

            InventoryItem inventoryItemToCreateInFirstInventory = Mocks.TestInventoryItem(createdProduct.Id, 0, createdInventory.Id);
            _context.InventoryItems.Add(inventoryItemToCreateInFirstInventory);
            _context.SaveChanges();

            //Act & Assert
            Assert.True(CheckAmount(createdProduct, createdInventory.Id, 0));

            CreateOrderItemDto orderItem = new CreateOrderItemDto();
            orderItem.Amount = 10;
            orderItem.productId = createdProduct.Id;

            CreateShipmentDto createShipmentDto = new CreateShipmentDto();
            createShipmentDto.OrderItems = new List<CreateOrderItemDto>();
            createShipmentDto.OrderItems.Add(orderItem);
            createShipmentDto.inventoryId = createdInventory.Id;

            _shipmentRepository.Receive(createShipmentDto);
            _shipmentRepository.SaveChanges();


            Assert.True(CheckAmount(createdProduct, createdInventory.Id, 10));

            TransferDto transferDto = new TransferDto();
            transferDto.Amount = 10;
            transferDto.Id = inventoryItemToCreateInFirstInventory.Id;
            transferDto.productId = createdProduct.Id;
            transferDto.fromInventoryId = createdInventory.Id;
            transferDto.toInventoryId = createdInventory1.Id;

            TransferDtoMany transferDtoMany = new TransferDtoMany();
            transferDtoMany.transferDtos = new List<TransferDto>();
            transferDtoMany.transferDtos.Add(transferDto);
            transferDtoMany.toInventoryId = createdInventory1.Id;

            Shipment shipmentTransfer = _shipmentRepository.TransferToAnotherInventory(transferDtoMany);
            _shipmentRepository.SaveChanges();

            Assert.True(CheckAmount(createdProduct, createdInventory.Id, 0));
            Assert.True(CheckAmount(createdProduct, createdInventory1.Id, 10));

            _shipmentRepository.Delete(shipmentTransfer.Id);
            _shipmentRepository.SaveChanges();

            Assert.True(CheckAmount(createdProduct, createdInventory.Id, 0));
        }
    }
}