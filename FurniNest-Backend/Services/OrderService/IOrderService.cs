using FurniNest_Backend.DTOs.OrderDTOs;
using FurniNest_Backend.Models;

namespace FurniNest_Backend.Services.OrderService
{
    public interface IOrderService
    {
        Task<string> CreateRazorpayOrder(long price);

        Task<bool> Payment(RazorpayPaymentDTO payment);



        Task<bool> CreateOrder(int userId, CreateOrderDTO createOrderDTO);

        Task<ApiResponse<List<OrderViewDTO>>> GetOrderItems(int userId);
        Task<List<AdminViewOrderDTO>> GetUserOrderByAdmin(int userId);

        Task<bool> ChangeOrderStatus(int orderId, string orderStatus);

        Task<decimal> TotalRevenue();

        Task<int> TotalProductSold();

        Task<RevenueRecordDTO> GetRevenueRecords();

        Task<List<OrderItemDTO>> GetOrderItemByOrderId(int orderId);





    }
}
