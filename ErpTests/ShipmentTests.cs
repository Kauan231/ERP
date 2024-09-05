using AutoMapper;
using ERP.Data;
using ERP.Data.Dtos;
using ERP.Profiles;
using ERP.Models;
using ERP.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ErpTests
{
    public class ShipmentTests : IDisposable
    {
        private readonly ErpContext _context;
        private readonly InventoryRepository _inventoryRepository;
        private readonly ShipmentRepository _shipmentRepository;
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
            });
            _mapper = new Mapper(config);

            _inventoryRepository = new InventoryRepository(_context, _mapper);
            _shipmentRepository = new ShipmentRepository(_context, _mapper);
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

            foreach (Product p in readInventoryDto.Products)
            {
                if (p.Name == product.Name)
                {
                    isAmount = (p.Amount == amountExpected);
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

            Inventory createdInventory = Mocks.TestInventory(user.Id);
            Inventory createdInventory1 = Mocks.TestInventory(user1.Id);
            _context.Inventories.Add(createdInventory);
            _context.Inventories.Add(createdInventory1);
            _context.SaveChanges();

            Product createdProduct = Mocks.TestProduct(createdInventory.Id, 0);
            _context.Products.Add(createdProduct);
            _context.SaveChanges();

            //Act & Assert

            Assert.True(CheckAmount(createdProduct, createdInventory.Id, 0));

            CreateShipmentDto createShipmentDto = new CreateShipmentDto();
            createShipmentDto.Amount = 10;
            createShipmentDto.productId = createdProduct.Id;     
            _shipmentRepository.Receive(createShipmentDto);
            _shipmentRepository.SaveChanges();

            Assert.True(CheckAmount(createdProduct, createdInventory.Id, 10));

            TransferDto transferDto = new TransferDto();
            transferDto.Amount = 10;
            transferDto.productId = createdProduct.Id;
            transferDto.toInventoryId = createdInventory1.Id;

            Shipment shipmentTransfer = _shipmentRepository.TransferToAnotherInventory(transferDto);
            _shipmentRepository.SaveChanges();

            Assert.True(CheckAmount(createdProduct, createdInventory.Id, 0));
            Assert.True(CheckAmount(createdProduct, createdInventory1.Id, 10));

            _shipmentRepository.Delete(shipmentTransfer.Id);
            _shipmentRepository.SaveChanges();

            Assert.True(CheckAmount(createdProduct, createdInventory1.Id, 0));
        }
    }
}