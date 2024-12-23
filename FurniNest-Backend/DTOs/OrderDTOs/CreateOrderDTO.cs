namespace FurniNest_Backend.DTOs.OrderDTOs
{
    public class CreateOrderDTO
    {

        public int AddressId { get; set; }
        public decimal Totalamount { get; set; }
        public string TransactionId { get; set; }

    }
}
