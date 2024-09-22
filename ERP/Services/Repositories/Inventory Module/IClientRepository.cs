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
        List<Client> ReadAllBusinessClients(string Id);
        void Delete(string id);
    }
}
