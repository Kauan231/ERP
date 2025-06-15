using ERP.Data.Dtos;
using ERP.Models;

namespace ERP.Repositories
{
    public interface IProductRepository
    {
        void SaveChanges();
        Product Create(CreateProductDto product);
        ReadProductDto Read(string id);
        List<Product> ReadAll(string businessId, int skip = 0, int limit = 10);
        List<Product> ReadAllWithFilter(string businessId, string name, int skip = 0, int limit = 10);
        int Count(string businessId);
        int CountWithFilter(string businessId, string name);
        void Delete(string id);
    }
}
