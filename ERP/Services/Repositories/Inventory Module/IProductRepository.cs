using ERP.Data.Dtos;
using ERP.Models;

namespace ERP.Repositories
{
    public interface IProductRepository
    {
        void SaveChanges();
        Product Create(CreateProductDto product);
        ReadProductDto Read(string id);
        List<Product> ReadAll();
        void Delete(string id);
    }
}
