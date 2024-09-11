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
    public class InventoryTests : IDisposable
    {
        private readonly ErpContext _context;
        private readonly InventoryRepository _inventoryRepository;
        private readonly Mapper _mapper;

        public InventoryTests()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "ErpDevInventory.db" };
            var connectionString = connectionStringBuilder.ToString();

            var options = new DbContextOptionsBuilder<ErpContext>()
                .UseSqlite("DataSource=ErpDevInventory.db")
                .Options;

            _context = new ErpContext(options);
            _context.Database.Migrate();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new InventoryProfile());
            });
            _mapper = new Mapper(config);

            _inventoryRepository = new InventoryRepository(_context, _mapper);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void Creation()
        {
            //Arrange
            User user = Mocks.TestUser();
            _context.Users.Add(user);
            _context.SaveChanges();

            Business business = Mocks.TestBusiness(user.Id);
            _context.Businesses.Add(business);
            _context.SaveChanges();

            CreateInventoryDto createInventoryDto = new CreateInventoryDto();
            createInventoryDto.businessId = business.Id;
            createInventoryDto.Name = Guid.NewGuid().ToString();

            //Act & Assert
            Inventory createdInventory = _inventoryRepository.Create(createInventoryDto);
            _inventoryRepository.SaveChanges();

            ReadInventoryDto readInventoryDto = _inventoryRepository.Read(createdInventory.Id);
            Assert.NotNull(readInventoryDto);
            Assert.Equal(readInventoryDto.Id, createdInventory.Id);
        }


        [Fact]
        public void AddProducts()
        {
            //Arrange
            User user = Mocks.TestUser();
            _context.Users.Add(user);
            _context.SaveChanges();

            Business business = Mocks.TestBusiness(user.Id);
            _context.Businesses.Add(business);
            _context.SaveChanges();

            CreateInventoryDto createInventoryDto = new CreateInventoryDto();
            createInventoryDto.businessId = business.Id;
            createInventoryDto.Name = Guid.NewGuid().ToString();

            Inventory createdInventory = _inventoryRepository.Create(createInventoryDto);
            _inventoryRepository.SaveChanges();

            Product product = Mocks.TestProduct(createdInventory.Id, 10);
            Product product2 = Mocks.TestProduct(createdInventory.Id, 5);

            _context.Products.Add(product);
            _context.Products.Add(product2);
            _context.SaveChanges();

            ReadInventoryDto readInventoryDto = _inventoryRepository.Read(createdInventory.Id);
            Assert.True(readInventoryDto.Products.Contains(product));
            Assert.True(readInventoryDto.Products.Contains(product2));
        }
    }
}