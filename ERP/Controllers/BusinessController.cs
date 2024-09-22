using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ERP.Repositories;
using ERP.Data.Dtos.Domain;
using ERP.Data.Dtos;
using ERP.Models.Domain;

namespace ERP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IClientRepository _clientRepository;

        public BusinessController(IBusinessRepository businessRepository, IClientRepository clientRepository)
        {
            _businessRepository = businessRepository;
            _clientRepository = clientRepository;
        }

        // GET <BusinessController>/5
        [HttpGet("{id}")]
        [Authorize]
        public ReadBusinessDto Get(string id)
        {
            ReadBusinessDto readBusinessDto = _businessRepository.Read(id);
            return readBusinessDto;
        }

        // POST <BusinessController>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public Business Post(string BusinessName)
        {
            CreateBusinessDto createBusinessDto = new CreateBusinessDto();
            createBusinessDto.userId = this.User.Claims.First(i => i.Type == "id").Value;
            createBusinessDto.Name = BusinessName;
            Business business = _businessRepository.Create(createBusinessDto);
            _businessRepository.SaveChanges();
            return business;
        }

        // DELETE <BusinessController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public void Delete(string id)
        {
            _businessRepository.Delete(id);
            _businessRepository.SaveChanges();
        }

        [HttpPost("{Id}/Client")]
        [Authorize(Roles = "admin")]
        public Client CreateClient([FromRoute] string Id, string ClientName)
        {
            CreateClientDto dto = new CreateClientDto();
            dto.Name = ClientName;
            dto.businessId = Id;

            Client client = _clientRepository.Create(dto);
            _clientRepository.SaveChanges();
            return client;
        }

        [HttpGet("{Id}/Clients")]
        [Authorize(Roles = "admin")]
        public List<Client> ReadAllClients([FromRoute] string Id)
        {
            List<Client> clients = _clientRepository.ReadAllBusinessClients(Id);
            return clients;
        }

        [HttpGet("{Id}/Client/{clientId}")]
        [Authorize(Roles = "admin")]
        public ReadClientDto ReadClient([FromRoute] string clientId)
        {
            ReadClientDto client = _clientRepository.Read(clientId);
            return client;
        }

        [HttpDelete("{Id}/Client/{clientId}")]
        [Authorize(Roles = "admin")]
        public void DeleteClient([FromRoute] string clientId)
        {
            _clientRepository.Delete(clientId);
            _clientRepository.SaveChanges();
        }
    }
}
