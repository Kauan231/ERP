using AutoMapper;
using ERP.Data.Dtos;
using ERP.Models;

namespace ERP.Profiles
{
    public class ShipmentProfile : Profile
    {
        public ShipmentProfile() { 
            CreateMap<Shipment, ReadShipmentDto>();
            CreateMap<ReadShipmentDto, Shipment>();
            CreateMap<CreateShipmentDto, Shipment>();
            CreateMap<Shipment, CreateShipmentDto>();
        }
    }
}
