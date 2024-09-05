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
    public class ProductsTests : IDisposable
    {
        private readonly ErpContext _context;
        private readonly ProductRepository _productRepository;
        private readonly Mapper _mapper;

        public ProductsTests()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "ErpDevProducts.db" };
            var connectionString = connectionStringBuilder.ToString();

            var options = new DbContextOptionsBuilder<ErpContext>()
                .UseSqlite("DataSource=ErpDevProducts.db")
                .Options;

            _context = new ErpContext(options);
            _context.Database.Migrate();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ProductProfile());
            });
            _mapper = new Mapper(config);

            _productRepository = new ProductRepository(_context, _mapper);
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

            Inventory inventory = Mocks.TestInventory(user.Id);
            _context.Inventories.Add(inventory);
            _context.SaveChanges();

            CreateProductDto product = new CreateProductDto();
            product.inventoryId = inventory.Id;
            product.Name = "Lorem";
            product.Description = "Description";
            product.Amount = 10;

            CreateProductDto product2 = new CreateProductDto();
            product2.inventoryId = inventory.Id;
            product2.Name = "Ipsum";
            product2.Description = "Description";
            product2.Amount = 5;


            // Normal product creation
            Product createdProduct = _productRepository.Create(product);
            Product createdProduct2 = _productRepository.Create(product2);
            _productRepository.SaveChanges();

            ReadProductDto search = _productRepository.Read(createdProduct.Id);
            Assert.Equal(createdProduct.Name, search.Name);

            ReadProductDto search2 = _productRepository.Read(createdProduct2.Id);
            Assert.Equal(createdProduct2.Name, search2.Name);

            // If user adds same product to the same Inventory
            try
            {
                _productRepository.Create(product);
            } catch (Exception ex)
            {
                Assert.Equal("Product already exists", ex.Message);
            }

            //Subtract amount
            _productRepository.SubtractAmount(search.Id, 5);
            _productRepository.SaveChanges();

            Assert.Equal(5, _productRepository.Read(createdProduct.Id).Amount);

            //Subtract more than existing
            try
            {
                _productRepository.SubtractAmount(search.Id, 10);
            } catch (Exception ex)
            {
                Assert.Equal("Greater than avaliable amount", ex.Message);
            }

            //Add amount 
            _productRepository.AddAmount(search.Id, 5);
            _productRepository.SaveChanges();
            Assert.Equal(10, _productRepository.Read(createdProduct.Id).Amount);

            //Add with wrong id
            try
            {
                _productRepository.SubtractAmount("nonExistentID", 10);
            } catch (Exception ex)
            {
                Assert.Equal("Product does not exist", ex.Message);
            }

        }
    }
}