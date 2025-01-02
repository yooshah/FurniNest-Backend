﻿using FurniNest_Backend.DTOs.CartDTOs;
using FurniNest_Backend.Models;

namespace FurniNest_Backend.Services.CartService
{
    public interface ICartService
    {

        Task<ApiResponse<CartItemViewDTO>> AddToCart(int userId, int productId);

        Task<ApiResponse<List<CartItemViewDTO>>> ViewCartByUser(int userId);

        Task <ApiResponse<string>> DeleteCartByUser(int userId,int CartItemId);

        Task<ApiResponse<bool>> IncreaseCartItemQuantity(int userId, int CartItemId);

        Task<bool>DecresaeQuantity(int userId, int CartItemId);
    }
}
