using FurniNest_Backend.Enums;

namespace FurniNest_Backend.DTOs.OrderDTOs
{
    public class OrderViewDTO
    {



        public string TransactionId { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }

        public string? DeliveryAdrress { get; set; }

        public string? Phone {  get; set; }
        public DateTime OrderDate { get; set; } 

        public List<OrderItemDTO> Items { get; set; }

    }
}
