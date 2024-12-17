using System.ComponentModel.DataAnnotations;

namespace FurniNest_Backend.DTOs.ProductDTOs
{
    public class AddProductDTO
    {

        [Required]
        public string? Name { get; set; }

        [Required]

        public decimal Price { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]

        public string? Brand { get; set; }




    }
}
