using ERP.Data.Dtos;
using ERP.Models;

namespace ERP.Repositories
{
    public interface IProductRepository
    {
        void SaveChanges();
        Product Create(CreateProductDto product);
        ReadProductDto Read(string id);
        List<Product> ReadAll(int skip = 0, int limit = 10);
        int Count();
        void Delete(string id);
    }
}
