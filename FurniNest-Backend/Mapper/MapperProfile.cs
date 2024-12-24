using AutoMapper;
using FurniNest_Backend.DTOs.AdminDTO;
using FurniNest_Backend.DTOs.OrderDTOs;
using FurniNest_Backend.DTOs.ProductDTOs;
using FurniNest_Backend.DTOs.UserDTOs;
using FurniNest_Backend.Models;

namespace FurniNest_Backend.Mapper
{
    public class MapperProfile : Profile
    {

     public MapperProfile() 
        {
        CreateMap<User,UserRegisterDTO>().ReverseMap();
        CreateMap<Product,AddProductDTO>().ReverseMap();
        CreateMap<Product,ProductDTO>().ReverseMap();
        CreateMap<ShippingAddress,OrderAddressDTO>().ReverseMap();
        CreateMap<User, UserViewDTO>();

        }
    }
}
