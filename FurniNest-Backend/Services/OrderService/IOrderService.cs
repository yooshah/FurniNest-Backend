using FurniNest_Backend.DTOs.OrderDTOs;
using FurniNest_Backend.Models;

namespace FurniNest_Backend.Services.OrderService
{
    public interface IOrderService
    {
        Task<string> CreateRazorpayOrder(long price);

        Task<bool> VerifyRazorpayPayment(RazorpayPaymentDTO payment);



        Task<bool> CreateOrder(int userId, CreateOrderDTO createOrderDTO);

        Task<ApiResponse<OrderViewDTO>> GetOrderItems(int userId);
        Task<List<AdminViewOrderDTO>> GetUserOrderByAdmin(int userId);

        Task<decimal> TotalRevenue();

        Task<int> TotalProductSold();



    }
}
