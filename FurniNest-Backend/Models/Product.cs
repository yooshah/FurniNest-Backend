using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FurniNest_Backend.Models
{
    public class Product
    {

        public int ProductId { get; set; }
        [Required(ErrorMessage = "Product name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Product image is required")]
        public string? Image { get; set; }

        [Required(ErrorMessage = "Rating is required")]

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required]

        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        public string? Brand { get; set; }

        public virtual Category? Category { get; set; }

        public virtual ICollection<CartItem>? CartItems { get; set; }

        public virtual ICollection<WishListItem>? WishListItems { get; set; }

    }
}
