using AutoMapper;
using ERP.Data.Dtos;
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
