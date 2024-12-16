using AutoMapper;
using FurniNest_Backend.DTOs.UserDTOs;
using FurniNest_Backend.Models;

namespace FurniNest_Backend.Mapper
{
    public class MapperProfile : Profile
    {

     public MapperProfile() 
        {
        CreateMap<User,UserRegisterDTO>().ReverseMap();

        }
    }
}
