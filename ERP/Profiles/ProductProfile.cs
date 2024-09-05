using AutoMapper;
using ERP.Data.Dtos;
using ERP.Models;

namespace ERP.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile() { 
            CreateMap<Product, ReadProductDto>();
            CreateMap<ReadProductDto, Product>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<Product, CreateProductDto>();
            CreateMap<ReadProductDto,  CreateProductDto>();
            CreateMap<CreateProductDto, ReadProductDto>();
        }
    }
}
