using FurniNest_Backend.DTOs.OrderDTOs;
using FurniNest_Backend.Models;

namespace FurniNest_Backend.Services.AddressService
{
    public interface IAddressService
    {

        Task<ApiResponse<string>> CreateShippingAddress(int userUId, OrderAddressDTO orderAddressDTO);

        Task<ApiResponse<List<OrderAddressDTO>>> GetShippingAddress(int userId);

        Task <bool> RemoveShippingAddressByUser(int userId,int addressId);
    }
}
