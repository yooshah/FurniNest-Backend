namespace FurniNest_Backend.DTOs.OrderDTOs
{
    public class AdminViewOrderDTO
    {
        public string TransactionId { get; set; }

        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
