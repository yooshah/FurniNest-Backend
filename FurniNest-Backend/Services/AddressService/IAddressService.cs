using FurniNest_Backend.DTOs.AddressDTOs;
using FurniNest_Backend.DTOs.OrderDTOs;
using FurniNest_Backend.Models;

namespace FurniNest_Backend.Services.AddressService
{
    public interface IAddressService
    {

        Task<ApiResponse<int>> CreateShippingAddress(int userUId, OrderAddressDTO orderAddressDTO);

        Task<ApiResponse<List<ViewAddressDTO>>> GetShippingAddress(int userId);

        Task <bool> RemoveShippingAddressByUser(int userId,int addressId);
    }
}
