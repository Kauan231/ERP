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

        public BusinessController(IBusinessRepository businessRepository)
        {
            _businessRepository = businessRepository;
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
    }
}
