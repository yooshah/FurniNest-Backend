using FurniNest_Backend.DTOs.ProductDTOs;
using FurniNest_Backend.Models;

namespace FurniNest_Backend.Services.WishListService
{
    public interface IWishListService
    {

        Task<ApiResponse<string>> AddToWishList(int userId, int productId);
        Task<ApiResponse<string>> RemoveFromWishList(int userId, int productId);

        Task<ApiResponse<List<ProductDTO>>> GetWishListProducts(int userId);
    }
}
