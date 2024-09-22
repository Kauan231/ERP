using AutoMapper;
using ERP.Data.Dtos;
using ERP.Data.Dtos.Domain;
using ERP.Models.Domain;

namespace ERP.Profiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile() { 
            CreateMap<Client, ReadClientDto>();
            CreateMap<ReadClientDto, Client>();
            CreateMap<CreateClientDto, Client>();
            CreateMap<IEnumerable<Client>, Client>();
        }
    }
}
