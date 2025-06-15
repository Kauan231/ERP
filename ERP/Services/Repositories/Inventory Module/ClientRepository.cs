using AutoMapper;
using ERP.Data;
using ERP.Data.Dtos;
using ERP.Data.Dtos.Domain;
using ERP.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ERP.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ErpContext _context;
        private readonly IMapper _mapper;
        public ClientRepository(ErpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public Client Create(CreateClientDto dto)
        {
            Client client = _mapper.Map<Client>(dto);
            client.Id = Guid.NewGuid().ToString();
            _context.Clients.Add(client);
            return client;
        }

        public ReadClientDto Read(string Id)
        {
            Client client = _context.Clients.SingleOrDefault(x => x.Id == Id);
            ReadClientDto dto = _mapper.Map<ReadClientDto>(client);
            return dto;
        }

        public ReadAllClientDto ReadAllBusinessClients(string Id, int skip, int limit, string search)
        {
            IQueryable<Client> query = _context.Clients.Where(x => x.businessId == Id);
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(client => EF.Functions.Like(client.Name, $"%{search}%"));
            }
            int totalOfItems = _context.Clients.Where(x => x.businessId == Id).Count();

            List<Client> clients = query.Skip(skip).Take(limit).ToList();
            ReadAllClientDto readAllClientDto = new ReadAllClientDto();
            readAllClientDto.clients = clients;
            readAllClientDto.totalOfItems = totalOfItems;
            return readAllClientDto;
        }

        public void Delete(string clientId)
        {
            Client client = _context.Clients.SingleOrDefault(x => x.Id == clientId);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }
        }
    }
}
