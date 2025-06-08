using ERP.Data.Dtos;
using ERP.Models;

namespace ERP.Repositories
{
    public interface IProductRepository
    {
        void SaveChanges();
        Product Create(CreateProductDto product);
        ReadProductDto Read(string id);
        List<Product> ReadAll(string userId, int skip = 0, int limit = 10);
        List<Product> ReadAllWithFilter(string userId, string name, int skip = 0, int limit = 10);
        int Count(string userId);
        int CountWithFilter(string userId, string name);
        void Delete(string id);
    }
}
