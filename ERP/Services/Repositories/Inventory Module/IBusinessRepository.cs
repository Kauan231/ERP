using ERP.Data.Dtos;
using ERP.Data.Dtos.Domain;
using ERP.Models.Domain;

namespace ERP.Repositories
{
    public interface IBusinessRepository
    {
        void SaveChanges();
        Business Create(CreateBusinessDto business);
        ReadBusinessDto Read(string id);
        void Delete(string id);
        List<Business> ReadAllUserBusinesses(string userId);
    }
}
