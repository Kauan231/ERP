using AutoMapper;
using ERP.Data.Dtos;
using ERP.Data.Dtos.Domain;
using ERP.Models.Domain;

namespace ERP.Profiles
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile() { 
            CreateMap<Business, ReadBusinessDto>();
            CreateMap<ReadBusinessDto, Business>();
            CreateMap<CreateBusinessDto, Business>();
            CreateMap<IEnumerable<Business>, Business>();
        }
    }
}
