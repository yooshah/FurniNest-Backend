namespace FurniNest_Backend.DTOs.OrderDTOs
{
    public class OrderItemDTO
    {
        public string? ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice { get; set; }


    }
}
