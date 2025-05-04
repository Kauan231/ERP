using AutoMapper;
using ERP.Data.Dtos;
using ERP.Models;

namespace ERP.Profiles
{
    public class InventoryItemProfile : Profile
    {
        public InventoryItemProfile()
        {
            CreateMap<InventoryItem, ReadInventoryItemDto>();
            CreateMap<ReadInventoryItemDto, InventoryItem>();
            CreateMap<CreateInventoryItemDto, InventoryItem>();
            CreateMap<InventoryItem, CreateInventoryItemDto>();
            CreateMap<ReadInventoryItemDto, CreateInventoryItemDto>();
            CreateMap<CreateInventoryItemDto, ReadInventoryItemDto>();
        }
    }
}
