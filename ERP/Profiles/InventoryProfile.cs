using AutoMapper;
using ERP.Data.Dtos;
using ERP.Data.Dtos.Domain;
using ERP.Models;

namespace ERP.Profiles
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile() { 
            CreateMap<Inventory, ReadInventoryDto>();
            CreateMap<ReadInventoryDto, Inventory>();
            CreateMap<CreateInventoryDto, Inventory>();
            CreateMap<IEnumerable<Inventory>, Inventory>();
        }
    }
}
