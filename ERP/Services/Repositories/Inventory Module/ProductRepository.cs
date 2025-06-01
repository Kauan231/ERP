using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ERP.Data;
using ERP.Data.Dtos;
using ERP.Models;

namespace ERP.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ErpContext _context;
        private readonly IMapper _mapper;
        public ProductRepository(ErpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Product Create(CreateProductDto createDto)
        {
            Product product = _mapper.Map<Product>(createDto);
            Product productSearch = _context.Products.SingleOrDefault(product => product.Name == createDto.Name);
            if (productSearch != null) throw new Exception("Product already exists");

            var guid = Guid.NewGuid().ToString();
            product.Id = guid;

            _context.Products.Add(product);
            return product;
        }

        public Product ModifyItem(Product product)
        {
            Product productInInventory = _context.Products.SingleOrDefault(x => x.Id == product.Id);
            if (productInInventory == null) throw new Exception("Product does not exist");
            productInInventory = product;
            return productInInventory;
        }

        public ReadProductDto Read(string id)
        {
            Product Product = _context.Products.FirstOrDefault(x => x.Id == id);
            ReadProductDto dto = _mapper.Map<ReadProductDto>(Product);
            return dto;
        }

        public List<Product> ReadAll(int skip = 0, int limit = 10)
        {
            List<Product> products = _context.Products.Skip(skip).Take(limit).ToList();
            return products;
        }

        public List<Product> ReadAllWithFilter(string name, int skip = 0, int limit = 10)
        {
            List<Product> products = _context.Products.Where(product => EF.Functions.Like(product.Name, $"%{name}%")).Skip(skip).Take(limit).ToList();
            return products;
        }

        public int Count()
        {
            return _context.Products.Count();
        }

        public int CountWithFilter(string name)
        {
            return _context.Products.Where(product => EF.Functions.Like(product.Name, $"%{name}%")).Count();
        }

        public void Delete(string id)
        {
            Product Product = _context.Products.SingleOrDefault(x => x.Id == id);
            if (Product != null)
            {
                _context.Products.Remove(Product);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
