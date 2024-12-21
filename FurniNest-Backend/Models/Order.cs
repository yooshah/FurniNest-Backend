using FurniNest_Backend.Enums;

namespace FurniNest_Backend.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public int  ShippingAddressId { get; set; }

        public string TransactionId { get; set; }

        public virtual User User { get; set; }

        public virtual ShippingAddress ShippingAddress { get; set; }


        public virtual ICollection<OrderItem> OrderItems { get; set; }

    }
}
