using AutoMapper;
using ERP.Data.Dtos;
using ERP.Models;

namespace ERP.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile() { 
            CreateMap<User, CreateUserDto>();
            CreateMap<CreateUserDto, User>();
        }
    }
}
