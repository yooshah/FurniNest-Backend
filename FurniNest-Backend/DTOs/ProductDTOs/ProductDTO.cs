using FurniNest_Backend.Models;
using System.ComponentModel.DataAnnotations;

namespace FurniNest_Backend.DTOs.ProductDTOs
{
    public class ProductDTO
    {

        public int ProductId { get; set; }
       
        public string? Name { get; set; }

        
        public decimal Price { get; set; }

       
        public int Rating { get; set; }

        public string? Image {  get; set; } 


        public string Category { get; set; }

     
        public string? Brand { get; set; }
        
        public int Stock { get; set; }  
    }
}
