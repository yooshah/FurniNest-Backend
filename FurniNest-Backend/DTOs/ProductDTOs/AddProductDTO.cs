using System.ComponentModel.DataAnnotations;

namespace FurniNest_Backend.DTOs.ProductDTOs
{
    public class AddProductDTO
    {

        
        [Required(ErrorMessage = "Product name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal Price { get; set; }


        [Required(ErrorMessage = "Rating is required")]

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required]

        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Brand is required")]
        public string? Brand { get; set; }

        [Required (ErrorMessage ="Stock is Required")] 
        
        public int Stock {  get; set; } 
        





    }
}
