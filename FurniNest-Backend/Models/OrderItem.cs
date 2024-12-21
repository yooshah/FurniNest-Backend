using System.ComponentModel.DataAnnotations;

namespace FurniNest_Backend.Models
{
    public class OrderItem
    {

        public int Id { get; set; }

        [Required(ErrorMessage ="OrderId is Required")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "ProductId is Required")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity is Required")]
        [Range(1, 50, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
            
        [Required(ErrorMessage = "Price is Required")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "TotalPrice is Required")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Total price must be a positive value.")]
        public decimal TotalPrice { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
