using ERP.Data.Dtos;
using ERP.Data.Dtos.Domain;
using ERP.Models.Domain;

namespace ERP.Repositories
{
    public interface IClientRepository
    {
        void SaveChanges();
        Client Create(CreateClientDto client);
        ReadClientDto Read(string id);
        ReadAllClientDto ReadAllBusinessClients(string Id, int skip, int limit, string search);
        void Delete(string id);
    }
}
