namespace FurniNest_Backend.DTOs.OrderDTOs
{
    public class RazorpayPaymentDTO
    {

        public string? razorpay_payment_id { get; set; }
        public string? razorpay_orderId { get; set; }
        public string? razorpay_signature { get; set; }
    }
}
