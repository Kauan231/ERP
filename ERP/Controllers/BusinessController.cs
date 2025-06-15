using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ERP.Repositories;
using ERP.Data.Dtos.Domain;
using ERP.Models.Domain;

namespace ERP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessRepository _businessRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<BusinessController> _logger;

        public BusinessController(IBusinessRepository businessRepository, IClientRepository clientRepository, ILogger<BusinessController> logger)
        {
            _businessRepository = businessRepository;
            _clientRepository = clientRepository;
            _logger = logger;
        }

        // GET <BusinessController>/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<ReadBusinessDto> Get(string id)
        {
            ReadBusinessDto readBusinessDto = _businessRepository.Read(id);
            return Ok(readBusinessDto);
        }

        // POST <BusinessController>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult<Business> Post(string businessName)
        {
            var userIdClaim = User.Claims.FirstOrDefault(i => i.Type == "id");

            if (userIdClaim == null)
            {
                _logger.LogWarning("Usuário não possui o claim 'id'.");
                return Unauthorized("Usuário não autenticado corretamente.");
            }

            CreateBusinessDto createBusinessDto = new CreateBusinessDto
            {
                userId = userIdClaim.Value,
                Name = businessName
            };

            _logger.LogInformation("Criando Business: UserId: {UserId}, Name: {Name}", createBusinessDto.userId, createBusinessDto.Name);

            Business business = _businessRepository.Create(createBusinessDto);
            _logger.LogInformation("Business criado {businessId}, {Name}", business.Id, business.Name);

            _businessRepository.SaveChanges();

            return Ok(business);
        }

        // DELETE <BusinessController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(string id)
        {
            _businessRepository.Delete(id);
            _businessRepository.SaveChanges();
            return Ok(id);
        }

        [HttpPost("{Id}/addClient")]
        [Authorize(Roles = "admin")]
        public ActionResult<Client> CreateClient([FromRoute] string Id, string ClientName)
        {
            CreateClientDto dto = new CreateClientDto { Name = ClientName, businessId = Id };
            Client client = _clientRepository.Create(dto);
            _clientRepository.SaveChanges();
            return Ok(client);
        }

        [HttpGet("{Id}/Clients")]
        [Authorize(Roles = "admin")]
        public ActionResult<ReadAllClientDto> ReadAllClients(
                [FromRoute] string Id,
                [FromQuery] int skip = 0,
                [FromQuery] int limit = 10,
                [FromQuery] string? search = ""
            )
        {
            ReadAllClientDto clients = _clientRepository.ReadAllBusinessClients(Id, skip, limit, search);
            return Ok(clients);
        }

        [HttpGet("{Id}/Client/{clientId}")]
        [Authorize(Roles = "admin")]
        public ActionResult<ReadClientDto> ReadClient([FromRoute] string clientId)
        {
            ReadClientDto client = _clientRepository.Read(clientId);
            return Ok(client);
        }

        [HttpDelete("{Id}/Client/{clientId}")]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteClient([FromRoute] string clientId)
        {
            _clientRepository.Delete(clientId);
            _clientRepository.SaveChanges();
            return Ok(clientId);
        }
    }
}
